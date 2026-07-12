using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Inventory;
using School.Infrastructure;

namespace School.Infrastructure.Seeds
{
    public class DefaultInventoryData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            if (!await context.ItemCategories.IgnoreQueryFilters().AnyAsync(c => c.SchoolRegistrationId == schoolId))
            {
                var stationery = new ItemCategory { Name = "Stationery Supplies", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var labs = new ItemCategory { Name = "Science Lab Equipment", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                context.ItemCategories.AddRange(stationery, labs);
                await context.SaveChangesAsync();

                // Fetch asset & expense accounts
                var stockAsset = await context.CoaAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Code == "12000");
                var expenseAcc = await context.CoaAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Code == "51000");
                var creditorAcc = await context.CoaAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Code == "20000");

                if (!await context.InventoryItems.IgnoreQueryFilters().AnyAsync(i => i.SchoolRegistrationId == schoolId))
                {
                    var notepad = new InventoryItem
                    {
                        Sku = "ST-NOTEPAD-01",
                        Name = "Executive Notepad A4",
                        CategoryId = stationery.Id,
                        Uom = "pcs",
                        MinStockLevel = 10,
                        CurrentStock = 100,
                        UnitPrice = 45.00m,
                        AssetAccountId = stockAsset?.Id,
                        ExpenseAccountId = expenseAcc?.Id,
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow
                    };

                    var beaker = new InventoryItem
                    {
                        Sku = "LB-BEAKER-250",
                        Name = "Borosilicate Beaker 250ml",
                        CategoryId = labs.Id,
                        Uom = "pcs",
                        MinStockLevel = 5,
                        CurrentStock = 20,
                        UnitPrice = 120.00m,
                        AssetAccountId = stockAsset?.Id,
                        ExpenseAccountId = expenseAcc?.Id,
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow
                    };

                    context.InventoryItems.AddRange(notepad, beaker);
                }

                if (!await context.Vendors.IgnoreQueryFilters().AnyAsync(v => v.SchoolRegistrationId == schoolId))
                {
                    var vendor = new Vendor
                    {
                        Code = "VND-DELHI-01",
                        Name = "Delhi Books and Stationers Ltd",
                        ContactPerson = "Amit Sharma",
                        Email = "sales@delhibooks.com",
                        Phone = "011-4521635",
                        TaxRegistrationNo = "07AAAAA1111A1Z1",
                        CreditorAccountId = creditorAcc?.Id,
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow
                    };

                    context.Vendors.Add(vendor);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
