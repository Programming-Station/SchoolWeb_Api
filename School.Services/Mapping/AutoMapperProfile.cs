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
                if (source.Name == "DashboardConfig" || source.Name == "DashboardWidget" || source.Name == "ReportTemplate" || source.Name == "AiChatSession" || source.Name == "AiPrediction")
                {
                    continue;
                }
                if (destination.Name.Contains("Dto"))
                {
                    if (destination.Name.StartsWith("Create") || destination.Name.StartsWith("Update"))
                    {
                        CreateMap(destination, source).ReverseMap();
                    }
                    else
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
                }
                else
                {
                    CreateMap(source, destination).ReverseMap();
                }
            }

            // Register mappings for generic HR Master entities to HrMasterDto, CreateHrMasterDto, and UpdateHrMasterDto
            var hrMasterTypes = sourceTypes.Where(t => 
                t.IsClass && 
                !t.IsAbstract &&
                typeof(global::School.Domain.BaseEntity.IAuditEntity<int>).IsAssignableFrom(t) &&
                typeof(global::School.Domain.BaseEntity.ITenantEntity).IsAssignableFrom(t));

            foreach (var type in hrMasterTypes)
            {
                CreateMap(type, typeof(global::School_DTOs.Hr.HrMasterDto)).ReverseMap();
                CreateMap(typeof(global::School_DTOs.Hr.CreateHrMasterDto), type);
                CreateMap(typeof(global::School_DTOs.Hr.UpdateHrMasterDto), type);
            }
             

            CreateMap<global::School.Domain.Event, global::School_DTOs.Event.EventDto>()
                .ForMember(dest => dest.EventDate,
                    opt => opt.MapFrom(src => src.EventDate.ToString("dd-MM-yyyy hh:mm:ss tt")));  

            CreateMap<global::School.Domain.School.SchoolOwner, global::School_DTOs.School.SchoolOwnerDto>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolRegistration != null ? src.SchoolRegistration.SchoolName : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.UserName : string.Empty));

            CreateMap<global::School.Domain.Hr.Employee, global::School_DTOs.Hr.EmployeeDto>()
                .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.FatherName : null))
                .ForMember(dest => dest.MotherName, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.MotherName : null))
                .ForMember(dest => dest.AadhaarNumber, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.AadhaarNumber : null))
                .ForMember(dest => dest.PANNumber, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.PANNumber : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.Address : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.City : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.State : null))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.Country : null))
                .ForMember(dest => dest.PinCode, opt => opt.MapFrom(src => src.EmployeeDetail != null ? src.EmployeeDetail.PinCode : null))
                .ForMember(dest => dest.BloodGroup, opt => opt.MapFrom(src => src.EmployeeDetail != null && src.EmployeeDetail.BloodGroup != null ? src.EmployeeDetail.BloodGroup.Name : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.DesignationName, opt => opt.MapFrom(src => src.Designation != null ? src.Designation.Name : string.Empty));

            CreateMap<global::School.Domain.School.SchoolRegistration, global::School.Models.School.SchoolRegistrationModel>();
            CreateMap<global::School.Models.School.SchoolRegistrationModel, global::School.Domain.School.SchoolRegistration>()
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.AffiliationBoard, opt => opt.Ignore())
                .ForMember(dest => dest.SchoolType, opt => opt.Ignore());

            CreateMap<global::School.Domain.School.SchoolProfileSetting, global::School_DTOs.School.SchoolProfileSettingDto>().ReverseMap();
            CreateMap<global::School.Domain.School.SchoolProfileSetting, global::School.Models.School.SchoolProfileSettingModel>().ReverseMap();
            CreateMap<global::School.Domain.School.SchoolSubscription, global::School_DTOs.School.SchoolSubscriptionDto>().ReverseMap();
            CreateMap<global::School.Domain.School.SchoolSubscription, global::School.Models.School.SchoolSubscriptionModel>().ReverseMap();

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

            // Communication - Recipients Module
            CreateMap<global::School.Domain.Communication.Recipients.Recipient, global::School_DTOs.Communication.Recipients.RecipientDto>()
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.GroupMembers.Select(gm => gm.Group)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName)));

            CreateMap<global::School.Domain.Communication.Recipients.RecipientGroup, global::School_DTOs.Communication.Recipients.RecipientGroupDto>()
                .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count));

            // Hostel Module Custom Projections
            CreateMap<global::School.Domain.Hostel.Building, global::School_DTOs.Hostel.BuildingDto>()
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Hostel != null ? src.Hostel.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.Floor, global::School_DTOs.Hostel.FloorDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : string.Empty))
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Building != null && src.Building.Hostel != null ? src.Building.Hostel.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.Room, global::School_DTOs.Hostel.RoomDto>()
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Hostel != null ? src.Hostel.Name : string.Empty))
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : string.Empty))
                .ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.Floor != null ? src.Floor.FloorNumber : 0))
                .ForMember(dest => dest.RoomCategoryName, opt => opt.MapFrom(src => src.RoomCategory != null ? src.RoomCategory.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.Bed, global::School_DTOs.Hostel.BedDto>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : string.Empty))
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Room != null && src.Room.Hostel != null ? src.Room.Hostel.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelWarden, global::School_DTOs.Hostel.HostelWardenDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty))
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EmployeeCode : string.Empty))
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Hostel != null ? src.Hostel.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelAdmission, global::School_DTOs.Hostel.HostelAdmissionDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Student != null && src.Student.Class != null ? src.Student.Class.Name : string.Empty))
                .ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.Student != null && src.Student.Class != null ? src.Student.Class.Section : string.Empty))
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Hostel != null ? src.Hostel.Name : string.Empty))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : string.Empty))
                .ForMember(dest => dest.BedNumber, opt => opt.MapFrom(src => src.Bed != null ? src.Bed.BedNumber : string.Empty))
                .ForMember(dest => dest.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear != null ? src.AcademicYear.YearName : string.Empty));

            CreateMap<global::School.Domain.Hostel.MessMenu, global::School_DTOs.Hostel.MessMenuDto>()
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Hostel != null ? src.Hostel.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.MealAttendance, global::School_DTOs.Hostel.MealAttendanceDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelVisitor, global::School_DTOs.Hostel.HostelVisitorDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelGatePass, global::School_DTOs.Hostel.HostelGatePassDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Admission != null && src.Admission.Room != null ? src.Admission.Room.RoomNumber : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelAttendance, global::School_DTOs.Hostel.HostelAttendanceDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty))
                .ForMember(dest => dest.RoomNumber, opt => opt.Ignore());

            CreateMap<global::School.Domain.Hostel.HostelComplaint, global::School_DTOs.Hostel.HostelComplaintDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.RoomNumber, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedStaffName, opt => opt.MapFrom(src => src.AssignedStaff != null ? (src.AssignedStaff.FirstName + " " + src.AssignedStaff.LastName) : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelMaintenance, global::School_DTOs.Hostel.HostelMaintenanceDto>()
                .ForMember(dest => dest.ComplaintDescription, opt => opt.MapFrom(src => src.Complaint != null ? src.Complaint.Description : string.Empty));

            CreateMap<global::School.Domain.Hostel.LaundryTransaction, global::School_DTOs.Hostel.LaundryTransactionDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelInventory, global::School_DTOs.Hostel.HostelInventoryDto>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : string.Empty))
                .ForMember(dest => dest.HostelName, opt => opt.MapFrom(src => src.Room != null && src.Room.Hostel != null ? src.Room.Hostel.Name : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelMedicalLog, global::School_DTOs.Hostel.HostelMedicalLogDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.IsolationRoomNumber, opt => opt.MapFrom(src => src.IsolationRoom != null ? src.IsolationRoom.RoomNumber : string.Empty));

            CreateMap<global::School.Domain.Hostel.HostelDiscipline, global::School_DTOs.Hostel.HostelDisciplineDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty));

            // ══════════════════════════════════════════════════════════════════════════
            // TRANSPORT MODULE MAPPINGS
            // ══════════════════════════════════════════════════════════════════════════
            CreateMap<global::School.Domain.Transport.Vehicle, global::School_DTOs.Transport.VehicleDto>();
            CreateMap<global::School_DTOs.Transport.CreateVehicleDto, global::School.Domain.Transport.Vehicle>();

            CreateMap<global::School.Domain.Transport.TransportRoute, global::School_DTOs.Transport.TransportRouteDto>();
            CreateMap<global::School_DTOs.Transport.CreateTransportRouteDto, global::School.Domain.Transport.TransportRoute>();

            CreateMap<global::School.Domain.Transport.TransportStop, global::School_DTOs.Transport.TransportStopDto>();
            CreateMap<global::School_DTOs.Transport.CreateTransportStopDto, global::School.Domain.Transport.TransportStop>();

            CreateMap<global::School.Domain.Transport.RouteStopMapping, global::School_DTOs.Transport.RouteStopMappingDto>()
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.RouteName : string.Empty))
                .ForMember(dest => dest.StopName, opt => opt.MapFrom(src => src.Stop != null ? src.Stop.StopName : string.Empty));

            CreateMap<global::School.Domain.Transport.Conductor, global::School_DTOs.Transport.ConductorDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty))
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EmployeeCode : string.Empty));
            CreateMap<global::School_DTOs.Transport.CreateConductorDto, global::School.Domain.Transport.Conductor>();

            CreateMap<global::School.Domain.Transport.RouteAssignment, global::School_DTOs.Transport.RouteAssignmentDto>()
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.RouteName : string.Empty))
                .ForMember(dest => dest.RouteCode, opt => opt.MapFrom(src => src.Route != null ? src.Route.RouteCode : string.Empty))
                .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Name : string.Empty))
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.RegistrationNumber : string.Empty))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? (src.Driver.FirstName + " " + src.Driver.LastName) : string.Empty))
                .ForMember(dest => dest.DriverPhone, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.MobileNumber : string.Empty))
                .ForMember(dest => dest.ConductorName, opt => opt.MapFrom(src => src.Conductor != null && src.Conductor.Employee != null ? (src.Conductor.Employee.FirstName + " " + src.Conductor.Employee.LastName) : string.Empty))
                .ForMember(dest => dest.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear != null ? src.AcademicYear.YearName : string.Empty))
                .ForMember(dest => dest.BackupVehicleName, opt => opt.MapFrom(src => src.BackupVehicle != null ? src.BackupVehicle.Name : string.Empty))
                .ForMember(dest => dest.BackupDriverName, opt => opt.MapFrom(src => src.BackupDriver != null ? (src.BackupDriver.FirstName + " " + src.BackupDriver.LastName) : string.Empty));
            CreateMap<global::School_DTOs.Transport.CreateRouteAssignmentDto, global::School.Domain.Transport.RouteAssignment>();

            CreateMap<global::School.Domain.Transport.TransportAllocation, global::School_DTOs.Transport.TransportAllocationDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.AdmissionNumber, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentId : string.Empty))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Student != null && src.Student.Class != null ? src.Student.Class.Name : string.Empty))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty))
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EmployeeCode : string.Empty))
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.TransportRoute != null ? src.TransportRoute.RouteName : string.Empty))
                .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.TransportRoute != null && src.TransportRoute.Vehicle != null ? src.TransportRoute.Vehicle.Name : string.Empty))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.TransportRoute != null && src.TransportRoute.Vehicle != null ? src.TransportRoute.Vehicle.DriverName : string.Empty))
                .ForMember(dest => dest.DriverPhone, opt => opt.MapFrom(src => src.TransportRoute != null && src.TransportRoute.Vehicle != null ? src.TransportRoute.Vehicle.DriverPhone : string.Empty))
                .ForMember(dest => dest.PickupStopName, opt => opt.MapFrom(src => src.PickupStop != null ? src.PickupStop.StopName : string.Empty))
                .ForMember(dest => dest.DropStopName, opt => opt.MapFrom(src => src.DropStop != null ? src.DropStop.StopName : string.Empty));
            // Enable ReverseMap for TransportAllocation
            CreateMap<global::School_DTOs.Transport.CreateTransportAllocationDto, global::School.Domain.Transport.TransportAllocation>().ReverseMap();

            // Existing TransportTrip mapping (already present)
            CreateMap<global::School.Domain.Transport.TransportTrip, global::School_DTOs.Transport.TransportTripDto>()
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.RouteName : string.Empty))
                .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Name : string.Empty))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? (src.Driver.FirstName + " " + src.Driver.LastName) : string.Empty))
                .ForMember(dest => dest.ConductorName, opt => opt.MapFrom(src => src.Conductor != null && src.Conductor.Employee != null ? (src.Conductor.Employee.FirstName + " " + src.Conductor.Employee.LastName) : string.Empty))
                .ReverseMap();

            CreateMap<global::School.Domain.Transport.RfidScanLog, global::School_DTOs.Transport.RfidScanLogDto>()
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Trip != null && src.Trip.Route != null ? src.Trip.Route.RouteName : string.Empty))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? (src.Employee.FirstName + " " + src.Employee.LastName) : string.Empty));

            CreateMap<global::School.Domain.Transport.FuelLog, global::School_DTOs.Transport.FuelLogDto>()
                .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Name : string.Empty));
            CreateMap<global::School_DTOs.Transport.CreateFuelLogDto, global::School.Domain.Transport.FuelLog>();

            CreateMap<global::School.Domain.Transport.VehicleMaintenance, global::School_DTOs.Transport.VehicleMaintenanceDto>()
                .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Name : string.Empty));
            CreateMap<global::School_DTOs.Transport.CreateVehicleMaintenanceDto, global::School.Domain.Transport.VehicleMaintenance>();

            CreateMap<global::School.Domain.Transport.VehicleIncident, global::School_DTOs.Transport.VehicleIncidentDto>()
                .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Name : string.Empty));
            CreateMap<global::School_DTOs.Transport.CreateVehicleIncidentDto, global::School.Domain.Transport.VehicleIncident>();

            CreateMap<global::School.Domain.Transport.TransportInventory, global::School_DTOs.Transport.TransportInventoryDto>();
            // Enable ReverseMap for TransportInventory
            CreateMap<global::School_DTOs.Transport.CreateTransportInventoryDto, global::School.Domain.Transport.TransportInventory>().ReverseMap();

            CreateMap<global::School.Domain.Transport.TransportGateLog, global::School_DTOs.Transport.TransportGateLogDto>();
            // Enable ReverseMap for TransportGateLog
            CreateMap<global::School_DTOs.Transport.CreateTransportGateLogDto, global::School.Domain.Transport.TransportGateLog>().ReverseMap();

            // Custom mappings for Analytics and AI modules
            CreateMap<global::School.Domain.Analytics.DashboardConfig, global::School_DTOs.Analytics.DashboardConfigDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleScope ?? string.Empty))
                .ForMember(dest => dest.LayoutJson, opt => opt.MapFrom(src => "{}"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null));

            CreateMap<global::School_DTOs.Analytics.DashboardConfigDto, global::School.Domain.Analytics.DashboardConfig>()
                .ForMember(dest => dest.RoleScope, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role + " Dashboard"));

            CreateMap<global::School.Domain.Analytics.DashboardWidget, global::School_DTOs.Analytics.DashboardWidgetDto>()
                .ForMember(dest => dest.DataSourceUrl, opt => opt.MapFrom(src => src.SourceApiUrl))
                .ForMember(dest => dest.ColSpan, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.RowSpan, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null));

            CreateMap<global::School_DTOs.Analytics.DashboardWidgetDto, global::School.Domain.Analytics.DashboardWidget>()
                .ForMember(dest => dest.SourceApiUrl, opt => opt.MapFrom(src => src.DataSourceUrl))
                .ForMember(dest => dest.ConfigJson, opt => opt.MapFrom(src => "{}"));

            CreateMap<global::School.Domain.Analytics.ReportTemplate, global::School_DTOs.Analytics.ReportTemplateDto>()
                .ForMember(dest => dest.ReportType, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.LayoutTemplate, opt => opt.MapFrom(src => src.SelectedColumnsJson))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null));

            CreateMap<global::School_DTOs.Analytics.ReportTemplateDto, global::School.Domain.Analytics.ReportTemplate>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ReportType))
                .ForMember(dest => dest.SelectedColumnsJson, opt => opt.MapFrom(src => src.LayoutTemplate));

            CreateMap<global::School.Domain.AI.AiChatSession, global::School_DTOs.AI.AiChatSessionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null));

            CreateMap<global::School_DTOs.AI.AiChatSessionDto, global::School.Domain.AI.AiChatSession>();

            CreateMap<global::School.Domain.AI.AiPrediction, global::School_DTOs.AI.AiPredictionDto>()
                .ForMember(dest => dest.TargetEntityName, opt => opt.MapFrom(src => "Entity #" + src.TargetEntityId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : null));

            CreateMap<global::School_DTOs.AI.AiPredictionDto, global::School.Domain.AI.AiPrediction>();

            // New Mappings
            CreateMap<global::School.Domain.Administration.Complaint, global::School_DTOs.Administration.ComplaintDto>().ReverseMap();
            CreateMap<global::School.Domain.Administration.Complaint, global::School_DTOs.Administration.CreateComplaintDto>().ReverseMap();
            CreateMap<global::School.Domain.Administration.Visitor, global::School_DTOs.Administration.VisitorDto>().ReverseMap();
            CreateMap<global::School.Domain.Administration.Visitor, global::School_DTOs.Administration.CreateVisitorDto>().ReverseMap();
            CreateMap<global::School.Domain.Administration.CertificateIssuanceLog, global::School_DTOs.Administration.CertificateIssuanceLogDto>().ReverseMap();
            CreateMap<global::School.Domain.Administration.CertificateIssuanceLog, global::School_DTOs.Administration.CreateCertificateIssuanceDto>().ReverseMap();
            CreateMap<global::School.Domain.Communication.NotificationLog, global::School_DTOs.Communication.NotificationLogDto>().ReverseMap();
            CreateMap<global::School.Domain.Communication.NotificationLog, global::School_DTOs.Communication.CreateNotificationDto>().ReverseMap();
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
