using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Finance;
using School.Infrastructure;

namespace School.Infrastructure.Seeds
{
    public class DefaultFinanceData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            // Seed a default school registration if none exist to prevent tenant constraint issues
            var school = await context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            if (!await context.CoaAccounts.IgnoreQueryFilters().AnyAsync(a => a.SchoolRegistrationId == schoolId))
            {
                var accounts = new List<CoaAccount>
                {
                    new CoaAccount { Code = "10000", Name = "Cash in Hand", AccountType = "Asset", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "11000", Name = "HDFC Operational Bank", AccountType = "Asset", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "12000", Name = "Inventory Stock Ledger", AccountType = "Asset", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "20000", Name = "Accounts Payable (Creditors)", AccountType = "Liability", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "21000", Name = "GST Payable Ledger", AccountType = "Liability", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "30000", Name = "Capital Equity Account", AccountType = "Equity", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "40000", Name = "Tuition Fees Income", AccountType = "Revenue", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "50000", Name = "Staff Salaries Expense", AccountType = "Expense", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow },
                    new CoaAccount { Code = "51000", Name = "Fixed Asset Depreciation Expense", AccountType = "Expense", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow }
                };

                context.CoaAccounts.AddRange(accounts);
                await context.SaveChangesAsync();
            }
        }
    }
}
