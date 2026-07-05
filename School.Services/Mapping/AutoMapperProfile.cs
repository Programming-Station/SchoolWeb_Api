using AutoMapper; 
using System.Reflection;
namespace School.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        { 


            var domainAssembly = Assembly.Load("School.Domain");  // Domain project assembly
            var dtoAssembly = Assembly.Load("School_DTOs");        // DTO project assembly
            var modelAssembly = Assembly.Load("School.Models");   // Models project assembly

            var sourceTypes = domainAssembly.GetExportedTypes();      // Domain types
            var destinationTypes = dtoAssembly.GetExportedTypes()     // DTO types
                .Concat(modelAssembly.GetExportedTypes());            // Models types

            var mappings = from source in sourceTypes
                           from destination in destinationTypes
                           where source.Name == destination.Name.Replace("Dto", "")
                              || source.Name == destination.Name.Replace("Model", "")
                           select (Source: source, Destination: destination);


            foreach (var (source, destination) in mappings)
            {
                if (destination.Name.Contains("Dto"))
                {
                    var createdDateProp = destination.GetProperty("CreatedDate");
                    var updatedDateProp = destination.GetProperty("UpdatedDate");
                    
                    if (createdDateProp != null || updatedDateProp != null)
                    {
                        var mapConfig = CreateMap(source, destination);
                        
                        if (createdDateProp != null)
                        {
                            mapConfig.ForMember("CreatedDate",
                                opt => opt.MapFrom((src, dest, destMember, context) =>
                                {
                                    var createdDate = src.GetType().GetProperty("CreatedDate")?.GetValue(src);
                                    return createdDate is DateTime dateTime
                                        ? dateTime.ToString("dd-MM-yyyy hh:mm:ss tt")
                                        : string.Empty;
                                }
                            ));
                        }
                        
                        if (updatedDateProp != null)
                        {
                            mapConfig.ForMember("UpdatedDate",
                                opt => opt.MapFrom((src, dest, destMember, context) =>
                                {
                                    var updatedDate = src.GetType().GetProperty("UpdatedDate")?.GetValue(src);
                                    return updatedDate is DateTime dateTime
                                        ? dateTime.ToString("dd-MM-yyyy hh:mm:ss tt")
                                        : string.Empty;
                                }
                            ));
                        }
                    }
                    else
                    {
                        CreateMap(source, destination);
                    }

                }
                else
                {
                    CreateMap(source, destination).ReverseMap();

                }
            }
             

            CreateMap<global::School.Domain.Event, global::School_DTOs.Event.EventDto>()
                .ForMember(dest => dest.EventDate,
                    opt => opt.MapFrom(src => src.EventDate.ToString("dd-MM-yyyy hh:mm:ss tt")));  

        }

        private static DateTime ParseDateOfBirth(string? dateOfBirth)
        {
            if (string.IsNullOrEmpty(dateOfBirth))
                return DateTime.MinValue;
            
            if (DateTime.TryParseExact(dateOfBirth, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                return parsedDate;
            
            if (DateTime.TryParse(dateOfBirth, out DateTime parsedDate2))
                return parsedDate2;
            
            return DateTime.MinValue;
        }
    }
}
