using FluentValidation;
using School_DTOs.Transport;

namespace School_API.Validators.Transport
{
    public class CreateVehicleDtoValidator : AbstractValidator<CreateVehicleDto>
    {
        public CreateVehicleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vehicle name is required.")
                .MaximumLength(200).WithMessage("Vehicle name cannot exceed 200 characters.");

            RuleFor(x => x.RegistrationNumber)
                .NotEmpty().WithMessage("Registration number is required.")
                .MaximumLength(20).WithMessage("Registration number cannot exceed 20 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than zero.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1990, DateTime.Now.Year + 1).WithMessage("Invalid manufacturing year.");
        }
    }

    public class CreateTransportRouteDtoValidator : AbstractValidator<CreateTransportRouteDto>
    {
        public CreateTransportRouteDtoValidator()
        {
            RuleFor(x => x.RouteName)
                .NotEmpty().WithMessage("Route name is required.")
                .MaximumLength(200).WithMessage("Route name cannot exceed 200 characters.");

            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Source location is required.");

            RuleFor(x => x.Destination)
                .NotEmpty().WithMessage("Destination is required.");

            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle assignment is required.");

            RuleFor(x => x.DistanceKm)
                .GreaterThan(0).WithMessage("Distance must be greater than zero.");

            RuleFor(x => x.MaximumCapacity)
                .GreaterThan(0).WithMessage("Maximum capacity must be greater than zero.");
        }
    }

    public class CreateTransportAllocationDtoValidator : AbstractValidator<CreateTransportAllocationDto>
    {
        public CreateTransportAllocationDtoValidator()
        {
            RuleFor(x => x.TransportRouteId)
                .GreaterThan(0).WithMessage("Transport route is required.");

            RuleFor(x => x.MonthlyCharge)
                .GreaterThanOrEqualTo(0).WithMessage("Monthly charge cannot be negative.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("Academic year is required.");

            RuleFor(x => x)
                .Must(x => x.StudentId.HasValue || x.EmployeeId.HasValue)
                .WithMessage("Either student or employee must be specified.");
        }
    }

    public class CreateTransportTripDtoValidator : AbstractValidator<CreateTransportTripDto>
    {
        public CreateTransportTripDtoValidator()
        {
            RuleFor(x => x.RouteId)
                .GreaterThan(0).WithMessage("Route is required.");

            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle is required.");

            RuleFor(x => x.DriverId)
                .GreaterThan(0).WithMessage("Driver is required.");

            RuleFor(x => x.TripDate)
                .NotEmpty().WithMessage("Trip date is required.");

            RuleFor(x => x.ScheduledStart)
                .NotEmpty().WithMessage("Scheduled start time is required.");
        }
    }

    public class CreateFuelLogDtoValidator : AbstractValidator<CreateFuelLogDto>
    {
        public CreateFuelLogDtoValidator()
        {
            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle is required.");

            RuleFor(x => x.FuelQuantity)
                .GreaterThan(0).WithMessage("Fuel quantity must be greater than zero.");

            RuleFor(x => x.CostPerUnit)
                .GreaterThan(0).WithMessage("Cost per unit must be greater than zero.");

            RuleFor(x => x.TotalCost)
                .GreaterThan(0).WithMessage("Total cost must be greater than zero.");

            RuleFor(x => x.OdometerReading)
                .GreaterThan(0).WithMessage("Odometer reading must be greater than zero.");
        }
    }

    public class CreateVehicleMaintenanceDtoValidator : AbstractValidator<CreateVehicleMaintenanceDto>
    {
        public CreateVehicleMaintenanceDtoValidator()
        {
            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle is required.");

            RuleFor(x => x.MaintenanceType)
                .NotEmpty().WithMessage("Maintenance type is required.");

            RuleFor(x => x.ServiceDate)
                .NotEmpty().WithMessage("Service date is required.");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.");
        }
    }
}
