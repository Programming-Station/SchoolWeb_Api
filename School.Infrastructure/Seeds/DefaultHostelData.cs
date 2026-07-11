using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Hostel;
using School.Domain.Hr;
using School.Domain.Student;

namespace School.Infrastructure.Seeds
{
    public static class DefaultHostelData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DPSVAR001")
                         ?? await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001")
                         ?? await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;
            int schoolId = school.Id;

            // 1. Seed Hostels
            if (await context.Hostels.AnyAsync(h => h.SchoolRegistrationId == schoolId)) return;

            var hostels = new List<Hostel>
            {
                new()
                {
                    Name = "Grand Boys Residency",
                    Code = "GBR-01",
                    HostelType = "Boys",
                    Capacity = 150,
                    Address = "North Campus Sector-4",
                    Description = "Premium boys engineering residency building",
                    Status = "active",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    Name = "Elite Girls Hostel",
                    Code = "EGH-01",
                    HostelType = "Girls",
                    Capacity = 150,
                    Address = "South Campus Sector-5",
                    Description = "Sleek girls residency building with automated security",
                    Status = "active",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.Hostels.AddRangeAsync(hostels);
            await context.SaveChangesAsync();

            var boysHostel = hostels[0];
            var girlsHostel = hostels[1];

            // 2. Seed Buildings
            var buildings = new List<Building>
            {
                new()
                {
                    HostelId = boysHostel.Id,
                    Name = "Aryabhatta Block",
                    Code = "ABB-01",
                    NumberOfFloors = 4,
                    ConstructionYear = 2021,
                    Status = "active",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    HostelId = girlsHostel.Id,
                    Name = "Gargi Block",
                    Code = "GGB-01",
                    NumberOfFloors = 4,
                    ConstructionYear = 2022,
                    Status = "active",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.Buildings.AddRangeAsync(buildings);
            await context.SaveChangesAsync();

            var boysBuilding = buildings[0];
            var girlsBuilding = buildings[1];

            // 3. Seed Floors
            var floors = new List<Floor>
            {
                new()
                {
                    BuildingId = boysBuilding.Id,
                    FloorNumber = 1,
                    Description = "First Floor",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    BuildingId = boysBuilding.Id,
                    FloorNumber = 2,
                    Description = "Second Floor",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    BuildingId = girlsBuilding.Id,
                    FloorNumber = 1,
                    Description = "First Floor",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    BuildingId = girlsBuilding.Id,
                    FloorNumber = 2,
                    Description = "Second Floor",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.Floors.AddRangeAsync(floors);
            await context.SaveChangesAsync();

            var boysFloor1 = floors[0];
            var boysFloor2 = floors[1];
            var girlsFloor1 = floors[2];
            var girlsFloor2 = floors[3];

            // 4. Seed Room Categories
            var categories = new List<RoomCategory>
            {
                new()
                {
                    Name = "Single AC Suite",
                    IsAc = true,
                    HasAttachedBathroom = true,
                    HasWifi = true,
                    DefaultFee = 12000,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    Name = "Double Sharing AC",
                    IsAc = true,
                    HasAttachedBathroom = true,
                    HasWifi = true,
                    DefaultFee = 8000,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    Name = "Triple Sharing Non-AC",
                    IsAc = false,
                    HasAttachedBathroom = true,
                    HasWifi = false,
                    DefaultFee = 5000,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.RoomCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var singleAC = categories[0];
            var doubleAC = categories[1];
            var tripleNonAC = categories[2];

            // 5. Seed Rooms
            var rooms = new List<Room>
            {
                new()
                {
                    HostelId = boysHostel.Id,
                    BuildingId = boysBuilding.Id,
                    FloorId = boysFloor1.Id,
                    RoomCategoryId = singleAC.Id,
                    RoomNumber = "B-101",
                    Capacity = 1,
                    Status = "Available",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    HostelId = boysHostel.Id,
                    BuildingId = boysBuilding.Id,
                    FloorId = boysFloor1.Id,
                    RoomCategoryId = doubleAC.Id,
                    RoomNumber = "B-102",
                    Capacity = 2,
                    Status = "Available",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    HostelId = girlsHostel.Id,
                    BuildingId = girlsBuilding.Id,
                    FloorId = girlsFloor1.Id,
                    RoomCategoryId = doubleAC.Id,
                    RoomNumber = "G-101",
                    Capacity = 2,
                    Status = "Available",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    HostelId = girlsHostel.Id,
                    BuildingId = girlsBuilding.Id,
                    FloorId = girlsFloor1.Id,
                    RoomCategoryId = tripleNonAC.Id,
                    RoomNumber = "G-102",
                    Capacity = 3,
                    Status = "Available",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.Rooms.AddRangeAsync(rooms);
            await context.SaveChangesAsync();

            // 6. Seed Beds
            var beds = new List<Bed>();
            foreach (var room in rooms)
            {
                for (int i = 1; i <= room.Capacity; i++)
                {
                    beds.Add(new Bed
                    {
                        RoomId = room.Id,
                        BedNumber = $"{room.RoomNumber}-Bed{i}",
                        Status = "Available",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = "seed",
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
            await context.Beds.AddRangeAsync(beds);
            await context.SaveChangesAsync();

            // 7. Seed Mess Menu (Monday to Sunday)
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var mealTypes = new[] { "Breakfast", "Lunch", "Snacks", "Dinner" };
            var menuList = new List<MessMenu>();
            foreach (var h in hostels)
            {
                foreach (var day in days)
                {
                    foreach (var meal in mealTypes)
                    {
                        string itemsText = meal switch
                        {
                            "Breakfast" => "Healthy Multigrain Parathas, Butter, Curd & Sprouts Salad",
                            "Lunch" => "Rich Shahi Paneer, Dal Makhani, Basmati Rice, Butter Roti, Green Salad",
                            "Snacks" => "Hot Vegetable Samosas, Mint Chutney & Cardamom Tea",
                            "Dinner" => "Healthy Seasonal Veg Sabzi, Dal Fry, Jeera Rice, Tandoori Roti & Sweet Kheer",
                            _ => "Standard Meal"
                        };

                        menuList.Add(new MessMenu
                        {
                            HostelId = h.Id,
                            DayOfWeek = day,
                            MealType = meal,
                            FoodItems = itemsText,
                            SpecialItems = "None",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "seed",
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }
            }
            await context.MessMenus.AddRangeAsync(menuList);
            await context.SaveChangesAsync();

            // 8. Associate Wardens if employees exist
            var emp = await context.Employees.FirstOrDefaultAsync(e => e.SchoolRegistrationId == schoolId && !e.IsDeleted);
            if (emp != null)
            {
                var warden = new HostelWarden
                {
                    HostelId = boysHostel.Id,
                    EmployeeId = emp.Id,
                    RoleType = "ChiefWarden",
                    Status = "Active",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.UtcNow
                };
                await context.HostelWardens.AddAsync(warden);
                await context.SaveChangesAsync();
            }

            // 9. Associate Admissions if students exist
            var maleStudent = await context.Students.FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId && s.Gender.ToLower().StartsWith("m") && !s.IsDeleted);
            var femaleStudent = await context.Students.FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId && s.Gender.ToLower().StartsWith("f") && !s.IsDeleted);
            var academicYear = await context.AcademicYears.FirstOrDefaultAsync(y => y.SchoolRegistrationId == schoolId && y.IsActive);

            if (academicYear != null)
            {
                // Male student admission
                if (maleStudent != null)
                {
                    var bed = await context.Beds.Include(b => b.Room).FirstOrDefaultAsync(b => b.Room.HostelId == boysHostel.Id && b.Status == "Available");
                    if (bed != null)
                    {
                        var adm = new HostelAdmission
                        {
                            StudentId = maleStudent.Id,
                            HostelId = boysHostel.Id,
                            RoomId = bed.RoomId,
                            BedId = bed.Id,
                            AcademicYearId = academicYear.Id,
                            AdmissionNumber = "HADM-2026-0001",
                            AdmissionDate = DateTime.UtcNow.AddMonths(-2),
                            Status = "checkedin",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "seed",
                            CreatedDate = DateTime.UtcNow
                        };
                        await context.HostelAdmissions.AddAsync(adm);

                        bed.Status = "Occupied";
                        context.Entry(bed).State = EntityState.Modified;

                        var room = bed.Room;
                        room.Status = "Available"; // still has vacancy
                        context.Entry(room).State = EntityState.Modified;

                        // Create some mock activity: Attendance, Complaint
                        await context.HostelAttendances.AddAsync(new HostelAttendance
                        {
                            StudentId = maleStudent.Id,
                            Date = DateTime.UtcNow.AddDays(-1),
                            Status = "Present",
                            Remarks = "Regular checkin at night roll-call",
                            AttendanceType = "NightRollCall",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "seed",
                            CreatedDate = DateTime.UtcNow
                        });

                        await context.HostelComplaints.AddAsync(new HostelComplaint
                        {
                            StudentId = maleStudent.Id,
                            Category = "Electrical",
                            Description = "Study desk lamp holder not working properly, sparks when turned on.",
                            Priority = "High",
                            Status = "Open",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "seed",
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }

                // Female student admission
                if (femaleStudent != null)
                {
                    var bed = await context.Beds.Include(b => b.Room).FirstOrDefaultAsync(b => b.Room.HostelId == girlsHostel.Id && b.Status == "Available");
                    if (bed != null)
                    {
                        var adm = new HostelAdmission
                        {
                            StudentId = femaleStudent.Id,
                            HostelId = girlsHostel.Id,
                            RoomId = bed.RoomId,
                            BedId = bed.Id,
                            AcademicYearId = academicYear.Id,
                            AdmissionNumber = "HADM-2026-0002",
                            AdmissionDate = DateTime.UtcNow.AddMonths(-1),
                            Status = "checkedin",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "seed",
                            CreatedDate = DateTime.UtcNow
                        };
                        await context.HostelAdmissions.AddAsync(adm);

                        bed.Status = "Occupied";
                        context.Entry(bed).State = EntityState.Modified;

                        var room = bed.Room;
                        room.Status = "Available"; // still has vacancy
                        context.Entry(room).State = EntityState.Modified;
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
