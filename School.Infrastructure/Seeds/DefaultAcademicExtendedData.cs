using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Academic;
using School.Domain.School;

namespace School.Infrastructure.Seeds
{
    public static class DefaultAcademicExtendedData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;
            int schoolId = school.Id;

            // Resolve dependecies
            var cls = await context.Classes.FirstOrDefaultAsync(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId);
            if (cls == null) return;

            var sub = await context.Subjects.FirstOrDefaultAsync(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId);
            if (sub == null) return;

            var student = await context.Students.FirstOrDefaultAsync(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId);
            if (student == null) return;

            // 1. GradeConfig Default Rules Seeds
            if (!await context.GradeConfigs.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var grades = new List<GradeConfig>
                {
                    new() { GradeName = "A+", MinPercent = 90.00m, MaxPercent = 100.00m, GradePoint = 10.00m, Remark = "Outstanding", IsPass = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GradeName = "A",  MinPercent = 80.00m, MaxPercent = 89.99m, GradePoint = 9.00m,  Remark = "Excellent",   IsPass = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GradeName = "B+", MinPercent = 70.00m, MaxPercent = 79.99m, GradePoint = 8.00m,  Remark = "Very Good",   IsPass = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GradeName = "B",  MinPercent = 60.00m, MaxPercent = 69.99m, GradePoint = 7.00m,  Remark = "Good",        IsPass = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GradeName = "C",  MinPercent = 50.00m, MaxPercent = 59.99m, GradePoint = 6.00m,  Remark = "Satisfactory",IsPass = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GradeName = "D",  MinPercent = 33.00m, MaxPercent = 49.99m, GradePoint = 4.00m,  Remark = "Pass",        IsPass = true, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { GradeName = "F",  MinPercent = 0.00m,  MaxPercent = 32.99m, GradePoint = 0.00m,  Remark = "Fail",        IsPass = false, SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.GradeConfigs.AddRangeAsync(grades);
                await context.SaveChangesAsync();
            }

            // 2. Syllabus Chapters Seeds
            if (!await context.SyllabusChapters.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var chapters = new List<SyllabusChapter>
                {
                    new()
                    {
                        SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 1,
                        ChapterName = "Real Numbers & Quantitative Aptitude",
                        Description = "Fundamental Theorem of Arithmetic, Rational & Irrational numbers representations.",
                        TotalPeriods = 8, CompletedPeriods = 8, Status = "Completed",
                        StartedDate = DateTime.UtcNow.AddDays(-20), CompletedDate = DateTime.UtcNow.AddDays(-12),
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 2,
                        ChapterName = "Polynomials & Linear Equations",
                        Description = "Geometrical meaning of zeroes, algebraic relationships of quadratic expressions.",
                        TotalPeriods = 10, CompletedPeriods = 4, Status = "InProgress",
                        StartedDate = DateTime.UtcNow.AddDays(-10),
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 3,
                        ChapterName = "Introduction to Trigonometric Ratios",
                        Description = "Trigonometric values for specific angles, standard identities configurations.",
                        TotalPeriods = 12, CompletedPeriods = 0, Status = "NotStarted",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    }
                };
                await context.SyllabusChapters.AddRangeAsync(chapters);
                await context.SaveChangesAsync();

                var firstChapter = chapters[0];
                var lessonPlans = new List<LessonPlan>
                {
                    new()
                    {
                        SyllabusChapterId = firstChapter.Id, SubjectId = sub.Id, ClassId = cls.Id,
                        PlannedDate = DateTime.Today.AddDays(-18), Topic = "Euclid's Division Lemma",
                        Objectives = "Configure understanding of HCF calculation methodologies.",
                        TeachingMethod = "Classroom Lecture with Blackboard demonstration.",
                        MaterialsRequired = "Standard textbook, worksheets.", Status = "Completed",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        SyllabusChapterId = firstChapter.Id, SubjectId = sub.Id, ClassId = cls.Id,
                        PlannedDate = DateTime.Today.AddDays(-15), Topic = "Fundamental Theorem of Arithmetic",
                        Objectives = "Prime factorizations logic & composite numbers structures.",
                        TeachingMethod = "Interactive equations solver session.",
                        MaterialsRequired = "Homework sheets", Status = "Completed",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    }
                };
                await context.LessonPlans.AddRangeAsync(lessonPlans);
                await context.SaveChangesAsync();
            }

            // 3. Homework & Submissions Seeds
            if (!await context.Homeworks.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var hws = new List<Homework>
                {
                    new()
                    {
                        Title = "Exercise 1.2: Prime Factorizations",
                        Description = "Complete all problems on prime factors and HCF/LCM from textbook page 14.",
                        SubjectId = sub.Id, ClassId = cls.Id,
                        AssignedDate = DateTime.Today.AddDays(-5), DueDate = DateTime.Today.AddDays(-1),
                        AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        Title = "Polynomials Division Algorithm",
                        Description = "Solve worksheet problems on dividing cubic polynomials and finding remaining roots.",
                        SubjectId = sub.Id, ClassId = cls.Id,
                        AssignedDate = DateTime.Today.AddDays(-2), DueDate = DateTime.Today.AddDays(3),
                        AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    }
                };
                await context.Homeworks.AddRangeAsync(hws);
                await context.SaveChangesAsync();

                var hw1 = hws[0];
                var homeworkSubmission = new HomeworkSubmission
                {
                    HomeworkId = hw1.Id, StudentId = student.Id,
                    SubmittedDate = DateTime.Now.AddDays(-2), FilePath = "/uploads/homework/prime_factors_student.pdf",
                    StudentRemarks = "Attached my scanned pages for review", Status = "Submitted",
                    SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                };
                await context.HomeworkSubmissions.AddAsync(homeworkSubmission);
                await context.SaveChangesAsync();
            }

            // 4. Graded Assignments & Submissions Seeds
            if (!await context.Assignments.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var assigns = new List<Assignment>
                {
                    new()
                    {
                        Title = "Periodic Assessment 1: Real Numbers",
                        Instructions = "Formal graded classroom assessment. Scan and upload sheets within the due window.",
                        SubjectId = sub.Id, ClassId = cls.Id,
                        StartDate = DateTime.Today.AddDays(-6), EndDate = DateTime.Today.AddDays(-2),
                        MaxMarks = 25.00m, AttachmentPath = "/uploads/assignments/pa1_maths.pdf",
                        CreatedByUserId = "seed", Status = "Published",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        Title = "Group Project: Polynomials Applications",
                        Instructions = "Illustrate real world usage of parabolas and projectile curves.",
                        SubjectId = sub.Id, ClassId = cls.Id,
                        StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(10),
                        MaxMarks = 50.00m, CreatedByUserId = "seed", Status = "Published",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    }
                };
                await context.Assignments.AddRangeAsync(assigns);
                await context.SaveChangesAsync();

                var assign1 = assigns[0];
                var assignmentSubmission = new AssignmentSubmission
                {
                    AssignmentId = assign1.Id, StudentId = student.Id,
                    SubmittedDate = DateTime.Now.AddDays(-3), FilePath = "/uploads/submissions/pa1_response_stu.pdf",
                    StudentRemarks = "Completed all reactions equations.", Status = "Graded",
                    MarksObtained = 22.50m, TeacherFeedback = "Excellent mathematical steps. Good work!",
                    SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                };
                await context.AssignmentSubmissions.AddAsync(assignmentSubmission);
                await context.SaveChangesAsync();
            }

            // 5. Online Classes / Live Sessions Seeds
            if (!await context.OnlineClasses.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var sessions = new List<OnlineClass>
                {
                    new()
                    {
                        Title = "Polynomials zeroes graphical interpretations",
                        Description = "Understanding quadratic graphs and parabola intersections.",
                        SubjectId = sub.Id, ClassId = cls.Id,
                        ScheduledAt = DateTime.Now.AddHours(4), DurationMinutes = 45,
                        Platform = "Google Meet", MeetingLink = "https://meet.google.com/xyz-qwe-rty",
                        MeetingId = "xyz-qwe-rty", MeetingPassword = "123", Status = "Scheduled",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        Title = "HCF & LCM Core Concepts Refresher",
                        Description = "Archived session covering division lemma practice queries.",
                        SubjectId = sub.Id, ClassId = cls.Id,
                        ScheduledAt = DateTime.Now.AddDays(-3), DurationMinutes = 60,
                        Platform = "Zoom", MeetingLink = "https://zoom.us/j/11223344",
                        MeetingId = "11223344", RecordingLink = "https://zoom.us/rec/archive/maths_hcf",
                        Status = "Completed",
                        SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                    }
                };
                await context.OnlineClasses.AddRangeAsync(sessions);
                await context.SaveChangesAsync();
            }

            // 6. Term Exams Timetables & ReportCards Seeds
            var exam = await context.Exams.FirstOrDefaultAsync(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId);
            if (exam != null)
            {
                if (!await context.ExamSchedules.AnyAsync(x => x.SchoolRegistrationId == schoolId))
                {
                    var examSchedules = new List<ExamSchedule>
                    {
                        new()
                        {
                            ExamId = exam.Id, SubjectId = sub.Id, ClassId = cls.Id,
                            ExamDate = DateTime.Today.AddDays(10), StartTime = "09:30 AM", EndTime = "12:30 PM",
                            MaxMarks = 100.00m, PassingMarks = 33.00m, RoomNo = "Room 304, Block C",
                            Instructions = "Calculators are strictly prohibited. Bring your own geometry kits.",
                            SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                        }
                    };
                    await context.ExamSchedules.AddRangeAsync(examSchedules);
                    await context.SaveChangesAsync();
                }

                if (!await context.ReportCards.AnyAsync(x => x.SchoolRegistrationId == schoolId))
                {
                    var reportCards = new List<ReportCard>
                    {
                        new()
                        {
                            StudentId = student.Id, ExamId = exam.Id, ClassId = cls.Id,
                            TotalMarksObtained = 88.00m, TotalMaxMarks = 100.00m, Percentage = 88.00m,
                            Grade = "A", GradePoint = "9.0", Rank = 3, Status = "Pass",
                            PublishStatus = "Published", PublishedDate = DateTime.Now.AddDays(-2),
                            Remarks = "Extremely focused student. Analytical skills are exemplary.",
                            PdfPath = "/uploads/reportcards/rep_math_exam.pdf",
                            SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow
                        }
                    };
                    await context.ReportCards.AddRangeAsync(reportCards);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
