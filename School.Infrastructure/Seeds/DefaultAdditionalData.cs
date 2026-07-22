using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Domain.Hr.Attendance;
using School.Domain.Hr.LeaveManagement;
using School.Domain.Payroll;
using School.Domain.Transport;

namespace School.Infrastructure.Seeds
{
    public static class DefaultAdditionalData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DPSVAR001")
                         ?? await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001")
                         ?? await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;
            int schoolId = school.Id;

            // 1. Seed Leave Types
            if (!await context.LeaveTypes.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var leaveTypes = new List<LeaveType>
                {
                    new() { Name = "Casual Leave", Code = "CL", Description = "Short-term leave for personal urgent matters (Max 12 days per year)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Sick Leave", Code = "SL", Description = "Medical leave for self or immediate family health recovery (Max 10 days per year)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Earned Leave", Code = "EL", Description = "Privileged annual leave accrued based on working days (Max 15 days per year)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Maternity Leave", Code = "ML", Description = "Paid parental leave for female employees post-delivery (Max 180 days per occurrence)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Paternity Leave", Code = "PL", Description = "Paid parental leave for male employees post-delivery (Max 15 days per occurrence)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "On Duty Leave", Code = "OD", Description = "Official out-of-campus visits, educational seminars, or sports training", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Leave Without Pay", Code = "LWP", Description = "Unpaid leave taken when other leave balances are exhausted", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.LeaveTypes.AddRangeAsync(leaveTypes);
                await context.SaveChangesAsync();
            }

            // 2. Seed Leave Settings
            if (!await context.LeaveSettings.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var leaveSettings = new List<LeaveSetting>
                {
                    new() { Name = "Academic Faculty Policy (2026)", Code = "POL-ACAD", Description = "Applicable to Teachers, Lecturers, HODs, and Co-ordinators", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Administrative Staff Policy (2026)", Code = "POL-ADMIN", Description = "Applicable to Office Clerks, Accountants, HR Managers, and Receptionists", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Support Staff Policy (2026)", Code = "POL-SUPP", Description = "Applicable to Drivers, Helpers, Security personnel, and Peons", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.LeaveSettings.AddRangeAsync(leaveSettings);
                await context.SaveChangesAsync();
            }

            // 3. Seed Shift Masters
            if (!await context.ShiftMasters.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var shifts = new List<ShiftMaster>
                {
                    new() { Name = "Morning Shift (Academic)", Code = "SHF-ACAD", Description = "07:30 AM to 02:30 PM (Core teaching hours)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "General Office Shift", Code = "SHF-GEN", Description = "09:00 AM to 05:00 PM (Administrative & Accounts staff)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Day Security Shift", Code = "SHF-SECD", Description = "08:00 AM to 08:00 PM (Security Guard Day)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Night Security Shift", Code = "SHF-SECN", Description = "08:00 PM to 08:00 AM (Security Guard Night)", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.ShiftMasters.AddRangeAsync(shifts);
                await context.SaveChangesAsync();
            }

            // 4. Seed Week Offs
            if (!await context.WeekOffs.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var weekOffs = new List<WeekOff>
                {
                    new() { Name = "Sunday Weekly Off", Code = "WO-SUN", Description = "Standard Sunday weekly day-off for all staff", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Second & Fourth Saturdays Off", Code = "WO-SAT24", Description = "Academic staff holiday on alternate Saturdays", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Rotation Off (Security)", Code = "WO-ROT", Description = "Rotational weekly off for security guards as per roster", Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.WeekOffs.AddRangeAsync(weekOffs);
                await context.SaveChangesAsync();
            }

            // 5. Seed Salary Components
            if (!await context.SalaryComponents.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var salaryComponents = new List<SalaryComponent>
                {
                    new() { Name = "Basic Pay", Type = "Earning", Amount = 25000.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Dearness Allowance (DA)", Type = "Earning", Amount = 7500.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "House Rent Allowance (HRA)", Type = "Earning", Amount = 10000.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Conveyance Allowance", Type = "Earning", Amount = 2000.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Medical Allowance", Type = "Earning", Amount = 1250.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Special Allowance", Type = "Earning", Amount = 5000.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Employees Provident Fund (EPF)", Type = "Deduction", Amount = 1800.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Professional Tax (PT)", Type = "Deduction", Amount = 200.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Employee State Insurance (ESI)", Type = "Deduction", Amount = 350.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Income Tax (TDS)", Type = "Deduction", Amount = 3000.00m, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.SalaryComponents.AddRangeAsync(salaryComponents);
                await context.SaveChangesAsync();
            }

            // 6. Seed Vehicles
            if (!await context.Vehicles.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var vehicles = new List<Vehicle>
                {
                    new() { Name = "Tata LPO 1618 (40-Seater)", RegistrationNumber = "MH-12-FG-2024", DriverName = "Rajesh Sharma", DriverPhone = "9823456789", Capacity = 40, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Eicher Starline (32-Seater)", RegistrationNumber = "MH-12-HJ-5678", DriverName = "Sardar Baldev Singh", DriverPhone = "9988776655", Capacity = 32, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Force Traveller (17-Seater)", RegistrationNumber = "MH-12-KL-9012", DriverName = "Vilas Shinde", DriverPhone = "9422001122", Capacity = 17, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Name = "Mahindra Bolero Pickup (Utility)", RegistrationNumber = "MH-12-ZX-3456", DriverName = "Amit Patil", DriverPhone = "9850123456", Capacity = 5, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();
            }

            // 7. Seed Transport Routes
            if (!await context.TransportRoutes.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var bus1 = await context.Vehicles.FirstOrDefaultAsync(v => v.RegistrationNumber == "MH-12-FG-2024");
                var bus2 = await context.Vehicles.FirstOrDefaultAsync(v => v.RegistrationNumber == "MH-12-HJ-5678");
                var van = await context.Vehicles.FirstOrDefaultAsync(v => v.RegistrationNumber == "MH-12-KL-9012");

                if (bus1 != null && bus2 != null && van != null)
                {
                    var routes = new List<TransportRoute>
                    {
                        new() { RouteName = "Route 101 - Kothrud Depot to School via Karve Road", Description = "Stops: Kothrud, Karve Putla, Nal Stop, Erandwane, Deccan", VehicleId = bus1.Id, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                        new() { RouteName = "Route 102 - Hadapsar Gadar to School via Camp", Description = "Stops: Hadapsar, Magarpatta, Fatimanagar, Pulgate, MG Road", VehicleId = bus2.Id, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                        new() { RouteName = "Route 201 - Wakad Flyover to School via Baner Road", Description = "Stops: Wakad, Dange Chowk, Balewadi, Baner, University Circle", VehicleId = van.Id, Status = "active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                    };
                    await context.TransportRoutes.AddRangeAsync(routes);
                    await context.SaveChangesAsync();
                }
            }

            // 8. Seed Payment Gateways
            if (!await context.PaymentGateways.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var gateways = new List<PaymentGateway>
                {
                    new() { GatewayName = "Razorpay Test Gateway", ApiKey = "rzp_test_U87dKfjvL91oEa", SecretKey = "secret_W912kaJhs82109lkaD", WebhookSecret = "wh_rzp_sec_382193892", IsActive = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GatewayName = "Stripe Sandbox", ApiKey = "pk_test_51NfG84Jsj8w19sDkfjh", SecretKey = "sk_test_51NfG84Jsj8w19sDkfjh7382J", WebhookSecret = "whsec_stripe_sandbox_11827", IsActive = false, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GatewayName = "Paytm Business Gateway", ApiKey = "paytm_test_mid_991823901", SecretKey = "paytm_merchant_secret_772", WebhookSecret = "wh_paytm_sec_881", IsActive = false, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.PaymentGateways.AddRangeAsync(gateways);
                await context.SaveChangesAsync();
            }
        }
    }
}
