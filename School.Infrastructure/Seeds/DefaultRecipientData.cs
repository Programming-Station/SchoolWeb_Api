using Microsoft.EntityFrameworkCore;
using School.Domain.Communication.Recipients;

namespace School.Infrastructure.Seeds
{
    public static class DefaultRecipientData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // ════════════════════════════════════════════════════════════════════
            // 1. SEED RECIPIENT CATEGORIES (8 Categories)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientCategories.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var categories = new List<RecipientCategory>
                {
                    new() { Name = "Academic Staff", Description = "Teaching staff including professors, lecturers, and lab assistants", ColorHex = "#8b5cf6", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Non-Teaching Staff", Description = "Administrative, support, and maintenance staff", ColorHex = "#06b6d4", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Students - Primary", Description = "Students from Class 1 to Class 5", ColorHex = "#10b981", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Students - Secondary", Description = "Students from Class 6 to Class 10", ColorHex = "#3b82f6", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Students - Senior Secondary", Description = "Students from Class 11 and Class 12", ColorHex = "#f59e0b", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Parents & Guardians", Description = "Parents, guardians, and authorized family contacts", ColorHex = "#ef4444", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Management & Board", Description = "Board of directors, trustees, and senior management", ColorHex = "#d946ef", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "External Contacts", Description = "Vendors, suppliers, alumni, and prospective students", ColorHex = "#64748b", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.RecipientCategories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 2. SEED RECIPIENTS (30 realistic Indian school contacts)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.Recipients.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var recipients = new List<Recipient>
                {
                    // ── Students ─────────────────────────────────────────────
                    new() { RecipientCode = "STU-2026-001", RecipientType = "Student", FullName = "Arjun Sharma", DisplayName = "Arjun", Gender = "Male", DateOfBirth = new DateTime(2012, 3, 15), Email = "arjun.sharma@student.school.in", Mobile = "+919876543210", WhatsAppNumber = "+919876543210", PreferredChannel = "WhatsApp", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "STU-2026-002", RecipientType = "Student", FullName = "Priya Patel", DisplayName = "Priya", Gender = "Female", DateOfBirth = new DateTime(2011, 7, 22), Email = "priya.patel@student.school.in", Mobile = "+919812345678", WhatsAppNumber = "+919812345678", PreferredChannel = "Email", City = "Ahmedabad", State = "Gujarat", Country = "India", Pincode = "380001", Language = "Gujarati", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "STU-2026-003", RecipientType = "Student", FullName = "Rahul Verma", DisplayName = "Rahul", Gender = "Male", DateOfBirth = new DateTime(2013, 1, 8), Email = "rahul.verma@student.school.in", Mobile = "+919845678901", WhatsAppNumber = "+919845678901", PreferredChannel = "WhatsApp", City = "Lucknow", State = "Uttar Pradesh", Country = "India", Pincode = "226001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "STU-2026-004", RecipientType = "Student", FullName = "Sneha Reddy", DisplayName = "Sneha", Gender = "Female", DateOfBirth = new DateTime(2012, 11, 5), Email = "sneha.reddy@student.school.in", Mobile = "+919734567890", WhatsAppNumber = "+919734567890", PreferredChannel = "Push Notification", City = "Hyderabad", State = "Telangana", Country = "India", Pincode = "500001", Language = "Telugu", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "STU-2026-005", RecipientType = "Student", FullName = "Mohammed Farhan Khan", DisplayName = "Farhan", Gender = "Male", DateOfBirth = new DateTime(2011, 5, 18), Email = "farhan.khan@student.school.in", Mobile = "+919623456789", WhatsAppNumber = "+919623456789", PreferredChannel = "SMS", City = "Mumbai", State = "Maharashtra", Country = "India", Pincode = "400001", Language = "Urdu", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "STU-2026-006", RecipientType = "Student", FullName = "Ananya Gupta", DisplayName = "Ananya", Gender = "Female", DateOfBirth = new DateTime(2013, 9, 30), Email = "ananya.gupta@student.school.in", Mobile = "+919512345678", WhatsAppNumber = "+919512345678", PreferredChannel = "WhatsApp", City = "Jaipur", State = "Rajasthan", Country = "India", Pincode = "302001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // ── Parents ──────────────────────────────────────────────
                    new() { RecipientCode = "PAR-2026-001", RecipientType = "Parent", FullName = "Rajesh Sharma", DisplayName = "Mr. R. Sharma", Gender = "Male", DateOfBirth = new DateTime(1980, 6, 10), Email = "rajesh.sharma@gmail.com", AlternateEmail = "rajesh.sharma@corporate.in", Mobile = "+919876543200", AlternateMobile = "+919876543201", WhatsAppNumber = "+919876543200", EmergencyContact = "+919876543201", PreferredChannel = "WhatsApp", Address = "B-42, Vasant Kunj", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110070", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "PAR-2026-002", RecipientType = "Parent", FullName = "Meena Patel", DisplayName = "Mrs. M. Patel", Gender = "Female", DateOfBirth = new DateTime(1982, 2, 14), Email = "meena.patel@yahoo.com", Mobile = "+919812345600", WhatsAppNumber = "+919812345600", PreferredChannel = "Email", Address = "Flat 301, Sunrise Apartments, SG Highway", City = "Ahmedabad", State = "Gujarat", Country = "India", Pincode = "380054", Language = "Gujarati", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "PAR-2026-003", RecipientType = "Guardian", FullName = "Suresh Verma", DisplayName = "Mr. S. Verma", Gender = "Male", DateOfBirth = new DateTime(1975, 8, 25), Email = "suresh.verma@hotmail.com", Mobile = "+919845678900", WhatsAppNumber = "+919845678900", PreferredChannel = "SMS", Address = "12/3 Civil Lines, Hazratganj", City = "Lucknow", State = "Uttar Pradesh", Country = "India", Pincode = "226001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "PAR-2026-004", RecipientType = "Parent", FullName = "Lakshmi Reddy", DisplayName = "Mrs. L. Reddy", Gender = "Female", DateOfBirth = new DateTime(1984, 12, 1), Email = "lakshmi.reddy@outlook.com", Mobile = "+919734567800", WhatsAppNumber = "+919734567800", PreferredChannel = "WhatsApp", Address = "Plot 56, Jubilee Hills", City = "Hyderabad", State = "Telangana", Country = "India", Pincode = "500033", Language = "Telugu", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // ── Employees (Teaching Staff) ──────────────────────────
                    new() { RecipientCode = "EMP-2026-001", RecipientType = "Teacher", FullName = "Dr. Kavita Nair", DisplayName = "Dr. Nair", Gender = "Female", DateOfBirth = new DateTime(1978, 4, 12), Email = "kavita.nair@school.in", Mobile = "+919901234567", WhatsAppNumber = "+919901234567", PreferredChannel = "Email", Address = "Flat 12A, Teachers Colony", City = "Bangalore", State = "Karnataka", Country = "India", Pincode = "560001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-002", RecipientType = "Teacher", FullName = "Amit Kumar Singh", DisplayName = "Mr. A.K. Singh", Gender = "Male", DateOfBirth = new DateTime(1985, 10, 20), Email = "amit.singh@school.in", Mobile = "+919887654321", WhatsAppNumber = "+919887654321", PreferredChannel = "WhatsApp", Address = "House 78, Sector 15", City = "Noida", State = "Uttar Pradesh", Country = "India", Pincode = "201301", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-003", RecipientType = "Teacher", FullName = "Sunita Devi", DisplayName = "Mrs. Sunita", Gender = "Female", DateOfBirth = new DateTime(1990, 7, 3), Email = "sunita.devi@school.in", Mobile = "+919776543210", WhatsAppNumber = "+919776543210", PreferredChannel = "SMS", City = "Patna", State = "Bihar", Country = "India", Pincode = "800001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // ── Principal & Vice Principal ───────────────────────────
                    new() { RecipientCode = "EMP-2026-010", RecipientType = "Principal", FullName = "Dr. Ramesh Chandra Mishra", DisplayName = "Dr. R.C. Mishra", Gender = "Male", DateOfBirth = new DateTime(1968, 1, 26), Email = "principal@school.in", AlternateEmail = "rc.mishra@school.in", Mobile = "+919900112233", AlternateMobile = "+919900112234", WhatsAppNumber = "+919900112233", PreferredChannel = "Email", Address = "Principal's Quarters, School Campus", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-011", RecipientType = "Vice Principal", FullName = "Mrs. Anjali Bhatt", DisplayName = "Mrs. A. Bhatt", Gender = "Female", DateOfBirth = new DateTime(1972, 5, 14), Email = "vice.principal@school.in", Mobile = "+919900223344", WhatsAppNumber = "+919900223344", PreferredChannel = "Email", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // ── Non-Teaching Staff ───────────────────────────────────
                    new() { RecipientCode = "EMP-2026-020", RecipientType = "Accountant", FullName = "Vijay Tiwari", DisplayName = "Mr. V. Tiwari", Gender = "Male", DateOfBirth = new DateTime(1988, 3, 8), Email = "accounts@school.in", Mobile = "+919855667788", WhatsAppNumber = "+919855667788", PreferredChannel = "Email", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-021", RecipientType = "Receptionist", FullName = "Pooja Saxena", DisplayName = "Ms. Pooja", Gender = "Female", DateOfBirth = new DateTime(1995, 11, 17), Email = "reception@school.in", Mobile = "+919844556677", WhatsAppNumber = "+919844556677", PreferredChannel = "WhatsApp", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-022", RecipientType = "Librarian", FullName = "Deepak Joshi", DisplayName = "Mr. D. Joshi", Gender = "Male", DateOfBirth = new DateTime(1983, 9, 5), Email = "library@school.in", Mobile = "+919833445566", WhatsAppNumber = "+919833445566", PreferredChannel = "Email", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-023", RecipientType = "Transport Manager", FullName = "Ravi Prakash", DisplayName = "Mr. R. Prakash", Gender = "Male", DateOfBirth = new DateTime(1979, 6, 28), Email = "transport@school.in", Mobile = "+919822334455", WhatsAppNumber = "+919822334455", PreferredChannel = "SMS", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-024", RecipientType = "Driver", FullName = "Manoj Kumar", DisplayName = "Manoj", Gender = "Male", DateOfBirth = new DateTime(1986, 2, 10), Email = "manoj.driver@school.in", Mobile = "+919811223344", WhatsAppNumber = "+919811223344", PreferredChannel = "SMS", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "EMP-2026-025", RecipientType = "Hostel Warden", FullName = "Sanjay Dubey", DisplayName = "Mr. S. Dubey", Gender = "Male", DateOfBirth = new DateTime(1976, 4, 15), Email = "hostel.warden@school.in", Mobile = "+919800112233", WhatsAppNumber = "+919800112233", PreferredChannel = "WhatsApp", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // ── External Contacts ────────────────────────────────────
                    new() { RecipientCode = "VEN-2026-001", RecipientType = "Vendor", FullName = "Sunrise Book Distributors", DisplayName = "Sunrise Books", Gender = "Male", Email = "orders@sunrisebooks.in", Mobile = "+919100200300", WhatsAppNumber = "+919100200300", PreferredChannel = "Email", Address = "42, Nai Sarak, Chandni Chowk", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110006", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "VEN-2026-002", RecipientType = "Supplier", FullName = "TechEdu Solutions Pvt Ltd", DisplayName = "TechEdu", Gender = "Male", Email = "sales@techedu.in", Mobile = "+919200300400", WhatsAppNumber = "+919200300400", PreferredChannel = "Email", Address = "Tower B, IT Park, Sector 62", City = "Noida", State = "Uttar Pradesh", Country = "India", Pincode = "201309", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "ALM-2026-001", RecipientType = "Alumni", FullName = "Vikram Malhotra", DisplayName = "Vikram (Batch 2018)", Gender = "Male", DateOfBirth = new DateTime(2000, 8, 12), Email = "vikram.malhotra@alumni.school.in", Mobile = "+919300400500", WhatsAppNumber = "+919300400500", PreferredChannel = "Email", City = "Pune", State = "Maharashtra", Country = "India", Pincode = "411001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "PRO-2026-001", RecipientType = "Prospective Student", FullName = "Aarav Mehta", DisplayName = "Aarav", Gender = "Male", DateOfBirth = new DateTime(2014, 4, 20), Email = "mehta.family@gmail.com", Mobile = "+919400500600", WhatsAppNumber = "+919400500600", PreferredChannel = "WhatsApp", City = "Gurgaon", State = "Haryana", Country = "India", Pincode = "122001", Language = "Hindi", TimeZone = "Asia/Kolkata", IsVerified = false, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },

                    // ── Management ───────────────────────────────────────────
                    new() { RecipientCode = "MGT-2026-001", RecipientType = "Management", FullName = "Shri Arun Kumar Agarwal", DisplayName = "Chairman", Gender = "Male", DateOfBirth = new DateTime(1960, 1, 1), Email = "chairman@school.in", Mobile = "+919900990099", WhatsAppNumber = "+919900990099", PreferredChannel = "Email", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { RecipientCode = "ADM-2026-001", RecipientType = "Administrator", FullName = "Neha Kapoor", DisplayName = "Ms. N. Kapoor", Gender = "Female", DateOfBirth = new DateTime(1990, 7, 14), Email = "admin@school.in", Mobile = "+919988776655", WhatsAppNumber = "+919988776655", PreferredChannel = "Email", City = "New Delhi", State = "Delhi", Country = "India", Pincode = "110001", Language = "English", TimeZone = "Asia/Kolkata", IsVerified = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.Recipients.AddRange(recipients);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 3. SEED RECIPIENT GROUPS (10 Dynamic Groups)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientGroups.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var groups = new List<RecipientGroup>
                {
                    new() { Name = "All Students", Description = "Every enrolled student across all classes and sections", IsDynamic = true, DynamicFilterCriteria = "{\"type\":\"Student\"}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "All Parents & Guardians", Description = "All parents and guardians linked to enrolled students", IsDynamic = true, DynamicFilterCriteria = "{\"type\":[\"Parent\",\"Guardian\"]}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "All Teaching Staff", Description = "All teachers, professors, and academic coordinators", IsDynamic = true, DynamicFilterCriteria = "{\"type\":\"Teacher\"}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "All Employees", Description = "Every employee including teaching and non-teaching staff", IsDynamic = true, DynamicFilterCriteria = "{\"type\":[\"Teacher\",\"Accountant\",\"Receptionist\",\"Librarian\",\"Transport Manager\",\"Driver\",\"Hostel Warden\"]}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "School Management", Description = "Board of directors, principal, vice principal, and administrators", IsDynamic = true, DynamicFilterCriteria = "{\"type\":[\"Management\",\"Principal\",\"Vice Principal\",\"Administrator\"]}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Transport Team", Description = "Transport managers, drivers, and conductors", IsDynamic = true, DynamicFilterCriteria = "{\"type\":[\"Transport Manager\",\"Driver\"]}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Fee Reminder Recipients", Description = "Parents and students who receive fee payment reminders", IsDynamic = true, DynamicFilterCriteria = "{\"type\":[\"Parent\",\"Guardian\",\"Student\"],\"tag\":\"Fee Pending\"}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Emergency Broadcast", Description = "All active recipients for emergency notifications", IsDynamic = true, DynamicFilterCriteria = "{\"isActive\":true}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Alumni Network", Description = "Former students for alumni events and networking", IsDynamic = true, DynamicFilterCriteria = "{\"type\":\"Alumni\"}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Vendors & Suppliers", Description = "External business contacts for procurement and services", IsDynamic = true, DynamicFilterCriteria = "{\"type\":[\"Vendor\",\"Supplier\"]}", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.RecipientGroups.AddRange(groups);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 4. SEED RECIPIENT TAGS (Sample tags on a few recipients)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientTags.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var arjun = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-001" && r.SchoolRegistrationId == schoolId);
                var priya = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-002" && r.SchoolRegistrationId == schoolId);
                var farhan = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-005" && r.SchoolRegistrationId == schoolId);
                var principal = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "EMP-2026-010" && r.SchoolRegistrationId == schoolId);
                var chairman = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "MGT-2026-001" && r.SchoolRegistrationId == schoolId);

                var tags = new List<RecipientTag>();
                if (arjun != null) { tags.Add(new() { RecipientId = arjun.Id, TagName = "Sports", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); tags.Add(new() { RecipientId = arjun.Id, TagName = "Hosteller", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); }
                if (priya != null) { tags.Add(new() { RecipientId = priya.Id, TagName = "Scholarship", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); tags.Add(new() { RecipientId = priya.Id, TagName = "Transport", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); }
                if (farhan != null) { tags.Add(new() { RecipientId = farhan.Id, TagName = "Fee Pending", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); }
                if (principal != null) { tags.Add(new() { RecipientId = principal.Id, TagName = "VIP", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); }
                if (chairman != null) { tags.Add(new() { RecipientId = chairman.Id, TagName = "VIP", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }); }

                if (tags.Count > 0)
                {
                    context.RecipientTags.AddRange(tags);
                    await context.SaveChangesAsync();
                }
            }

            // ════════════════════════════════════════════════════════════════════
            // 5. SEED DISTRIBUTION LISTS (3 Lists)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.DistributionLists.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var lists = new List<DistributionList>
                {
                    new() { Name = "Monthly Newsletter", Description = "All parents, staff, and alumni who receive the monthly school newsletter", ListType = "Internal", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Fee Collection Alerts", Description = "Parents and guardians who receive automatic fee payment reminders", ListType = "Internal", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Vendor Communications", Description = "External vendors and suppliers for procurement and tender notices", ListType = "External", SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.DistributionLists.AddRange(lists);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 6. SEED RECIPIENT GROUP MEMBERS (Static memberships)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientGroupMembers.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var arjun = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-001" && r.SchoolRegistrationId == schoolId);
                var priya = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-002" && r.SchoolRegistrationId == schoolId);
                var kavita = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "EMP-2026-001" && r.SchoolRegistrationId == schoolId);

                var allStudentsGroup = await context.RecipientGroups.FirstOrDefaultAsync(g => g.Name == "All Students" && g.SchoolRegistrationId == schoolId);
                var allTeachersGroup = await context.RecipientGroups.FirstOrDefaultAsync(g => g.Name == "All Teaching Staff" && g.SchoolRegistrationId == schoolId);

                var members = new List<RecipientGroupMember>();

                if (arjun != null && allStudentsGroup != null)
                {
                    members.Add(new() { RecipientId = arjun.Id, GroupId = allStudentsGroup.Id, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }
                if (priya != null && allStudentsGroup != null)
                {
                    members.Add(new() { RecipientId = priya.Id, GroupId = allStudentsGroup.Id, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }
                if (kavita != null && allTeachersGroup != null)
                {
                    members.Add(new() { RecipientId = kavita.Id, GroupId = allTeachersGroup.Id, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (members.Count > 0)
                {
                    context.RecipientGroupMembers.AddRange(members);
                    await context.SaveChangesAsync();
                }
            }

            // ════════════════════════════════════════════════════════════════════
            // 7. SEED RECIPIENT PREFERENCES (Opt-in / Opt-out settings)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientPreferences.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var arjun = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-001" && r.SchoolRegistrationId == schoolId);
                var priya = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-002" && r.SchoolRegistrationId == schoolId);

                var preferences = new List<RecipientPreference>();

                if (arjun != null)
                {
                    preferences.Add(new() { RecipientId = arjun.Id, ChannelType = "WhatsApp", MessageCategory = "Alert", IsOptIn = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                    preferences.Add(new() { RecipientId = arjun.Id, ChannelType = "Email", MessageCategory = "Newsletter", IsOptIn = false, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (priya != null)
                {
                    preferences.Add(new() { RecipientId = priya.Id, ChannelType = "SMS", MessageCategory = "Circular", IsOptIn = true, SchoolRegistrationId = schoolId, IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" });
                }

                if (preferences.Count > 0)
                {
                    context.RecipientPreferences.AddRange(preferences);
                    await context.SaveChangesAsync();
                }
            }

            // ════════════════════════════════════════════════════════════════════
            // 8. SEED RECIPIENT HISTORY (Audit Trail)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientHistories.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var arjun = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-001" && r.SchoolRegistrationId == schoolId);
                var priya = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-002" && r.SchoolRegistrationId == schoolId);

                var histories = new List<RecipientHistory>();

                if (arjun != null)
                {
                    histories.Add(new()
                    {
                        RecipientId = arjun.Id,
                        Channel = "WhatsApp",
                        MessageSubject = "Welcome Alert",
                        MessageBody = "Welcome Arjun Sharma to the new School ERP portal!",
                        DeliveryStatus = "Read",
                        SentAt = DateTime.UtcNow.AddDays(-2),
                        DeliveredAt = DateTime.UtcNow.AddDays(-2).AddMinutes(1),
                        ReadAt = DateTime.UtcNow.AddDays(-2).AddMinutes(5),
                        SchoolRegistrationId = schoolId,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                if (priya != null)
                {
                    histories.Add(new()
                    {
                        RecipientId = priya.Id,
                        Channel = "Email",
                        MessageSubject = "Tuition Fee Outstanding Reminder",
                        MessageBody = "Dear Parent, this is a reminder that the tuition fee for Priya Patel is pending for Term 1.",
                        DeliveryStatus = "Sent",
                        SentAt = DateTime.UtcNow.AddDays(-1),
                        SchoolRegistrationId = schoolId,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                if (histories.Count > 0)
                {
                    context.RecipientHistories.AddRange(histories);
                    await context.SaveChangesAsync();
                }
            }

            // ════════════════════════════════════════════════════════════════════
            // 9. SEED RECIPIENT ACTIVITIES (Activity telemetries)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientActivities.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var arjun = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-001" && r.SchoolRegistrationId == schoolId);
                var activities = new List<RecipientActivity>();

                if (arjun != null)
                {
                    activities.Add(new()
                    {
                        RecipientId = arjun.Id,
                        ActivityType = "ProfileCreated",
                        Description = "Recipient profile generated automatically via database seeds",
                        ActivityDate = DateTime.UtcNow.AddDays(-3),
                        SchoolRegistrationId = schoolId,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                    activities.Add(new()
                    {
                        RecipientId = arjun.Id,
                        ActivityType = "WhatsAppVerified",
                        Description = "WhatsApp contact verified successfully",
                        ActivityDate = DateTime.UtcNow.AddDays(-2),
                        SchoolRegistrationId = schoolId,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    });
                }

                if (activities.Count > 0)
                {
                    context.RecipientActivities.AddRange(activities);
                    await context.SaveChangesAsync();
                }
            }

            // ════════════════════════════════════════════════════════════════════
            // 10. SEED RECIPIENT BLACKLIST (Bounce / Spam / Blocklist management)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.RecipientBlacklists.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                context.RecipientBlacklists.Add(new()
                {
                    BlockedAddress = "bounced-parent-email@invalid-domain.com",
                    Channel = "Email",
                    Reason = "Hard bounce - user mailbox does not exist",
                    BlockedAt = DateTime.UtcNow.AddDays(-5),
                    SchoolRegistrationId = schoolId,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                });
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 11. SEED EMAIL RECIPIENTS & ATTACHMENTS (Broadcast logs)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.EmailRecipients.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var arjun = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-001" && r.SchoolRegistrationId == schoolId);
                var priya = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "STU-2026-002" && r.SchoolRegistrationId == schoolId);
                var kavita = await context.Recipients.FirstOrDefaultAsync(r => r.RecipientCode == "EMP-2026-001" && r.SchoolRegistrationId == schoolId);

                // Create a parent EmailLog (representing the message)
                var emailLog = new School.Domain.Email.EmailLog
                {
                    SchoolRegistrationId = schoolId,
                    TemplateName = "System Summary Report",
                    RecipientEmail = arjun?.Email ?? "arjun.sharma@student.school.in",
                    Subject = "Welcome Kit and Orientation Guide 2026",
                    BodyHtml = "<p>Dear Students and Staff, please find attached the welcome kit for this academic session.</p>",
                    Status = "Sent",
                    SentTime = DateTime.UtcNow.AddDays(-1),
                    IsDraft = false,
                    IsScheduled = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed",
                    IsActive = true
                };

                context.EmailLogs.Add(emailLog);
                await context.SaveChangesAsync(); // Generates the EmailLog ID

                // Create recipients mapping (To, CC, BCC)
                var recipients = new List<EmailRecipient>();
                if (arjun != null)
                {
                    recipients.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        EmailMessageId = emailLog.Id,
                        AddressBookId = arjun.Id,
                        EmailAddress = arjun.Email,
                        RecipientType = RecipientType.To,
                        DisplayName = arjun.FullName,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    });
                }
                if (priya != null)
                {
                    recipients.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        EmailMessageId = emailLog.Id,
                        AddressBookId = priya.Id,
                        EmailAddress = priya.Email,
                        RecipientType = RecipientType.Cc,
                        DisplayName = priya.FullName,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    });
                }
                if (kavita != null)
                {
                    recipients.Add(new()
                    {
                        SchoolRegistrationId = schoolId,
                        EmailMessageId = emailLog.Id,
                        AddressBookId = kavita.Id,
                        EmailAddress = kavita.Email,
                        RecipientType = RecipientType.Bcc,
                        DisplayName = kavita.FullName,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed",
                        IsActive = true
                    });
                }

                if (recipients.Count > 0)
                {
                    context.EmailRecipients.AddRange(recipients);
                }

                // Add sample attachment
                context.EmailAttachments.Add(new()
                {
                    SchoolRegistrationId = schoolId,
                    EmailLogId = emailLog.Id,
                    FileName = "WelcomeGuide2026.pdf",
                    ContentType = "application/pdf",
                    FileBytes = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2d, 0x31, 0x2e, 0x34 }, // Mock PDF header bytes
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed",
                    IsActive = true
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
