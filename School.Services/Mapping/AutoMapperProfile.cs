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
                              || "Create" + source.Name == destination.Name.Replace("Dto", "")
                              || "Update" + source.Name == destination.Name.Replace("Dto", "")
                           select (Source: source, Destination: destination);


            foreach (var (source, destination) in mappings)
            {
                if (source.Name == "Employee" && destination.Name == "EmployeeDto")
                {
                    continue;
                }
                if (source.Name == "SchoolRegistration" && (destination.Name == "SchoolRegistrationModel" || destination.Name == "SchoolRegistrationDto"))
                {
                    continue;
                }
                if (destination.Name.Contains("Dto"))
                {
                    var createdDateProp = destination.GetProperty("CreatedDate");
                    var updatedDateProp = destination.GetProperty("UpdatedDate");
                    
                    if (createdDateProp != null || updatedDateProp != null)
                    {
                        var mapConfig = CreateMap(source, destination);
                        
                        if (createdDateProp != null && createdDateProp.PropertyType == typeof(string))
                        {
                            mapConfig.ForMember("CreatedDate",
                                opt => opt.MapFrom((src, dest, destMember, context) =>
                                {
                                    var createdDate = src.GetType().GetProperty("CreatedDate")?.GetValue(src);
                                    return createdDate is DateTime dateTime
                                        ? dateTime.ToString("dd-MM-yyyy hh:mm:ss tt")
                                        : null;
                                }
                            ));
                        }
                        
                        if (updatedDateProp != null && updatedDateProp.PropertyType == typeof(string))
                        {
                            mapConfig.ForMember("UpdatedDate",
                                opt => opt.MapFrom((src, dest, destMember, context) =>
                                {
                                    var updatedDate = src.GetType().GetProperty("UpdatedDate")?.GetValue(src);
                                    return updatedDate is DateTime dateTime
                                        ? dateTime.ToString("dd-MM-yyyy hh:mm:ss tt")
                                        : null;
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

            CreateMap<global::School.Domain.School.SchoolOwner, global::School_DTOs.School.SchoolOwnerDto>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolRegistration != null ? src.SchoolRegistration.SchoolName : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.UserName : string.Empty));

            CreateMap<global::School.Domain.Student.StudentRegistration, global::School_DTOs.Student.StudentRegistrationDto>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => ParseDateOfBirth(src.DateOfBirth)))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : string.Empty));

            CreateMap<global::School.Domain.Hr.Employee, global::School_DTOs.Hr.EmployeeDto>()
                .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.FatherName : null))
                .ForMember(dest => dest.MotherName, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.MotherName : null))
                .ForMember(dest => dest.AadhaarNumber, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.AadhaarNumber : null))
                .ForMember(dest => dest.PANNumber, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.PANNumber : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.Address : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.City : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.State : null))
                .ForMember(dest => dest.PinCode, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.PinCode : null))
                .ForMember(dest => dest.BloodGroup, opt => opt.MapFrom(src => src.EmployeeDetail != null && src.EmployeeDetail.BloodGroup != null ? src.EmployeeDetail.BloodGroup.Name : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.DesignationName, opt => opt.MapFrom(src => src.Designation != null ? src.Designation.Name : string.Empty));

            CreateMap<global::School.Domain.School.SchoolRegistration, global::School.Models.School.SchoolRegistrationModel>();
            CreateMap<global::School.Models.School.SchoolRegistrationModel, global::School.Domain.School.SchoolRegistration>()
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.SchoolProfileSetting, opt => opt.Ignore())
                .ForMember(dest => dest.SchoolSubscriptions, opt => opt.Ignore())
                .ForMember(dest => dest.AffiliationBoard, opt => opt.Ignore())
                .ForMember(dest => dest.SchoolType, opt => opt.Ignore());

            CreateMap<global::School.Domain.School.SchoolRegistration, global::School_DTOs.School.SchoolRegistrationDto>();

            CreateMap<global::School.Domain.Hr.Recruitment.JobPosting, global::School_DTOs.Hr.JobPostingDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));

            CreateMap<global::School.Domain.Hr.Recruitment.JobApplication, global::School_DTOs.Hr.JobApplicationDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPosting != null ? src.JobPosting.Title : string.Empty))
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.FullName : string.Empty))
                .ForMember(dest => dest.CandidateEmail, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.Email : string.Empty))
                .ForMember(dest => dest.CandidatePhone, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.Phone : string.Empty))
                .ForMember(dest => dest.CandidateResumePath, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.ResumePath : null));

            CreateMap<global::School.Domain.Hr.Performance.PerformanceReview, global::School_DTOs.Hr.PerformanceReviewDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Designation != null ? src.Employee.Designation.Name : string.Empty))
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer != null ? (src.Reviewer.FirstName + " " + src.Reviewer.LastName) : string.Empty));

            CreateMap<global::School.Domain.Hr.Training.TrainingEnrollment, global::School_DTOs.Hr.TrainingEnrollmentDto>()
                .ForMember(dest => dest.TrainingTitle, opt => opt.MapFrom(src => src.TrainingProgram != null ? src.TrainingProgram.Title : string.Empty))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty));

            CreateMap<global::School.Domain.Hr.Assets.AssetAssignment, global::School_DTOs.Hr.AssetAssignmentDto>()
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.SchoolAsset != null ? src.SchoolAsset.Name : string.Empty))
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.SchoolAsset != null ? src.SchoolAsset.AssetCode : string.Empty))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty));
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
