using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.Auth;
using School.Domain.Hr;
using School.Domain.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultHrData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
            if (school == null) return;
            int schoolId = school.Id;

            // 1. Seed Faculties
            var defaultFaculties = new List<Faculty>
            {
                new Faculty { Name = "Academics Faculty", Code = "ACAD", Description = "Core Academics and Curriculum development", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Administrative Faculty", Code = "ADMIN", Description = "School administration, registry and management", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Science Faculty", Code = "SCI", Description = "Physics, Chemistry, Biology and general science studies", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Commerce Faculty", Code = "COMM", Description = "Accountancy, Business Studies, Economics", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Arts & Humanities Faculty", Code = "ARTS", Description = "Languages, History, Geography, Fine Arts", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Physical Education & Sports Faculty", Code = "SPORTS", Description = "Sports coaching, PE, and wellness activities", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Information Technology Faculty", Code = "IT", Description = "Computer studies, systems administration and IT support", SchoolRegistrationId = schoolId, Status = "active" },
                new Faculty { Name = "Support Staff Faculty", Code = "SUPPORT", Description = "Housekeeping, security, transport and maintenance", SchoolRegistrationId = schoolId, Status = "active" }
            };

            foreach (var fac in defaultFaculties)
            {
                if (!await context.Faculties.AnyAsync(f => f.Code == fac.Code))
                {
                    context.Faculties.Add(fac);
                }
            }
            await context.SaveChangesAsync();

            var acadFaculty = await context.Faculties.FirstAsync(f => f.Code == "ACAD");
            var adminFaculty = await context.Faculties.FirstAsync(f => f.Code == "ADMIN");
            var sciFaculty = await context.Faculties.FirstAsync(f => f.Code == "SCI");
            var commFaculty = await context.Faculties.FirstAsync(f => f.Code == "COMM");
            var artsFaculty = await context.Faculties.FirstAsync(f => f.Code == "ARTS");
            var sportsFaculty = await context.Faculties.FirstAsync(f => f.Code == "SPORTS");
            var itFaculty = await context.Faculties.FirstAsync(f => f.Code == "IT");
            var supportFaculty = await context.Faculties.FirstAsync(f => f.Code == "SUPPORT");

            // 2. Seed Departments
            var defaultDepartments = new List<Department>
            {
                new Department { Name = "Mathematics Department", Code = "MATH", FacultyId = acadFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Mathematics and Statistics" },
                new Department { Name = "Languages Department", Code = "LANG", FacultyId = acadFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "English, Hindi, and regional languages" },
                new Department { Name = "Physics Department", Code = "PHY", FacultyId = sciFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Physics and Astronomy" },
                new Department { Name = "Chemistry Department", Code = "CHEM", FacultyId = sciFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Organic, Inorganic and Physical Chemistry" },
                new Department { Name = "Biology Department", Code = "BIO", FacultyId = sciFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Botany, Zoology and Biotechnology" },
                new Department { Name = "Accountancy & Commerce Department", Code = "ACC", FacultyId = commFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Accounts, Business Studies and Economics" },
                new Department { Name = "History & Social Sciences Department", Code = "HIST", FacultyId = artsFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "History, Civics, Geography and Sociology" },
                new Department { Name = "Fine Arts & Music Department", Code = "ART", FacultyId = artsFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Painting, Sculpting, Music and Performing Arts" },
                new Department { Name = "Physical Education Department", Code = "PE", FacultyId = sportsFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Sports coaching, fitness and training" },
                new Department { Name = "Computer Science Department", Code = "COMP", FacultyId = itFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Coding, computer applications and information technology" },
                new Department { Name = "General Administration Department", Code = "ADM", FacultyId = adminFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "School administration, registry and operations" },
                new Department { Name = "Finance & Accounts Department", Code = "FIN", FacultyId = adminFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Payroll, fees collection and school accounting" },
                new Department { Name = "Security & Maintenance Department", Code = "SEC", FacultyId = supportFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "Campus safety and facilities maintenance" },
                new Department { Name = "Transport & Logistics Department", Code = "TRANS", FacultyId = supportFaculty.Id, SchoolRegistrationId = schoolId, Status = "active", Description = "School buses, route planning and logistics" }
            };

            foreach (var dept in defaultDepartments)
            {
                if (!await context.Departments.AnyAsync(d => d.Code == dept.Code))
                {
                    context.Departments.Add(dept);
                }
            }
            await context.SaveChangesAsync();

            // 3. Seed Blood Groups
            var bloodGroups = new[] { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };
            foreach (var bg in bloodGroups)
            {
                if (!await context.BloodGroupMasters.AnyAsync(b => b.Name == bg))
                {
                    context.BloodGroupMasters.Add(new BloodGroupMaster
                    {
                        Name = bg,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 4. Seed Designations
            var defaultDesignations = new[]
            {
                "Principal", "Vice Principal", "Academic Coordinator", "Head of Department (HOD)",
                "Teacher", "Senior Teacher", "Primary Teacher (PRT)", "Trained Graduate Teacher (TGT)",
                "Post Graduate Teacher (PGT)", "Pre-Primary Teacher", "Special Educator",
                "Counselor", "Librarian", "Assistant Librarian", "Accountant",
                "Administrative Officer", "Registrar", "Office Assistant", "Clerk",
                "Lab Assistant", "IT Administrator", "System Administrator", "Receptionist",
                "Sports Coach", "Physical Education Instructor", "Nurse", "Chief Security Officer",
                "Security Guard", "Maintenance Supervisor", "Driver", "Peon"
            };

            foreach (var d in defaultDesignations)
            {
                if (!await context.Designations.AnyAsync(dg => dg.Name == d))
                {
                    context.Designations.Add(new Designation
                    {
                        Name = d,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 5. Seed Employee Categories
            var categories = new[]
            {
                "Teaching Staff", "Non-Teaching Staff", "Administrative Staff", "Support Staff",
                "Management Staff", "Technical Staff", "Counseling & Health Staff", "Executive Staff",
                "Library Staff", "Sports & Coaching Staff", "Security Staff", "Transport Staff",
                "Housekeeping Staff"
            };
            foreach (var c in categories)
            {
                if (!await context.EmployeeCategories.AnyAsync(ec => ec.Name == c))
                {
                    context.EmployeeCategories.Add(new EmployeeCategory
                    {
                        Name = c,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 6. Seed Employee Types
            var types = new[] { "Permanent", "Contractual", "Temporary", "Part-Time", "Probationary", "Intern / Trainee", "Guest Faculty", "Volunteer" };
            foreach (var t in types)
            {
                if (!await context.EmployeeTypes.AnyAsync(et => et.Name == t))
                {
                    context.EmployeeTypes.Add(new EmployeeType
                    {
                        Name = t,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 7. Seed Employment Statuses
            var statuses = new[] { "Active / Confirmed", "Probationary", "On Leave", "Suspended", "Resigned", "Terminated", "Retired", "Absconding" };
            foreach (var s in statuses)
            {
                if (!await context.EmploymentStatuses.AnyAsync(es => es.Name == s))
                {
                    context.EmploymentStatuses.Add(new EmploymentStatus
                    {
                        Name = s,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 8. Seed Notice Periods
            var noticePeriods = new[] { "No Notice Period", "15 Days", "1 Month", "2 Months", "3 Months", "6 Months", "Immediate" };
            foreach (var np in noticePeriods)
            {
                if (!await context.NoticePeriods.AnyAsync(n => n.Name == np))
                {
                    context.NoticePeriods.Add(new NoticePeriod
                    {
                        Name = np,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 9. Seed Qualifications
            var qualifications = new[]
            {
                "High School Diploma", "Associate Degree", "Bachelor of Arts (B.A.)", "Bachelor of Science (B.Sc.)",
                "Bachelor of Commerce (B.Com.)", "Bachelor of Education (B.Ed.)", "Bachelor of Technology (B.Tech.)",
                "Master of Arts (M.A.)", "Master of Science (M.Sc.)", "Master of Commerce (M.Com.)",
                "Master of Education (M.Ed.)", "Master of Technology (M.Tech.)", "Master of Business Administration (MBA)",
                "Doctor of Philosophy (Ph.D.)", "M.Phil.", "Diploma in Elementary Education (D.El.Ed.)"
            };
            foreach (var q in qualifications)
            {
                if (!await context.QualificationMasters.AnyAsync(qm => qm.Name == q))
                {
                    context.QualificationMasters.Add(new QualificationMaster
                    {
                        Name = q,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 10. Seed Religions
            var religions = new[] { "Hinduism", "Islam", "Christianity", "Sikhism", "Buddhism", "Jainism", "Judaism", "Zoroastrianism (Parsi)", "Others" };
            foreach (var r in religions)
            {
                if (!await context.ReligionMasters.AnyAsync(rm => rm.Name == r))
                {
                    context.ReligionMasters.Add(new ReligionMaster
                    {
                        Name = r,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 11. Seed Salary Grades
            var salaryGrades = new[]
            {
                "Grade A-1 (Executive)", "Grade A-2 (Senior Management)", "Grade B-1 (Academic Head)",
                "Grade B-2 (Senior Teacher)", "Grade C-1 (TGT / Primary Teacher)", "Grade C-2 (Assistant Teacher)",
                "Grade D-1 (Administrative Officer)", "Grade D-2 (Clerical / Support)", "Consolidated Salary"
            };
            foreach (var sg in salaryGrades)
            {
                if (!await context.SalaryGrades.AnyAsync(s => s.Name == sg))
                {
                    context.SalaryGrades.Add(new SalaryGrade
                    {
                        Name = sg,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 12. Seed Specializations
            var specializations = new[]
            {
                "Mathematics", "Physics", "Chemistry", "Biology (Botany/Zoology)", "Computer Science / IT",
                "English Literature & Grammar", "Hindi Literature", "Sanskrit", "Social Sciences (History & Civics)",
                "Geography & Environmental Science", "Economics & Business Studies", "Accountancy & Financial Management",
                "Fine Arts & Painting", "Music (Vocal & Instrumental)", "Physical Education & Sports Training",
                "Special Education & Counseling", "Kindergarten / Early Childhood Education"
            };
            foreach (var s in specializations)
            {
                if (!await context.Specializations.AnyAsync(sp => sp.Name == s))
                {
                    context.Specializations.Add(new Specialization
                    {
                        Name = s,
                        Status = "active",
                        SchoolRegistrationId = schoolId
                    });
                }
            }
            await context.SaveChangesAsync();

            // Ensure Employee role exists
            var employeeRole = await context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == "EMPLOYEE");
            if (employeeRole == null)
            {
                employeeRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                };
                context.Roles.Add(employeeRole);
                await context.SaveChangesAsync();
            }

            // 13. Seed Employees if count is less than 5
            if (await context.Employees.CountAsync() < 5)
            {
                // Clear existing to avoid duplicate conflicts
                var existing = await context.Employees.ToListAsync();
                context.Employees.RemoveRange(existing);
                await context.SaveChangesAsync();

                var mathDept = await context.Departments.FirstOrDefaultAsync(d => d.Code == "MATH");
                var phyDept = await context.Departments.FirstOrDefaultAsync(d => d.Code == "PHY");
                var admDept = await context.Departments.FirstOrDefaultAsync(d => d.Code == "ADM");
                var langDept = await context.Departments.FirstOrDefaultAsync(d => d.Code == "LANG");
                var finDept = await context.Departments.FirstOrDefaultAsync(d => d.Code == "FIN");

                var teacherDesig = await context.Designations.FirstOrDefaultAsync(d => d.Name == "Teacher");
                var principalDesig = await context.Designations.FirstOrDefaultAsync(d => d.Name == "Principal");
                var librarianDesig = await context.Designations.FirstOrDefaultAsync(d => d.Name == "Librarian");
                var accountantDesig = await context.Designations.FirstOrDefaultAsync(d => d.Name == "Accountant");

                var employeesToSeed = new List<(string Code, string First, string Last, string Gender, DateTime Dob, string Email, string Mobile, int DeptId, int? DesigId, decimal Exp)>
                {
                    ("EMP20260001", "Jane", "Smith", "Female", new DateTime(1992, 4, 18), "janesmith@defaultschool.com", "9876543210", mathDept?.Id ?? 1, teacherDesig?.Id, 4),
                    ("EMP20260002", "John", "Doe", "Male", new DateTime(1990, 8, 12), "johndoe@defaultschool.com", "9876543211", phyDept?.Id ?? 1, teacherDesig?.Id, 6),
                    ("EMP20260003", "Robert", "Johnson", "Male", new DateTime(1980, 11, 24), "robertj@defaultschool.com", "9876543212", admDept?.Id ?? 1, principalDesig?.Id, 15),
                    ("EMP20260004", "Emily", "Davis", "Female", new DateTime(1995, 3, 5), "emilyd@defaultschool.com", "9876543213", langDept?.Id ?? 1, librarianDesig?.Id, 3),
                    ("EMP20260005", "Michael", "Wilson", "Male", new DateTime(1988, 12, 30), "michaelw@defaultschool.com", "9876543214", finDept?.Id ?? 1, accountantDesig?.Id, 8)
                };

                foreach (var empData in employeesToSeed)
                {
                    var employeeUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == empData.Code);
                    if (employeeUser == null)
                    {
                        employeeUser = new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = empData.Code,
                            NormalizedUserName = empData.Code.ToUpper(),
                            Email = empData.Email,
                            NormalizedEmail = empData.Email.ToUpper(),
                            FirstName = empData.First,
                            LastName = empData.Last,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", // Admin@123
                            StatusId = 1,
                            IsDefaultPassword = false,
                            IsActive = true,
                            SchoolRegistrationId = schoolId
                        };
                        context.Users.Add(employeeUser);
                        await context.SaveChangesAsync();

                        // Add to user roles
                        context.UserRoles.Add(new IdentityUserRole<string>
                        {
                            UserId = employeeUser.Id,
                            RoleId = employeeRole.Id
                        });
                        await context.SaveChangesAsync();
                    }

                    var employee = new Employee
                    {
                        EmployeeCode = empData.Code,
                        FirstName = empData.First,
                        LastName = empData.Last,
                        Gender = empData.Gender,
                        DateOfBirth = empData.Dob,
                        Email = empData.Email,
                        MobileNumber = empData.Mobile,
                        Status = "active",
                        JoiningDate = DateTime.UtcNow.AddYears(-(int)empData.Exp).AddMonths(3),
                        DepartmentId = empData.DeptId,
                        DesignationId = empData.DesigId,
                        SchoolRegistrationId = schoolId,
                        Experience = empData.Exp,
                        ApplicationUserId = employeeUser.Id
                    };
                    context.Employees.Add(employee);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
