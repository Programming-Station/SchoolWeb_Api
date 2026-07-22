using Microsoft.EntityFrameworkCore;
using School.Domain.Communication;
using School.Utilities.Constants;

namespace School.Infrastructure.Seeds
{
    public static class DefaultCommunicationData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // ════════════════════════════════════════════════════════════════════
            // 1. SEED TEMPLATES (10+ Templates)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.CommunicationTemplates.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var templates = new List<CommunicationTemplate>
                {
                    new() { Name = "Fee Outstanding Reminder (Email)", Type = "Email", SubjectTemplate = "Fee Outstanding Reminder - {{StudentName}}", BodyTemplate = "Dear Parent,<br/><br/>This is a reminder that the tuition fee of {{Amount}} for your ward {{StudentName}} is outstanding for the term {{TermName}}. Please clear it before {{DueDate}} to avoid late fine.<br/><br/>Regards,<br/>Accounts Department", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Attendance Absent Alert (SMS)", Type = "SMS", BodyTemplate = "Dear Parent, your ward {{StudentName}} is marked ABSENT today, {{Date}}. If this is in error, please contact the class teacher.", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Hostel Bus Delay (WhatsApp)", Type = "WhatsApp", BodyTemplate = "Dear Parent, school bus route {{RouteNo}} is delayed by {{DelayMinutes}} minutes due to traffic. Expected arrival is {{ExpectedTime}}. Track live status on portal.", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Midterm Report Card Dispatch (Email)", Type = "Email", SubjectTemplate = "Midterm Progress Report Card - Grade {{GradeName}}", BodyTemplate = "Dear Parent,<br/><br/>The Term 1 Progress Report Card for your ward {{StudentName}} has been published. Please log in to the parent portal to download and review.<br/><br/>Regards,<br/>Principal Office", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Holiday Notice (SMS)", Type = "SMS", BodyTemplate = "School Alert: In observance of public holiday, the school will remain closed on {{HolidayDate}}. Classes resume on {{ResumeDate}}.", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Library Book Overdue (Email)", Type = "Email", SubjectTemplate = "Overdue Library Book Notification", BodyTemplate = "Dear Student,<br/><br/>The library book titled '{{BookTitle}}' issued to you was due on {{DueDate}}. Please return it immediately to avoid overdue fines.<br/><br/>Regards,<br/>Library Desk", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Admission Registered Confirmation (Email)", Type = "Email", SubjectTemplate = "Admission Application Registered Successfully", BodyTemplate = "Dear Applicant,<br/><br/>Thank you for registering. Your application ref number is {{AppRef}}. We will contact you for entrance test details.<br/><br/>Regards,<br/>Admissions Desk", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Leave Approved Alert (WhatsApp)", Type = "WhatsApp", BodyTemplate = "Hello {{StaffName}}, your leave application for dates {{Dates}} has been APPROVED by the management. Enjoy your time off!", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Exam Timetable Published (Push)", Type = "Push", SubjectTemplate = "Midterm Examination Timetable", BodyTemplate = "The Midterm examination schedule for Grade {{Grade}} has been published. Click to view timetable sheet.", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Emergency Contact Update (SMS)", Type = "SMS", BodyTemplate = "ERP Security: Emergency contact number for ward {{StudentName}} has been updated successfully. Contact support if not done by you.", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Name = "Salary Credit Notification (Email)", Type = "Email", SubjectTemplate = "Payslip Credit Confirmation - {{MonthName}}", BodyTemplate = "Dear Team,<br/><br/>Your salary slip for {{MonthName}} has been dispatched. Net credited amount is {{Amount}}. View detail summary on employee portal.<br/><br/>Regards,<br/>HR Operations", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.CommunicationTemplates.AddRange(templates);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 2. SEED ANNOUNCEMENTS (10+ Announcements)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.Announcements.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var announcements = new List<Announcement>
                {
                    new() { Title = "Annual Science Exhibition 2026", Content = "We are pleased to announce the Annual Science Exhibition to be held on September 15th, 2026. All students from grades 6 to 12 are encouraged to submit project summaries by the end of this month.", Scope = "School", Priority = "High", IsPinned = true, ExpiryDate = DateTime.UtcNow.AddMonths(2), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Staff Pedagogical Workshop", Content = "An interactive training session on 'Modern Pedagogy and AI tools in Education' is scheduled for all secondary faculty on Friday, August 28th, 2026.", Scope = "Department", TargetReferenceId = "Secondary", Priority = "Medium", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddDays(30), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Inter-School Sports Gala Registrations", Content = "Registrations are open for the annual sports meet. Events include football, basketball, track & field, and chess. Interested students register with Coach Miller.", Scope = "School", Priority = "Medium", IsPinned = true, ExpiryDate = DateTime.UtcNow.AddDays(15), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Winter Uniform Mandatory Notice", Content = "Effective November 1st, all students must wear the official navy blue blazers and woolen trousers. Uniform inspection will be conducted daily during morning assemblies.", Scope = "School", Priority = "Low", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddMonths(3), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Midterm Syllabus Revision Schedule", Content = "Class teachers have uploaded midterm study guides and revision worksheets in the documents folder. Parents are requested to ensure students complete assignments.", Scope = "Class", TargetReferenceId = "Class 10-A", Priority = "High", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddDays(20), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Mathematics Olympiad Olympiads Registrations", Content = "High school students interested in participating in the National Mathematics Olympiad register before the end of next week. Sample books are available in library.", Scope = "School", Priority = "Medium", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddDays(10), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Canteen Nutrition Standard Update", Content = "We are updating our school cafeteria menu to include healthier alternatives. Carbonated drinks are replaced with fresh fruit juices starting next Monday.", Scope = "School", Priority = "Low", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddDays(15), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Parent Portal Server Maintenance", Content = "The parent and employee dashboard will experience down-time on Sunday between 12:00 AM and 4:00 AM due to scheduled database server migrations and optimization.", Scope = "Public", Priority = "Low", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddDays(5), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Independence Day Celebration Program", Content = "The school will host a cultural parade and assembly to celebrate Independence Day on August 15th. Flag hoisting ceremony is at 8:00 AM sharp. Dress code: White.", Scope = "School", Priority = "High", IsPinned = true, ExpiryDate = DateTime.UtcNow.AddDays(25), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Classroom Smartboards Upgrades", Content = "Smart interactive boards are being installed in all secondary wing classrooms to facilitate dynamic visuals and interactive classroom sessions.", Scope = "Department", TargetReferenceId = "Secondary", Priority = "Medium", IsPinned = false, ExpiryDate = DateTime.UtcNow.AddDays(10), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.Announcements.AddRange(announcements);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 3. SEED MEETINGS (10+ Meetings)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.CommunicationMeetings.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var meetings = new List<CommunicationMeeting>
                {
                    new() { Title = "PTA Meeting - Term 1 Progress Review", Description = "Discussion regarding academic curriculum, upcoming assessments, and feedback collection.", StartTime = DateTime.UtcNow.AddDays(3).Date.AddHours(10), EndTime = DateTime.UtcNow.AddDays(3).Date.AddHours(12), Platform = "Zoom", MeetingLink = "https://zoom.us/j/9876543210?pwd=PTA_Review_2026", MeetingId = "987 654 3210", MeetingPassword = "PTA_Review_2026", Agenda = "1. Term 1 syllabus progress.\n2. Assessment pattern.\n3. Extracurricular schedules.\n4. Q&A Session.", Status = "Scheduled", TargetAudience = "Parents", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Weekly Departmental Alignment Meet", Description = "Weekly sync for syllabus tracking and resource review.", StartTime = DateTime.UtcNow.AddDays(1).Date.AddHours(14), EndTime = DateTime.UtcNow.AddDays(1).Date.AddHours(15), Platform = "Teams", MeetingLink = "https://teams.microsoft.com/l/meetup-join/weekly-alignment-2026", Status = "Scheduled", TargetAudience = "Employees", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Board of Directors Quarter Strategy Meet", Description = "Quarterly alignment on finance budgets, expansion logs, and curriculum plans.", StartTime = DateTime.UtcNow.AddDays(10).Date.AddHours(11), EndTime = DateTime.UtcNow.AddDays(10).Date.AddHours(13), Platform = "Zoom", MeetingLink = "https://zoom.us/j/1234567890?pwd=Board_Sync_2026", Status = "Scheduled", TargetAudience = "Employees", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "New Student Orientation Session", Description = "Welcome instructions for all newly admitted students and guidelines on code of conduct.", StartTime = DateTime.UtcNow.AddDays(5).Date.AddHours(9), EndTime = DateTime.UtcNow.AddDays(5).Date.AddHours(11), Platform = "GoogleMeet", MeetingLink = "https://meet.google.com/abc-defg-hij", Status = "Scheduled", TargetAudience = "Parents", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Teacher Training on AI in Grading", Description = "Detailed workshop on implementing smart marking assistants to save administrative effort.", StartTime = DateTime.UtcNow.AddDays(2).Date.AddHours(15), EndTime = DateTime.UtcNow.AddDays(2).Date.AddHours(17), Platform = "Zoom", MeetingLink = "https://zoom.us/j/888777666?pwd=AI_Grading", Status = "Scheduled", TargetAudience = "Employees", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Science Project Mid-Term Sync", Description = "Reviewing model projects and checking student progress guides.", StartTime = DateTime.UtcNow.AddDays(-1).Date.AddHours(10), EndTime = DateTime.UtcNow.AddDays(-1).Date.AddHours(11), Platform = "Zoom", MeetingLink = "https://zoom.us/j/333444555", Status = "Completed", MinutesOfMeeting = "Reviewed 15 student project drafts. Approved 12. 3 students requested extension.", TargetAudience = "Parents", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-2), CreatedBy = "seed" },
                    new() { Title = "Secondary Subject Coordinators Alignment", Description = "Syllabus tracking for upcoming mid-term exams.", StartTime = DateTime.UtcNow.AddDays(4).Date.AddHours(13), EndTime = DateTime.UtcNow.AddDays(4).Date.AddHours(14), Platform = "GoogleMeet", MeetingLink = "https://meet.google.com/xyz-qwe-mnp", Status = "Scheduled", TargetAudience = "Employees", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Alumni Reunion Committee Sync", Description = "Planning logistics and budget approvals for the upcoming alumni dinner.", StartTime = DateTime.UtcNow.AddDays(15).Date.AddHours(17), EndTime = DateTime.UtcNow.AddDays(15).Date.AddHours(19), Platform = "Teams", MeetingLink = "https://teams.microsoft.com/l/meetup-join/alumni-sync-2026", Status = "Scheduled", TargetAudience = "All", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Transport Safety Review Meeting", Description = "Sync with bus drivers to review speed limits, GPS tracking, and route audits.", StartTime = DateTime.UtcNow.AddDays(6).Date.AddHours(14), EndTime = DateTime.UtcNow.AddDays(6).Date.AddHours(15), Platform = "Physical", Status = "Scheduled", TargetAudience = "Employees", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Title = "Hostel Wardens Assembly", Description = "Monthly review of food supplies, room cleaning schedules, and student complaints.", StartTime = DateTime.UtcNow.AddDays(8).Date.AddHours(10), EndTime = DateTime.UtcNow.AddDays(8).Date.AddHours(12), Platform = "Physical", Status = "Scheduled", TargetAudience = "Employees", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.CommunicationMeetings.AddRange(meetings);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 4. SEED SUPPORT TICKETS (10+ Tickets)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.SupportTickets.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var tickets = new List<SupportTicket>
                {
                    new() { TicketNumber = "TKT-2026-0001", Subject = "Unable to access Fee Receipt download link", Description = "I paid the term fee yesterday, but when I click the print link under Parent Portal, it displays a blank screen. Please assist.", Category = "Fee", Status = "Open", Priority = "High", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddHours(24), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-4), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0002", Subject = "Bus Route 5 Delay Query", Description = "The morning bus for Route 5 is consistently arriving 20 minutes late at our stop. This causes my son to miss the assembly session.", Category = "Transport", Status = "InProgress", Priority = "Medium", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddHours(48), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-1), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0003", Subject = "Incorrect Grade marked in Biology Test 1", Description = "My Biology Test 1 score sheet shows 18/20, but on parent dashboard, it shows 8/20. Please double check.", Category = "Academic", Status = "Open", Priority = "Medium", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddDays(3), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-12), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0004", Subject = "Smartcard Identity Lost Request", Description = "My child lost his smart card inside school yesterday. Please block the existing card and issue a replacement card.", Category = "General", Status = "Open", Priority = "Low", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddDays(5), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-2), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0005", Subject = "Hostel Room AC Fan Noise Issue", Description = "Hostel room 402 AC fan is making extremely loud rattling noises during night. Students cannot sleep properly.", Category = "Hostel", Status = "InProgress", Priority = "High", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddHours(24), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-2), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0006", Subject = "Sibling Discount Request", Description = "Both of my children are studying here. I applied for the sibling discount, but the latest bill is showing full tuition fees.", Category = "Fee", Status = "Open", Priority = "High", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddHours(36), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-18), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0007", Subject = "Hostel Food Quality Concerns", Description = "Multiple parents have raised concerns about the lack of green vegetables in the hostel menu. Please add more salads.", Category = "Hostel", Status = "Resolved", Priority = "Medium", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddHours(72), ResolutionNotes = "Mess coordinator updated. Green salad added to mess menu daily.", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-5), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0008", Subject = "Unable to upload math assignment", Description = "When submitting the algebra assignment, the portal throws a server validation error 500. File is 3.5MB pdf.", Category = "IT", Status = "Open", Priority = "Medium", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddDays(2), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-1), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0009", Subject = "Bus Driver Rash Driving Complaint", Description = "Parents reported bus Route 3 driver driving aggressively near High Street roundabout. Please investigate.", Category = "Transport", Status = "Resolved", Priority = "High", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddHours(24), ResolutionNotes = "Driver warned. CCTV checked and supervisor verified safety standards.", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-3), CreatedBy = "student" },
                    new() { TicketNumber = "TKT-2026-0010", Subject = "Syllabus queries for midterm maths", Description = "We need clarification if Chapter 5 Statistics is included in Grade 10 midterm assessment.", Category = "Academic", Status = "Resolved", Priority = "Low", RaisedByUserId = Constants.StudentUser, AssignedStaffId = Constants.AdminUser, SLAExpiryDate = DateTime.UtcNow.AddDays(4), ResolutionNotes = "Chapter 5 is excluded. Syllabus sheet updated.", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-4), CreatedBy = "student" }
                };

                context.SupportTickets.AddRange(tickets);
                await context.SaveChangesAsync();

                // Seed Ticket Response for ticket2
                var response = new TicketResponse
                {
                    TicketId = tickets[1].Id,
                    SenderUserId = Constants.AdminUser,
                    Content = "Dear Parent, we have contacted the transport supervisor. They mentioned that road construction near Sector 12 is causing the delay. We are rerouting the bus from tomorrow to ensure timely pickup.",
                    IsInternalNote = false,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddHours(-1),
                    CreatedBy = "admin"
                };
                context.TicketResponses.Add(response);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 5. SEED QUICK POLLS (5+ Polls)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.QuickPolls.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var polls = new List<QuickPoll>
                {
                    new() { Question = "Would you support starting the school day 30 minutes earlier in summer (7:30 AM instead of 8:00 AM)?", OptionsJson = "[\"Yes, support\", \"No, do not support\", \"Neutral / Indifferent\"]", StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow.AddDays(5), IsActive = true, TargetAudience = "Parents", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Question = "Select your preferred date for the Science Fair Project Submission.", OptionsJson = "[\"Oct 10\", \"Oct 15\", \"Oct 20\"]", StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(4), IsActive = true, TargetAudience = "Parents", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Question = "Which extra-curricular sport would you like to see introduced in high school?", OptionsJson = "[\"Lawn Tennis\", \"Swimming\", \"Archery\", \"Gymnastics\"]", StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(10), IsActive = true, TargetAudience = "All", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Question = "Should we introduce digital textbooks instead of physical textbooks for secondary grades next session?", OptionsJson = "[\"Strongly Agree\", \"Agree\", \"Disagree\"]", StartDate = DateTime.UtcNow.AddDays(-4), EndDate = DateTime.UtcNow.AddDays(3), IsActive = true, TargetAudience = "Parents", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { Question = "Are you satisfied with cafeteria food menus?", OptionsJson = "[\"Highly Satisfied\", \"Needs Improvement\", \"Dissatisfied\"]", StartDate = DateTime.UtcNow.AddDays(-10), EndDate = DateTime.UtcNow.AddDays(-2), IsActive = false, TargetAudience = "All", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };

                context.QuickPolls.AddRange(polls);
                await context.SaveChangesAsync();

                // Seed some votes for first poll
                var votes = new List<PollVote>
                {
                    new() { PollId = polls[0].Id, UserId = Constants.StudentUser, SelectedOption = "Yes, support", SchoolRegistrationId = schoolId, VotedAt = DateTime.UtcNow.AddDays(-1) },
                    new() { PollId = polls[0].Id, UserId = Constants.EmployeeUser, SelectedOption = "No, do not support", SchoolRegistrationId = schoolId, VotedAt = DateTime.UtcNow.AddHours(-12) }
                };
                context.PollVotes.AddRange(votes);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 6. SEED SHARED DOCUMENTS (10+ Documents)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.SharedDocuments.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var docs = new List<SharedDocument>
                {
                    new() { FileName = "Academic Syllabus 2026-27.pdf", Description = "Complete course breakdown, grading criteria, and text book reference list.", FilePath = "/uploads/documents/syllabus_2026_27.pdf", FileSize = 2548000, FileType = "pdf", TargetAudience = "All", DownloadCount = 15, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Student Health & Safety Policy.docx", Description = "Medical policies, emergency guidelines, and campus rules.", FilePath = "/uploads/documents/safety_policy.docx", FileSize = 450000, FileType = "docx", TargetAudience = "Parents", DownloadCount = 3, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "School Bus Route Guideline.xlsx", Description = "Bus stop listings, pick up timings, and drivers phone directory.", FilePath = "/uploads/documents/bus_routes.xlsx", FileSize = 120000, FileType = "xlsx", TargetAudience = "All", DownloadCount = 38, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Science Fair Rules Sheet.pdf", Description = "Judgement parameters, registration limits, and criteria checklist.", FilePath = "/uploads/documents/science_fair_rules.pdf", FileSize = 850000, FileType = "pdf", TargetAudience = "All", DownloadCount = 22, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Hostel Room Rent Structure.pdf", Description = "Hostel room rent options, food schedules, and check-in steps.", FilePath = "/uploads/documents/hostel_rent.pdf", FileSize = 512000, FileType = "pdf", TargetAudience = "Parents", DownloadCount = 8, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Teacher Lesson Planner Template.docx", Description = "Lesson plan template structure guidelines for teachers.", FilePath = "/uploads/documents/lesson_planner.docx", FileSize = 98000, FileType = "docx", TargetAudience = "Teachers", DownloadCount = 45, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Canteen Food Price List.xlsx", Description = " cafetaria daily item list with item pricing.", FilePath = "/uploads/documents/canteen_pricing.xlsx", FileSize = 48000, FileType = "xlsx", TargetAudience = "All", DownloadCount = 10, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Student Leave Application Form.pdf", Description = "Parent signed leave permission slip template.", FilePath = "/uploads/documents/leave_form.pdf", FileSize = 150000, FileType = "pdf", TargetAudience = "Parents", DownloadCount = 4, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Midterm Exam Guidelines.docx", Description = "Exam protocols, roll number checks, and materials instructions.", FilePath = "/uploads/documents/exam_rules.docx", FileSize = 310000, FileType = "docx", TargetAudience = "All", DownloadCount = 60, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new() { FileName = "Teacher Appraisal Handbook.pdf", Description = "Monthly appraisal KPI listings and instructions manual.", FilePath = "/uploads/documents/appraisal_guide.pdf", FileSize = 1800000, FileType = "pdf", TargetAudience = "Teachers", DownloadCount = 14, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.SharedDocuments.AddRange(docs);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 7. SEED NOTICE BOARDS (10+ Notices)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.NoticeBoards.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var notices = new List<NoticeBoard>
                {
                    new() { Title = "Summer Vacation Holiday Notice", Content = "The school will remain closed for summer vacation starting June 1st, 2026 to July 5th, 2026. Online classes will resume from July 6th.", TargetAudience = "All", PublishedDate = DateTime.UtcNow.AddDays(-5), IsPinned = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-5), CreatedBy = "admin" },
                    new() { Title = "Fee Submission Timeline Extension", Content = "The last date for Term 1 tuition fee submission has been extended to July 25th, 2026 without late fee charges.", TargetAudience = "Parents", PublishedDate = DateTime.UtcNow.AddDays(-1), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-1), CreatedBy = "admin" },
                    new() { Title = "Annual Sports Day Date & Timings", Content = "Sports Day will be held on Oct 18th in the main stadium. All events start from 9:00 AM.", TargetAudience = "All", PublishedDate = DateTime.UtcNow.AddDays(-10), IsPinned = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-10), CreatedBy = "admin" },
                    new() { Title = "Lost & Found: Blue geometry box", Content = "A blue metal geometry box was found in high school corridor. Collect from library reception.", TargetAudience = "All", PublishedDate = DateTime.UtcNow.AddDays(-2), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-2), CreatedBy = "admin" },
                    new() { Title = "School Bus Route 4 Rerouted", Content = "Route 4 bus pick up timeline shifted 10 mins early due to road construction at MG road block.", TargetAudience = "Parents", PublishedDate = DateTime.UtcNow.AddDays(-3), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-3), CreatedBy = "admin" },
                    new() { Title = "Staff Meeting - Exam Review", Content = "Urgent alignment session for all secondary wing faculty in conference hall today after classes.", TargetAudience = "Teachers", PublishedDate = DateTime.UtcNow.AddHours(-3), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-3), CreatedBy = "admin" },
                    new() { Title = "Library Closed for Auditing", Content = "The central library will remain closed for book inventory check and physical audits on Saturday.", TargetAudience = "All", PublishedDate = DateTime.UtcNow.AddDays(-4), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-4), CreatedBy = "admin" },
                    new() { Title = "Entrance Test Results Term 2", Content = "entrance exam results for Grade 1 admissions are declared. View lists on the notification board.", TargetAudience = "Public", PublishedDate = DateTime.UtcNow.AddDays(-8), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-8), CreatedBy = "admin" },
                    new() { Title = "Flu Vaccination Drive on Campus", Content = "A local health unit is organizing voluntary influenza vaccination on Wednesday for student wellness.", TargetAudience = "All", PublishedDate = DateTime.UtcNow.AddDays(-6), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-6), CreatedBy = "admin" },
                    new() { Title = "Math Olympiad Syllabus Dispatch", Content = "Olympiad guidelines and sample papers are updated on student dashboards. Best of luck!", TargetAudience = "All", PublishedDate = DateTime.UtcNow.AddDays(-7), IsPinned = false, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-7), CreatedBy = "admin" }
                };
                context.NoticeBoards.AddRange(notices);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 8. SEED CIRCULARS (10+ Circulars)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.Circulars.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var circulars = new List<Circular>
                {
                    new() { CircularNo = "CIR/2026/012", Subject = "Uniform Guidelines for Midterm Examinations", Content = "All students must strictly adhere to the official school uniform dress code. Identity cards must be worn visibly at all times during the examination week.", TargetAudience = "All", PublishDate = DateTime.UtcNow.AddDays(-3), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-3), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/001", Subject = "Mobile Phone Policy inside Classrooms", Content = "Usage of mobile phones by students inside classrooms is strictly prohibited. Confiscated phones will only be returned to parents.", TargetAudience = "Parents", PublishDate = DateTime.UtcNow.AddDays(-15), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-15), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/002", Subject = "Secondary Syllabus Revision circular", Content = "Teachers are requested to compile weekly syllabus reports and register on ERP boards before Friday.", TargetAudience = "Teachers", PublishDate = DateTime.UtcNow.AddDays(-12), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-12), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/003", Subject = "School Timings change for summer session", Content = "Timings changed: 7:45 AM pick up to 1:30 PM drop. Bus routes timings adjusted by 15 mins.", TargetAudience = "All", PublishDate = DateTime.UtcNow.AddDays(-10), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-10), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/004", Subject = "Quarter 1 Fee Payment Deadline reminder", Content = " tuition fees invoice for Q1 must be settled before due date to avoid fine penalties.", TargetAudience = "Parents", PublishDate = DateTime.UtcNow.AddDays(-8), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-8), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/005", Subject = "Safety standards checks for labs", Content = "Biology, physics and chemistry lab assistants verify fire protection tools checklist.", TargetAudience = "Employees", PublishDate = DateTime.UtcNow.AddDays(-9), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-9), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/006", Subject = "Mid-Term assessment grading systems", Content = "Grades must be recorded on school ERP within 5 working days of test completion.", TargetAudience = "Teachers", PublishDate = DateTime.UtcNow.AddDays(-7), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-7), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/007", Subject = "Canteen Health inspection checks", Content = "Monthly food grade quality test is scheduled for Tuesday afternoon by district board.", TargetAudience = "Employees", PublishDate = DateTime.UtcNow.AddDays(-6), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-6), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/008", Subject = "Winter Sports coaching registrations", Content = "After-school cricket and athletic camps registration open for classes 6 to 9.", TargetAudience = "All", PublishDate = DateTime.UtcNow.AddDays(-5), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-5), CreatedBy = "admin" },
                    new() { CircularNo = "CIR/2026/009", Subject = "Discipline Code Guidelines Update", Content = "Please review the updated student discipline guidelines document on parent portal.", TargetAudience = "Parents", PublishDate = DateTime.UtcNow.AddDays(-4), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-4), CreatedBy = "admin" }
                };
                context.Circulars.AddRange(circulars);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 9. SEED SMS LOGS (10+ SMS Logs)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.SmsLogs.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var smsLogs = new List<SmsLog>
                {
                    new() { RecipientNo = "+919876543210", Message = "Dear Parent, your ward Student User was marked ABSENT today. Please check portal.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddHours(-6), ProviderResponse = "MSG-ID-01824", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-6), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543211", Message = "Fee Reminder: Term 1 fee submission extended to July 25th. Pay online.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-1), ProviderResponse = "MSG-ID-01742", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-1), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543212", Message = "ERP Alert: One-Time Password for login is 429810. Valid for 5 minutes.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddMinutes(-15), ProviderResponse = "MSG-ID-01991", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddMinutes(-15), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543213", Message = "PTA Meeting Reminder: PTA Review session is scheduled tomorrow 10:00 AM.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddHours(-18), ProviderResponse = "MSG-ID-01611", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-18), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543214", Message = "Library overdue: Please return Physics HC Verma book to library desk.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-2), ProviderResponse = "MSG-ID-01511", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-2), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543215", Message = "Report card Alert: Term 1 reports published. Login to parent portal.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-3), ProviderResponse = "MSG-ID-01411", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-3), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543216", Message = "Transport: Bus route 3 is delayed by 10 mins due to minor repair.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-4), ProviderResponse = "MSG-ID-01311", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-4), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543217", Message = "Holiday Alert: School closed on Monday for Independence Day.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-5), ProviderResponse = "MSG-ID-01211", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-5), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543218", Message = "Admission Status: Your application is approved. Complete fee payments.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-6), ProviderResponse = "MSG-ID-01111", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-6), CreatedBy = "system" },
                    new() { RecipientNo = "+919876543219", Message = "Salary Credit: Pay slip generated for June. Check employee portal.", SentStatus = "Sent", SentDate = DateTime.UtcNow.AddDays(-7), ProviderResponse = "MSG-ID-01011", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-7), CreatedBy = "system" }
                };
                context.SmsLogs.AddRange(smsLogs);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 10. SEED WHATSAPP LOGS (10+ WhatsApp Logs)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.WhatsAppLogs.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var waLogs = new List<WhatsAppLog>
                {
                    new() { RecipientPhone = "+919876543210", Message = "🔔 *School Update:* School bus Route 5 is delayed by 15 mins due to traffic. Track live status on portal.", Status = "Delivered", SentDate = DateTime.UtcNow.AddHours(-2), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddHours(-2), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543211", Message = "🔔 *Fee Alert:* Outstanding Term 1 Tuition fee invoice generated. Click to pay: https://schoolsaas.com/fees", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-1), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-1), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543212", Message = "🔔 *Assessment:* Grade card for midterm exams published. Download via parent desk.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-2), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-2), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543213", Message = "🔔 *Attendance:* Ward marked absent today. Inform class teacher if on leave.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-3), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-3), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543214", Message = "🔔 *Holiday:* School will remain closed on Friday for Teacher Day workshops.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-4), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-4), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543215", Message = "🔔 *Admission:* Registration successful. Entrace schedule details updated.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-5), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-5), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543216", Message = "🔔 *Notice:* Science Exhibition program scheduled for next Thursday.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-6), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-6), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543217", Message = "🔔 *Leave Approved:* Your personal leave request was verified by principal.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-7), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-7), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543218", Message = "🔔 *Meeting:* Weekly online PTA feedback session starting in 1 hour.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-8), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-8), CreatedBy = "system" },
                    new() { RecipientPhone = "+919876543219", Message = "🔔 *Activity:* Basketball tournament schedule declared. Register today.", Status = "Delivered", SentDate = DateTime.UtcNow.AddDays(-9), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow.AddDays(-9), CreatedBy = "system" }
                };
                context.WhatsAppLogs.AddRange(waLogs);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 11. SEED CENTRAL NOTIFICATIONS (10+ Notifications)
            // ════════════════════════════════════════════════════════════════════
            if (!await context.CentralNotifications.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var notifications = new List<CentralNotification>
                {
                    new() { RecipientUserId = Constants.StudentUser, Title = "Syllabus Update: Physics Term 1", Body = "A new topic 'Wave Optics' has been appended to the midterm schedule.", Category = "Academic", Priority = "Medium", ActionUrl = "/student/my-academic", IsRead = false, SentDate = DateTime.UtcNow.AddMinutes(-30), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.StudentUser, Title = "Urgent: Fee Receipt Available", Body = "Payment receipt for Term 1 tuition fee has been generated successfully.", Category = "Fee", Priority = "High", ActionUrl = "/student/my-receipts", IsRead = true, SentDate = DateTime.UtcNow.AddDays(-1), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.AdminUser, Title = "New Support Ticket Raised", Body = "Ticket TKT-2026-0001 requires assignation.", Category = "Support", Priority = "High", ActionUrl = "/communication", IsRead = false, SentDate = DateTime.UtcNow.AddHours(-1), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.StudentUser, Title = "Bus Route 5 Delayed", Body = "Route 5 bus pick up delayed by 15 mins due to city traffic roadblocks.", Category = "Transport", Priority = "High", ActionUrl = "/student/my-transport", IsRead = false, SentDate = DateTime.UtcNow.AddMinutes(-10), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.StudentUser, Title = "Homework Assigned: Chemistry Ch 3", Body = "Chemistry worksheet for Organic chemistry posted. Submit before Friday.", Category = "Academic", Priority = "Medium", ActionUrl = "/student/my-assignments", IsRead = false, SentDate = DateTime.UtcNow.AddHours(-2), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.StudentUser, Title = "Library Overdue Book Reminder", Body = "Please return Chemistry part 1 reference book to library counter immediately.", Category = "Academic", Priority = "Low", ActionUrl = "/student/my-library", IsRead = false, SentDate = DateTime.UtcNow.AddDays(-2), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.AdminUser, Title = "Leave Application Submitted", Body = "Teacher Sarah has applied for 3 days medical leave. Approval required.", Category = "HR", Priority = "High", ActionUrl = "/hr/leaves", IsRead = false, SentDate = DateTime.UtcNow.AddHours(-3), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.StudentUser, Title = "Grade Card Published", Body = "Biology Midterm Grade cards are declared. Download from report section.", Category = "Exam", Priority = "High", ActionUrl = "/student/my-grades", IsRead = true, SentDate = DateTime.UtcNow.AddDays(-3), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.StudentUser, Title = "Hostel Room Change Approved", Body = "Your room change application has been verified. Shift to Room 210.", Category = "Hostel", Priority = "Medium", ActionUrl = "/student/my-hostel", IsRead = false, SentDate = DateTime.UtcNow.AddDays(-4), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" },
                    new() { RecipientUserId = Constants.AdminUser, Title = "System Performance Upgrade Complete", Body = "Database index compression schedules finished. Processing speed optimal.", Category = "Notice", Priority = "Low", ActionUrl = "/communication", IsRead = true, SentDate = DateTime.UtcNow.AddDays(-5), SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "system" }
                };
                context.CentralNotifications.AddRange(notifications);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════════
            // 12. SEED CHATS & CHANNELS
            // ════════════════════════════════════════════════════════════════════
            if (!await context.GroupChatRooms.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var room = new GroupChatRoom
                {
                    Name = "Science Department Channel",
                    Type = "Department",
                    TargetReferenceId = "Science",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                };
                context.GroupChatRooms.Add(room);
                await context.SaveChangesAsync();

                var members = new List<GroupChatMember>
                {
                    new() { RoomId = room.Id, UserId = Constants.AdminUser, IsAdmin = true, SchoolRegistrationId = schoolId, JoinedAt = DateTime.UtcNow },
                    new() { RoomId = room.Id, UserId = Constants.EmployeeUser, IsAdmin = false, SchoolRegistrationId = schoolId, JoinedAt = DateTime.UtcNow }
                };
                context.GroupChatMembers.AddRange(members);
                await context.SaveChangesAsync();

                var message = new GroupChatMessage
                {
                    RoomId = room.Id,
                    SenderUserId = Constants.AdminUser,
                    MessageContent = "Welcome to the Science Department Chat channel! Please share your lesson plan drafts here.",
                    SentTime = DateTime.UtcNow.AddHours(-2),
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddHours(-2),
                    CreatedBy = "admin"
                };
                context.GroupChatMessages.Add(message);
                await context.SaveChangesAsync();
            }
        }
    }
}
