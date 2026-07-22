using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.Academic;

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
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 1, ChapterName = "Real Numbers", Description = "Fundamental Theorem of Arithmetic, Rational & Irrational numbers representations.", TotalPeriods = 8, CompletedPeriods = 8, Status = "Completed", StartedDate = DateTime.UtcNow.AddDays(-38), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 2, ChapterName = "Polynomials", Description = "Geometrical meaning of zeroes, algebraic relationships of quadratic expressions.", TotalPeriods = 10, CompletedPeriods = 10, Status = "Completed", StartedDate = DateTime.UtcNow.AddDays(-36), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 3, ChapterName = "Pair of Linear Equations in Two Variables", Description = "Graphical method of solution, algebraic methods.", TotalPeriods = 12, CompletedPeriods = 12, Status = "Completed", StartedDate = DateTime.UtcNow.AddDays(-34), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 4, ChapterName = "Quadratic Equations", Description = "Solutions by factorisation and completing the square.", TotalPeriods = 14, CompletedPeriods = 10, Status = "InProgress", StartedDate = DateTime.UtcNow.AddDays(-32), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 5, ChapterName = "Arithmetic Progressions", Description = "nth term and sum of first n terms.", TotalPeriods = 8, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-30), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 6, ChapterName = "Triangles", Description = "Similar figures, similarity of triangles, Pythagoras Theorem.", TotalPeriods = 15, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-28), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 7, ChapterName = "Coordinate Geometry", Description = "Distance formula, Section formula, Area of a triangle.", TotalPeriods = 10, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-26), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 8, ChapterName = "Introduction to Trigonometry", Description = "Trigonometric ratios, values, identities.", TotalPeriods = 12, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-24), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 9, ChapterName = "Some Applications of Trigonometry", Description = "Heights and distances.", TotalPeriods = 8, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-22), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 10, ChapterName = "Circles", Description = "Tangent to a circle, number of tangents from a point on a circle.", TotalPeriods = 10, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-20), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 11, ChapterName = "Constructions", Description = "Division of a line segment, construction of tangents.", TotalPeriods = 8, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-18), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 12, ChapterName = "Areas Related to Circles", Description = "Perimeter and area of a circle, areas of sector and segment.", TotalPeriods = 10, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-16), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 13, ChapterName = "Surface Areas and Volumes", Description = "Surface areas and volumes of combinations of solids.", TotalPeriods = 15, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-14), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 14, ChapterName = "Statistics", Description = "Mean, median and mode of grouped data, cumulative frequency graph.", TotalPeriods = 12, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-12), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SubjectId = sub.Id, ClassId = cls.Id, ChapterNo = 15, ChapterName = "Probability", Description = "Classical definition of probability, simple problems.", TotalPeriods = 8, CompletedPeriods = 0, Status = "NotStarted", StartedDate = DateTime.UtcNow.AddDays(-10), SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.SyllabusChapters.AddRangeAsync(chapters);
                await context.SaveChangesAsync();

                var lessonPlans = new List<LessonPlan>
                {
                    new() { SyllabusChapterId = chapters[0].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-29), Topic = "Euclid's Division Lemma", Objectives = "Configure understanding of HCF calculation methodologies.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[0].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-28), Topic = "Fundamental Theorem of Arithmetic", Objectives = "Prime factorizations logic & composite numbers structures.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[0].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-27), Topic = "Revisiting Irrational Numbers", Objectives = "Proofs of irrationality of root 2, 3, 5.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[0].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-26), Topic = "Revisiting Rational Numbers", Objectives = "Decimal expansions of rational numbers.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[1].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-25), Topic = "Geometrical Meaning of Zeroes", Objectives = "Intersection of graphs with x-axis.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[1].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-24), Topic = "Relationship between Zeroes and Coefficients", Objectives = "Sum and product of roots.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[1].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-23), Topic = "Division Algorithm for Polynomials", Objectives = "Long division method for polynomials.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[2].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-22), Topic = "Graphical Method of Solution", Objectives = "Consistent and inconsistent systems.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[2].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-21), Topic = "Substitution Method", Objectives = "Solving linear equations by substitution.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[2].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-20), Topic = "Elimination Method", Objectives = "Solving linear equations by equating coefficients.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[2].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-19), Topic = "Cross-Multiplication Method", Objectives = "Alternative algebraic method.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[3].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-18), Topic = "Solution by Factorisation", Objectives = "Splitting the middle term.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[3].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-17), Topic = "Solution by Completing the Square", Objectives = "Making perfect squares.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[3].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-16), Topic = "Nature of Roots", Objectives = "Discriminant and root types.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { SyllabusChapterId = chapters[4].Id, SubjectId = sub.Id, ClassId = cls.Id, PlannedDate = DateTime.Today.AddDays(-15), Topic = "nth Term of an AP", Objectives = "Finding specific terms in a sequence.", TeachingMethod = "Classroom Lecture", MaterialsRequired = "Textbook, worksheets", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.LessonPlans.AddRangeAsync(lessonPlans);
                await context.SaveChangesAsync();
            }

            // 3. Homework & Submissions Seeds
            if (!await context.Homeworks.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var hws = new List<Homework>
                {
                    new() { Title = "Exercise 1.1: Euclid's Division Lemma", Description = "Complete problems 1 to 5 from page 7.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-20), DueDate = DateTime.Today.AddDays(-18), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 1.2: Fundamental Theorem of Arithmetic", Description = "Complete all problems on prime factors from page 11.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-19), DueDate = DateTime.Today.AddDays(-17), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 1.3: Irrational Numbers", Description = "Prove root 2, 3, 5 are irrational.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-18), DueDate = DateTime.Today.AddDays(-16), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 2.1: Polynomial Zeroes", Description = "Find zeroes of the given graphs.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-17), DueDate = DateTime.Today.AddDays(-15), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 2.2: Zeroes and Coefficients", Description = "Verify relationship between zeroes and coefficients.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-16), DueDate = DateTime.Today.AddDays(-14), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 2.3: Division Algorithm", Description = "Divide the polynomial p(x) by polynomial g(x).", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-15), DueDate = DateTime.Today.AddDays(-13), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 3.1: Graphical Methods", Description = "Represent situations algebraically and graphically.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-14), DueDate = DateTime.Today.AddDays(-12), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 3.2: Consistency", Description = "Check consistency of linear equations.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-13), DueDate = DateTime.Today.AddDays(-11), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 3.3: Substitution Method", Description = "Solve equations by substitution method.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-12), DueDate = DateTime.Today.AddDays(-10), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Exercise 3.4: Elimination Method", Description = "Solve equations by elimination method.", SubjectId = sub.Id, ClassId = cls.Id, AssignedDate = DateTime.Today.AddDays(-11), DueDate = DateTime.Today.AddDays(-9), AssignedByUserId = "seed", AssignedByName = "Teacher Admin", Status = "Active", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.Homeworks.AddRangeAsync(hws);
                await context.SaveChangesAsync();

                var homeworkSubmissions = new List<HomeworkSubmission>
                {
                    new() { HomeworkId = hws[0].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-18), FilePath = "/uploads/homework/hw_0_student.pdf", StudentRemarks = "Attached scanned pages", Status = "Submitted", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { HomeworkId = hws[1].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-17), FilePath = "/uploads/homework/hw_1_student.pdf", StudentRemarks = "Attached scanned pages", Status = "Submitted", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { HomeworkId = hws[2].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-16), FilePath = "/uploads/homework/hw_2_student.pdf", StudentRemarks = "Attached scanned pages", Status = "Submitted", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { HomeworkId = hws[3].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-15), FilePath = "/uploads/homework/hw_3_student.pdf", StudentRemarks = "Attached scanned pages", Status = "Submitted", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { HomeworkId = hws[4].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-14), FilePath = "/uploads/homework/hw_4_student.pdf", StudentRemarks = "Attached scanned pages", Status = "Submitted", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.HomeworkSubmissions.AddRangeAsync(homeworkSubmissions);
                await context.SaveChangesAsync();
            }

            // 4. Graded Assignments & Submissions Seeds
            if (!await context.Assignments.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var assigns = new List<Assignment>
                {
                    new() { Title = "Unit Test 1: Real Numbers", Instructions = "Formal graded assessment for Chapter 1.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-30), EndDate = DateTime.Today.AddDays(-25), MaxMarks = 25.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Unit Test 2: Polynomials", Instructions = "Formal graded assessment for Chapter 2.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-28), EndDate = DateTime.Today.AddDays(-23), MaxMarks = 25.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Unit Test 3: Linear Equations", Instructions = "Formal graded assessment for Chapter 3.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-26), EndDate = DateTime.Today.AddDays(-21), MaxMarks = 25.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Unit Test 4: Quadratic Equations", Instructions = "Formal graded assessment for Chapter 4.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-24), EndDate = DateTime.Today.AddDays(-19), MaxMarks = 25.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Unit Test 5: Arithmetic Progressions", Instructions = "Formal graded assessment for Chapter 5.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-22), EndDate = DateTime.Today.AddDays(-17), MaxMarks = 25.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Mid Term Project: Application of Trigonometry", Instructions = "Model creation and calculations.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-20), EndDate = DateTime.Today.AddDays(-15), MaxMarks = 50.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Class Test: Triangles", Instructions = "MCQ based class test.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-18), EndDate = DateTime.Today.AddDays(-13), MaxMarks = 20.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Class Test: Coordinate Geometry", Instructions = "MCQ based class test.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-16), EndDate = DateTime.Today.AddDays(-11), MaxMarks = 20.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Practical: Construction", Instructions = "Geometry construction practical record.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-14), EndDate = DateTime.Today.AddDays(-9), MaxMarks = 30.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Final Assessment Mock: Full Syllabus", Instructions = "Full length mock test.", SubjectId = sub.Id, ClassId = cls.Id, StartDate = DateTime.Today.AddDays(-12), EndDate = DateTime.Today.AddDays(-7), MaxMarks = 80.00m, CreatedByUserId = "seed", Status = "Published", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.Assignments.AddRangeAsync(assigns);
                await context.SaveChangesAsync();

                var assignmentSubmissions = new List<AssignmentSubmission>
                {
                    new() { AssignmentId = assigns[0].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-25), FilePath = "/uploads/submissions/assign_0_stu.pdf", StudentRemarks = "Completed", Status = "Graded", MarksObtained = 20.50m, TeacherFeedback = "Good work!", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { AssignmentId = assigns[1].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-23), FilePath = "/uploads/submissions/assign_1_stu.pdf", StudentRemarks = "Completed", Status = "Graded", MarksObtained = 19.50m, TeacherFeedback = "Good work!", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { AssignmentId = assigns[2].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-21), FilePath = "/uploads/submissions/assign_2_stu.pdf", StudentRemarks = "Completed", Status = "Graded", MarksObtained = 18.50m, TeacherFeedback = "Good work!", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { AssignmentId = assigns[3].Id, StudentId = student.Id, SubmittedDate = DateTime.Now.AddDays(-19), FilePath = "/uploads/submissions/assign_3_stu.pdf", StudentRemarks = "Completed", Status = "Graded", MarksObtained = 17.50m, TeacherFeedback = "Good work!", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
                };
                await context.AssignmentSubmissions.AddRangeAsync(assignmentSubmissions);
                await context.SaveChangesAsync();
            }

            // 5. Online Classes / Live Sessions Seeds
            if (!await context.OnlineClasses.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var sessions = new List<OnlineClass>
                {
                    new() { Title = "Doubt Clearing: Real Numbers", Description = "Addressing queries on Chapter 1.", SubjectId = sub.Id, ClassId = cls.Id, ScheduledAt = DateTime.Now.AddDays(-10), DurationMinutes = 45, Platform = "Google Meet", MeetingLink = "https://meet.google.com/xyz-qwe-rty", MeetingId = "xyz-qwe-rty", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Revision: Polynomials", Description = "Quick recap of zeroes and coefficients.", SubjectId = sub.Id, ClassId = cls.Id, ScheduledAt = DateTime.Now.AddDays(-9), DurationMinutes = 45, Platform = "Google Meet", MeetingLink = "https://meet.google.com/xyz-qwe-rty", MeetingId = "xyz-qwe-rty", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Live Quiz: Linear Equations", Description = "Interactive quiz session.", SubjectId = sub.Id, ClassId = cls.Id, ScheduledAt = DateTime.Now.AddDays(-8), DurationMinutes = 45, Platform = "Google Meet", MeetingLink = "https://meet.google.com/xyz-qwe-rty", MeetingId = "xyz-qwe-rty", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Interactive Session: Trigonometry Identities", Description = "Tricks to remember identities.", SubjectId = sub.Id, ClassId = cls.Id, ScheduledAt = DateTime.Now.AddDays(-7), DurationMinutes = 45, Platform = "Google Meet", MeetingLink = "https://meet.google.com/xyz-qwe-rty", MeetingId = "xyz-qwe-rty", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow },
                    new() { Title = "Final Exam Strategy", Description = "Tips and tricks for the final exam.", SubjectId = sub.Id, ClassId = cls.Id, ScheduledAt = DateTime.Now.AddDays(-6), DurationMinutes = 45, Platform = "Google Meet", MeetingLink = "https://meet.google.com/xyz-qwe-rty", MeetingId = "xyz-qwe-rty", Status = "Completed", SchoolRegistrationId = schoolId, CreatedBy = "seed", CreatedDate = DateTime.UtcNow }
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
                            Instructions = "Calculators are strictly prohibited.",
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

            // --- Additional Academic Models Seeding ---
            var academicYear = await context.AcademicYears.FirstOrDefaultAsync(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId);

            // 7. Education Level
            if (!await context.EducationLevels.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var edLevels = new List<EducationLevel>
                {
                    new() { Name = "Primary Education", Code = "PR", Description = "Grades 1 to 5", SchoolRegistrationId = schoolId },
                    new() { Name = "Secondary Education", Code = "SEC", Description = "Grades 6 to 10", SchoolRegistrationId = schoolId },
                    new() { Name = "Higher Secondary", Code = "HSEC", Description = "Grades 11 to 12", SchoolRegistrationId = schoolId }
                };
                await context.EducationLevels.AddRangeAsync(edLevels);
                await context.SaveChangesAsync();
            }

            var edLevel = await context.EducationLevels.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId);

            // 8. Program
            if (edLevel != null && !await context.Programs.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var programs = new List<Program>
                {
                    new() { Name = "Science Stream", Code = "SCI", EducationLevelId = edLevel.Id, DurationYears = 2, SchoolRegistrationId = schoolId },
                    new() { Name = "Commerce Stream", Code = "COM", EducationLevelId = edLevel.Id, DurationYears = 2, SchoolRegistrationId = schoolId },
                    new() { Name = "Arts & Humanities", Code = "ART", EducationLevelId = edLevel.Id, DurationYears = 2, SchoolRegistrationId = schoolId }
                };
                await context.Programs.AddRangeAsync(programs);
                await context.SaveChangesAsync();
            }

            var program = await context.Programs.FirstOrDefaultAsync(x => x.SchoolRegistrationId == schoolId);

            // 9. Batch & Branch
            if (program != null && academicYear != null)
            {
                if (!await context.Batches.AnyAsync(x => x.SchoolRegistrationId == schoolId))
                {
                    var batches = new List<Batch>
                    {
                        new() { Name = "Batch 2024-2026", Code = "B24", StartDate = DateTime.Today.AddDays(-100), EndDate = DateTime.Today.AddDays(600), AcademicYearId = academicYear.Id, ProgramId = program.Id, SchoolRegistrationId = schoolId },
                        new() { Name = "Batch 2025-2027", Code = "B25", StartDate = DateTime.Today.AddDays(200), EndDate = DateTime.Today.AddDays(900), AcademicYearId = academicYear.Id, ProgramId = program.Id, SchoolRegistrationId = schoolId }
                    };
                    await context.Batches.AddRangeAsync(batches);
                    await context.SaveChangesAsync();
                }

                if (!await context.Branches.AnyAsync(x => x.SchoolRegistrationId == schoolId))
                {
                    var branches = new List<Branch>
                    {
                        new() { Name = "Main Campus", Code = "MAIN", ProgramId = program.Id, SchoolRegistrationId = schoolId },
                        new() { Name = "North Wing", Code = "NW", ProgramId = program.Id, SchoolRegistrationId = schoolId }
                    };
                    await context.Branches.AddRangeAsync(branches);
                    await context.SaveChangesAsync();
                }
            }

            // 10. Year Semester
            if (!await context.YearSemesters.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var sems = new List<YearSemester>
                {
                    new() { Name = "Semester 1", Code = "SEM1", Sequence = 1, SchoolRegistrationId = schoolId },
                    new() { Name = "Semester 2", Code = "SEM2", Sequence = 2, SchoolRegistrationId = schoolId }
                };
                await context.YearSemesters.AddRangeAsync(sems);
                await context.SaveChangesAsync();
            }

            // 11. Timetable Period
            if (academicYear != null && !await context.TimetablePeriods.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var periods = new List<TimetablePeriod>();
                for (int d = 1; d <= 5; d++) // Monday to Friday
                {
                    for (int p = 1; p <= 6; p++) // 6 periods a day
                    {
                        periods.Add(new TimetablePeriod
                        {
                            ClassId = cls.Id,
                            SubjectId = sub.Id,
                            DayOfWeek = d,
                            PeriodNo = p,
                            StartTime = $"{8 + p}:00",
                            EndTime = $"{8 + p}:45",
                            RoomNo = "101",
                            AcademicYearId = academicYear.Id,
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "seed",
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }
                await context.TimetablePeriods.AddRangeAsync(periods);
                await context.SaveChangesAsync();
            }

            // 12. Exam Results
            if (exam != null && !await context.ExamResults.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var results = new List<ExamResult>();
                for (int i = 1; i <= 25; i++)
                {
                    results.Add(new ExamResult
                    {
                        ExamId = exam.Id,
                        StudentId = student.Id,
                        SubjectId = sub.Id,
                        MarksObtained = 40 + i,
                        TotalMarks = 100,
                        Grade = "B",
                        Status = "Pass",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = "seed",
                        CreatedDate = DateTime.UtcNow
                    });
                }
                await context.ExamResults.AddRangeAsync(results);
                await context.SaveChangesAsync();
            }

            // 13. Subject Enrollments
            if (academicYear != null && !await context.SubjectEnrollments.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var enrollments = new List<SubjectEnrollment>();
                for (int i = 1; i <= 25; i++)
                {
                    enrollments.Add(new SubjectEnrollment
                    {
                        StudentId = student.Id,
                        SubjectId = sub.Id,
                        YearSemesterId = academicYear.Id,
                        ClassId = cls.Id,
                        EnrolledDate = DateTime.UtcNow.AddDays(-50),
                        Status = "Enrolled",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = "seed",
                        CreatedDate = DateTime.UtcNow
                    });
                }
                await context.SubjectEnrollments.AddRangeAsync(enrollments);
                await context.SaveChangesAsync();
            }

            // 14. Student Attendance
            if (academicYear != null && !await context.StudentAttendances.AnyAsync(x => x.SchoolRegistrationId == schoolId))
            {
                var attendances = new List<StudentAttendance>();
                for (int i = 1; i <= 30; i++)
                {
                    attendances.Add(new StudentAttendance
                    {
                        StudentId = student.Id,
                        ClassId = cls.Id,
                        AttendanceDate = DateTime.Today.AddDays(-i),
                        Status = i % 10 == 0 ? "Absent" : "Present",
                        Remarks = "",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = "seed",
                        CreatedDate = DateTime.UtcNow
                    });
                }
                await context.StudentAttendances.AddRangeAsync(attendances);
                await context.SaveChangesAsync();
            }

        }
    }
}

