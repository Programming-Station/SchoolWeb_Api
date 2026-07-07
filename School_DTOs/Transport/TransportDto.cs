namespace School_DTOs.Transport
{
    public class VehicleDto{public int Id{get;set;} public string Name{get;set;}=null!; public string? RegistrationNumber{get;set;} public string? DriverName{get;set;} public string? DriverPhone{get;set;} public int Capacity{get;set;} public string Status{get;set;}="active";}
    public class CreateVehicleDto{public string Name{get;set;}=null!; public string? RegistrationNumber{get;set;} public string? DriverName{get;set;} public string? DriverPhone{get;set;} public int Capacity{get;set;} public string Status{get;set;}="active";}
    public class UpdateVehicleDto:CreateVehicleDto{public int Id{get;set;}}
    public class TransportRouteDto{public int Id{get;set;} public string RouteName{get;set;}=null!; public string? Description{get;set;} public int VehicleId{get;set;} public string Status{get;set;}="active";}
    public class CreateTransportRouteDto{public string RouteName{get;set;}=null!; public string? Description{get;set;} public int VehicleId{get;set;} public string Status{get;set;}="active";}
    public class UpdateTransportRouteDto:CreateTransportRouteDto{public int Id{get;set;}}
}
