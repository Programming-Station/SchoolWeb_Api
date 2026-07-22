using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain.AccessControl;
using School.Utilities.Constants;

namespace School.Infrastructure.Seeds
{
    public static class DefaultAccessControlData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            // Retrieve default school registration
            var defaultSchool = await context.SchoolRegistrations.FirstOrDefaultAsync(s => s.SchoolCode == "DEF001");
            int defaultSchoolId = defaultSchool?.Id ?? 1;

            // Get Category Modules
            var managementCategory = await context.CategoryModules.FirstOrDefaultAsync(c => c.Name == "Management");
            var adminCategory = await context.CategoryModules.FirstOrDefaultAsync(c => c.Name == "Administrative");
            var academicCategory = await context.CategoryModules.FirstOrDefaultAsync(c => c.Name == "Academic");
            var communicationCategory = await context.CategoryModules.FirstOrDefaultAsync(c => c.Name == "Communication");
            var reportsCategory = await context.CategoryModules.FirstOrDefaultAsync(c => c.Name == "Reports");

            if (managementCategory == null || adminCategory == null) return;

            // 1. Seed Modules
            var defaultModules = new List<Module>
            {
                new Module
                {
                    Name = "Access Control",
                    Route = "access-control",
                    Icon = "pi pi-shield",
                    CategoryModuleId = managementCategory.Id,
                    Order = 1,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Module
                {
                    Name = "HR Setup",
                    Route = "hr-setup",
                    Icon = "pi pi-cog",
                    CategoryModuleId = adminCategory.Id,
                    Order = 2,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Module
                {
                    Name = "Student Management",
                    Route = "students",
                    Icon = "pi pi-users",
                    CategoryModuleId = managementCategory.Id,
                    Order = 3,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                }
            };

            foreach (var module in defaultModules)
            {
                var exists = await context.Modules.AnyAsync(m => m.Route == module.Route && m.SchoolRegistrationId == defaultSchoolId);
                if (!exists)
                {
                    context.Modules.Add(module);
                }
            }
            await context.SaveChangesAsync();

            // 2. Seed Menus
            var defaultMenus = new List<Menu>
            {
                new Menu
                {
                    MenuName = "Dashboard",
                    URL = "/admin/dashboard",
                    Priority = 1,
                    MenuIcon = "pi pi-home",
                    IsVisible = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Menu
                {
                    MenuName = "Students",
                    URL = "/students",
                    Priority = 2,
                    MenuIcon = "pi pi-users",
                    IsVisible = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Menu
                {
                    MenuName = "Academic Setup",
                    URL = "/academic-year",
                    Priority = 3,
                    MenuIcon = "pi pi-book",
                    IsVisible = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Menu
                {
                    MenuName = "Staff & HR",
                    URL = "/hr/dashboard",
                    Priority = 4,
                    MenuIcon = "pi pi-user-plus",
                    IsVisible = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Menu
                {
                    MenuName = "Operations",
                    URL = "/event",
                    Priority = 5,
                    MenuIcon = "pi pi-calendar",
                    IsVisible = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                },
                new Menu
                {
                    MenuName = "Access Control",
                    URL = "/access-control",
                    Priority = 6,
                    MenuIcon = "pi pi-shield",
                    IsVisible = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = defaultSchoolId
                }
            };

            foreach (var menu in defaultMenus)
            {
                var exists = await context.Menus.AnyAsync(m => m.MenuName == menu.MenuName && m.SchoolRegistrationId == defaultSchoolId);
                if (!exists)
                {
                    context.Menus.Add(menu);
                }
            }
            await context.SaveChangesAsync();

            // Load saved menus to map submenus correctly
            var dbMenus = await context.Menus.Where(m => m.SchoolRegistrationId == defaultSchoolId).ToListAsync();

            // 3. Seed SubMenus
            var subMenusToAdd = new List<SubMenu>();

            var studentsMenu = dbMenus.FirstOrDefault(m => m.MenuName == "Students");
            if (studentsMenu != null)
            {
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Student List", URL = "/students", Priority = 1, Icon = "pi pi-list", MenuId = studentsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Student Registration", URL = "/student-registration", Priority = 2, Icon = "pi pi-user-plus", MenuId = studentsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
            }

            var academicMenu = dbMenus.FirstOrDefault(m => m.MenuName == "Academic Setup");
            if (academicMenu != null)
            {
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Academic Years", URL = "/academic-year", Priority = 1, Icon = "pi pi-calendar", MenuId = academicMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Courses", URL = "/course", Priority = 2, Icon = "pi pi-book", MenuId = academicMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Departments", URL = "/department", Priority = 3, Icon = "pi pi-building", MenuId = academicMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
            }

            var hrMenu = dbMenus.FirstOrDefault(m => m.MenuName == "Staff & HR");
            if (hrMenu != null)
            {
                subMenusToAdd.Add(new SubMenu { SubMenuName = "HR Dashboard", URL = "/hr/dashboard", Priority = 1, Icon = "pi pi-chart-bar", MenuId = hrMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Employees", URL = "/hr/employee/list", Priority = 2, Icon = "pi pi-users", MenuId = hrMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Leaves", URL = "/hr/leave/list", Priority = 3, Icon = "pi pi-calendar-times", MenuId = hrMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Attendance", URL = "/hr/attendance", Priority = 4, Icon = "pi pi-check-square", MenuId = hrMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Timesheet", URL = "/hr/timesheet", Priority = 5, Icon = "pi pi-clock", MenuId = hrMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "HR Setup", URL = "/hr/setup", Priority = 6, Icon = "pi pi-cog", MenuId = hrMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
            }

            var operationsMenu = dbMenus.FirstOrDefault(m => m.MenuName == "Operations");
            if (operationsMenu != null)
            {
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Events", URL = "/event", Priority = 1, Icon = "pi pi-calendar-plus", MenuId = operationsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Faculty", URL = "/faculty", Priority = 2, Icon = "pi pi-user", MenuId = operationsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Fees", URL = "/fee", Priority = 3, Icon = "pi pi-money-bill", MenuId = operationsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "States", URL = "/state", Priority = 4, Icon = "pi pi-map", MenuId = operationsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                subMenusToAdd.Add(new SubMenu { SubMenuName = "Masters", URL = "/master", Priority = 5, Icon = "pi pi-sliders-h", MenuId = operationsMenu.Id, SchoolRegistrationId = defaultSchoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
            }

            foreach (var sm in subMenusToAdd)
            {
                var exists = await context.SubMenus.AnyAsync(s => s.SubMenuName == sm.SubMenuName && s.MenuId == sm.MenuId && s.SchoolRegistrationId == defaultSchoolId);
                if (!exists)
                {
                    context.SubMenus.Add(sm);
                }
            }
            await context.SaveChangesAsync();

            // 4. Seed Default MenuPermissions (For Admin and Owner roles)
            var allDbMenus = await context.Menus.Where(m => m.SchoolRegistrationId == defaultSchoolId).ToListAsync();
            var allDbSubMenus = await context.SubMenus.Where(s => s.SchoolRegistrationId == defaultSchoolId).ToListAsync();

            var adminRole = await context.Set<IdentityRole>().FirstOrDefaultAsync(r => r.NormalizedName == "ADMIN");
            var ownerRole = await context.Set<IdentityRole>().FirstOrDefaultAsync(r => r.NormalizedName == "OWNER");

            string adminRoleId = adminRole?.Id ?? Constants.Admin;
            string ownerRoleId = ownerRole?.Id ?? Constants.Owner;

            // Admin Role Permissions
            foreach (var menu in allDbMenus)
            {
                var subMenus = allDbSubMenus.Where(s => s.MenuId == menu.Id).ToList();
                if (subMenus.Any())
                {
                    foreach (var sm in subMenus)
                    {
                        var hasPerm = await context.MenuPermessions.AnyAsync(p => p.MenuId == menu.Id && p.SubMenuId == sm.Id && p.RoleId == adminRoleId && p.SchoolRegistrationId == defaultSchoolId);
                        if (!hasPerm)
                        {
                            context.MenuPermessions.Add(new MenuPermession
                            {
                                MenuId = menu.Id,
                                SubMenuId = sm.Id,
                                RoleId = adminRoleId,
                                IsVisible = true,
                                SchoolRegistrationId = defaultSchoolId,
                                CreatedBy = "System",
                                CreatedDate = DateTime.UtcNow
                            });
                        }
                    }
                }
                else
                {
                    var hasPerm = await context.MenuPermessions.AnyAsync(p => p.MenuId == menu.Id && p.SubMenuId == null && p.RoleId == adminRoleId && p.SchoolRegistrationId == defaultSchoolId);
                    if (!hasPerm)
                    {
                        context.MenuPermessions.Add(new MenuPermession
                        {
                            MenuId = menu.Id,
                            SubMenuId = null,
                            RoleId = adminRoleId,
                            IsVisible = true,
                            SchoolRegistrationId = defaultSchoolId,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }
            }

            // Owner Role Permissions
            foreach (var menu in allDbMenus)
            {
                var subMenus = allDbSubMenus.Where(s => s.MenuId == menu.Id).ToList();
                if (subMenus.Any())
                {
                    foreach (var sm in subMenus)
                    {
                        var hasPerm = await context.MenuPermessions.AnyAsync(p => p.MenuId == menu.Id && p.SubMenuId == sm.Id && p.RoleId == ownerRoleId && p.SchoolRegistrationId == defaultSchoolId);
                        if (!hasPerm)
                        {
                            context.MenuPermessions.Add(new MenuPermession
                            {
                                MenuId = menu.Id,
                                SubMenuId = sm.Id,
                                RoleId = ownerRoleId,
                                IsVisible = true,
                                SchoolRegistrationId = defaultSchoolId,
                                CreatedBy = "System",
                                CreatedDate = DateTime.UtcNow
                            });
                        }
                    }
                }
                else
                {
                    var hasPerm = await context.MenuPermessions.AnyAsync(p => p.MenuId == menu.Id && p.SubMenuId == null && p.RoleId == ownerRoleId && p.SchoolRegistrationId == defaultSchoolId);
                    if (!hasPerm)
                    {
                        context.MenuPermessions.Add(new MenuPermession
                        {
                            MenuId = menu.Id,
                            SubMenuId = null,
                            RoleId = ownerRoleId,
                            IsVisible = true,
                            SchoolRegistrationId = defaultSchoolId,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
