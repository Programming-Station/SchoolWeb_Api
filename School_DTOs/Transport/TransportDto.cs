using System;

namespace School_DTOs.Transport
{
    #nullable disable

    // ══════════════════════════════════════════════════════════════════════════
    // VEHICLE DTOS
    // ══════════════════════════════════════════════════════════════════════════
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }

    public class CreateVehicleDto
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public int Capacity { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ROUTE DTOS
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportRouteDto
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string Description { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public int Capacity { get; set; }
        public int OccupiedSeats { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportRouteDto
    {
        public string RouteName { get; set; }
        public string Description { get; set; }
        public int VehicleId { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // ALLOCATION DTOS
    // ══════════════════════════════════════════════════════════════════════════
    public class TransportAllocationDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string AdmissionNumber { get; set; }
        public string ClassName { get; set; }
        public int TransportRouteId { get; set; }
        public string RouteName { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal MonthlyCharge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }

    public class CreateTransportAllocationDto
    {
        public int StudentId { get; set; }
        public int TransportRouteId { get; set; }
        public decimal MonthlyCharge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
