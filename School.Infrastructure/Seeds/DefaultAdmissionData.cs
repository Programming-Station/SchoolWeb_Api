using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.School;
using School.Domain.Student;
using School.Domain.FeeManagnment;
using School.Domain.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Seeds
{
    public static class DefaultAdmissionData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.FirstOrDefaultAsync();
            if (school == null) return;

            var academicYear = await context.AcademicYears.FirstOrDefaultAsync(ay => ay.IsCurrent) 
                               ?? await context.AcademicYears.FirstOrDefaultAsync();
            if (academicYear == null) return;

            // ─── 1. Seed Campuses (8 records) ──────────────────────────────────────────
            if (!context.Campuses.Any())
            {
                var campuses = new List<Campus>
                {
                    new Campus { Name = "Main Corporate Campus", Code = "MAIN", Email = "main@schoolsaas.com", Phone = "9876543210", Address = "Sector 62, Noida, UP, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "International North Branch", Code = "NORTH", Email = "north@schoolsaas.com", Phone = "9876543211", Address = "GT Road, Jalandhar, Punjab, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "Southern Medical & Paramedical Campus", Code = "SOUTH-MED", Email = "south.med@schoolsaas.com", Phone = "9876543212", Address = "Whitefield, Bangalore, Karnataka, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "City Tech Campus", Code = "TECH-CITY", Email = "tech.city@schoolsaas.com", Phone = "9876543213", Address = "Salt Lake Sector V, Kolkata, WB, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "Westhaven Elite Public Academy", Code = "WEST-ELITE", Email = "west@schoolsaas.com", Phone = "9876543214", Address = "Bandra West, Mumbai, MH, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "Eastern Heritage Campus", Code = "EAST-HERITAGE", Email = "east@schoolsaas.com", Phone = "9876543215", Address = "Dispur, Guwahati, Assam, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "Central Administration Campus", Code = "CENTRAL-ADMIN", Email = "central.admin@schoolsaas.com", Phone = "9876543216", Address = "Connaught Place, New Delhi, India", Status = "active", SchoolRegistrationId = school.Id },
                    new Campus { Name = "Western Technical Institute Branch", Code = "WEST-TECH", Email = "west.tech@schoolsaas.com", Phone = "9876543217", Address = "GIDC Electronics Zone, Gandhinagar, Gujarat, India", Status = "active", SchoolRegistrationId = school.Id }
                };
                await context.Campuses.AddRangeAsync(campuses);
                await context.SaveChangesAsync();
            }

            // ─── 2. Seed Education Levels (7 records) ──────────────────────────────────
            if (!context.EducationLevels.Any())
            {
                var levels = new List<EducationLevel>
                {
                    new EducationLevel { Name = "Primary & Middle School (Nursery - VIII)", Code = "PRIM-MID", Description = "Early childhood to middle school foundational programs", Status = "active", SchoolRegistrationId = school.Id },
                    new EducationLevel { Name = "Secondary & Higher Secondary (IX - XII)", Code = "SEC-HSC", Description = "High school matriculation and science/commerce preparation boards", Status = "active", SchoolRegistrationId = school.Id },
                    new EducationLevel { Name = "Polytechnic & ITI Diplomas", Code = "DIPLOMA-ITI", Description = "Technical, paramedical, ITI trade training and certification tracks", Status = "active", SchoolRegistrationId = school.Id },
                    new EducationLevel { Name = "Undergraduate Degrees (UG)", Code = "UG-BACHELOR", Description = "Bachelor of Science, B.Tech, Nursing GNM, BCA degrees", Status = "active", SchoolRegistrationId = school.Id },
                    new EducationLevel { Name = "Postgraduate Degrees (PG)", Code = "PG-MASTERS", Description = "Masters degrees, MBA, MCA courses", Status = "active", SchoolRegistrationId = school.Id },
                    new EducationLevel { Name = "Doctorate & Research (PhD)", Code = "PHD-DOCTORATE", Description = "PhD research, fellowship directories and post-doctorate files", Status = "active", SchoolRegistrationId = school.Id },
                    new EducationLevel { Name = "Vocational & Professional Certifications", Code = "VOCATIONAL-CERT", Description = "Short term skill trades, nursing certificates, and job ready modules", Status = "active", SchoolRegistrationId = school.Id }
                };
                await context.EducationLevels.AddRangeAsync(levels);
                await context.SaveChangesAsync();
            }

            // Resolve references
            var mainCampus = await context.Campuses.FirstAsync(c => c.Code == "MAIN");
            var southCampus = await context.Campuses.FirstAsync(c => c.Code == "SOUTH-MED");
            var westCampus = await context.Campuses.FirstAsync(c => c.Code == "WEST-ELITE");
            var techCampus = await context.Campuses.FirstAsync(c => c.Code == "TECH-CITY");
            var northCampus = await context.Campuses.FirstAsync(c => c.Code == "NORTH");

            var ugLevel = await context.EducationLevels.FirstAsync(l => l.Code == "UG-BACHELOR");
            var pgLevel = await context.EducationLevels.FirstAsync(l => l.Code == "PG-MASTERS");
            var phdLevel = await context.EducationLevels.FirstAsync(l => l.Code == "PHD-DOCTORATE");
            var dipLevel = await context.EducationLevels.FirstAsync(l => l.Code == "DIPLOMA-ITI");
            var schoolLevel = await context.EducationLevels.FirstAsync(l => l.Code == "PRIM-MID");
            var highSchoolLevel = await context.EducationLevels.FirstAsync(l => l.Code == "SEC-HSC");
            var vocLevel = await context.EducationLevels.FirstAsync(l => l.Code == "VOCATIONAL-CERT");

            // ─── 3. Seed Programs (12 records) ─────────────────────────────────────────
            if (!context.Programs.Any())
            {
                var programs = new List<Program>
                {
                    new Program { Name = "Bachelor of Technology in CSE", Code = "BTECH-CSE", EducationLevelId = ugLevel.Id, DurationYears = 4, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Master of Business Administration", Code = "MBA", EducationLevelId = pgLevel.Id, DurationYears = 2, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Diploma in General Nursing & Midwifery", Code = "DIPL-GNM", EducationLevelId = dipLevel.Id, DurationYears = 3, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Nursery Foundation Course", Code = "NUR-FOUND", EducationLevelId = schoolLevel.Id, DurationYears = 1, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Higher Secondary Science Matriculation", Code = "HSC-SCI", EducationLevelId = highSchoolLevel.Id, DurationYears = 2, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Bachelor of Computer Applications", Code = "BCA", EducationLevelId = ugLevel.Id, DurationYears = 3, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Master of Computer Applications", Code = "MCA", EducationLevelId = pgLevel.Id, DurationYears = 2, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Doctor of Philosophy in Computer Science", Code = "PHD-CS", EducationLevelId = phdLevel.Id, DurationYears = 3, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Bachelor of Science in Nursing", Code = "BSC-NURSING", EducationLevelId = ugLevel.Id, DurationYears = 4, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "ITI Electrician Certification", Code = "ITI-ELEC", EducationLevelId = vocLevel.Id, DurationYears = 2, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Polytechnic Diploma in Civil Engineering", Code = "POLY-CIVIL", EducationLevelId = dipLevel.Id, DurationYears = 3, Status = "active", SchoolRegistrationId = school.Id },
                    new Program { Name = "Master of Science in Biotechnology", Code = "MSC-BIOTECH", EducationLevelId = pgLevel.Id, DurationYears = 2, Status = "active", SchoolRegistrationId = school.Id }
                };
                await context.Programs.AddRangeAsync(programs);
                await context.SaveChangesAsync();
            }

            var btechProg = await context.Programs.FirstAsync(p => p.Code == "BTECH-CSE");
            var mbaProg = await context.Programs.FirstAsync(p => p.Code == "MBA");
            var gnmProg = await context.Programs.FirstAsync(p => p.Code == "DIPL-GNM");
            var nurseryProg = await context.Programs.FirstAsync(p => p.Code == "NUR-FOUND");
            var hscProg = await context.Programs.FirstAsync(p => p.Code == "HSC-SCI");
            var bcaProg = await context.Programs.FirstAsync(p => p.Code == "BCA");
            var mcaProg = await context.Programs.FirstAsync(p => p.Code == "MCA");
            var phdProg = await context.Programs.FirstAsync(p => p.Code == "PHD-CS");
            var bscNurseProg = await context.Programs.FirstAsync(p => p.Code == "BSC-NURSING");
            var itiElecProg = await context.Programs.FirstAsync(p => p.Code == "ITI-ELEC");
            var polyCivilProg = await context.Programs.FirstAsync(p => p.Code == "POLY-CIVIL");
            var biotechProg = await context.Programs.FirstAsync(p => p.Code == "MSC-BIOTECH");

            // ─── 4. Seed Branches / Specializations (12 records) ───────────────────────
            if (!context.Branches.Any())
            {
                var branches = new List<Branch>
                {
                    new Branch { Name = "Artificial Intelligence & Data Science", Code = "AI-DS", ProgramId = btechProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Software Engineering Practices", Code = "SWE", ProgramId = btechProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Finance & Portfolio Analytics", Code = "MBA-FIN", ProgramId = mbaProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Critical Care Nursing Unit", Code = "GNM-CCU", ProgramId = gnmProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Pure Mathematics & Physics", Code = "SCI-MP", ProgramId = hscProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Cloud Computing & DevOps", Code = "BCA-CC", ProgramId = bcaProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Machine Learning & Big Data", Code = "MCA-ML", ProgramId = mcaProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Natural Language Processing Research", Code = "PHD-NLP", ProgramId = phdProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Obstetrics & Pediatric Care", Code = "BSC-OBG", ProgramId = bscNurseProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Industrial Automation & Power Grid", Code = "ITI-POWER", ProgramId = itiElecProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Structural Engineering & Hydraulics", Code = "POLY-STRUCT", ProgramId = polyCivilProg.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Branch { Name = "Recombinant DNA & Gene Editing", Code = "BIOTECH-GEN", ProgramId = biotechProg.Id, Status = "active", SchoolRegistrationId = school.Id }
                };
                await context.Branches.AddRangeAsync(branches);
                await context.SaveChangesAsync();
            }

            var branchAi = await context.Branches.FirstAsync(b => b.Code == "AI-DS");
            var branchFin = await context.Branches.FirstAsync(b => b.Code == "MBA-FIN");
            var branchMcaMl = await context.Branches.FirstAsync(b => b.Code == "MCA-ML");
            var branchPhdNlp = await context.Branches.FirstAsync(b => b.Code == "PHD-NLP");
            var branchBscObg = await context.Branches.FirstAsync(b => b.Code == "BSC-OBG");
            var branchItiPower = await context.Branches.FirstAsync(b => b.Code == "ITI-POWER");
            var branchPolyStruct = await context.Branches.FirstAsync(b => b.Code == "POLY-STRUCT");
            var branchBiotechGen = await context.Branches.FirstAsync(b => b.Code == "BIOTECH-GEN");

            // ─── 5. Seed Batches (12 records) ──────────────────────────────────────────
            if (!context.Batches.Any())
            {
                var batches = new List<Batch>
                {
                    new Batch { Name = "B.Tech CSE Batch 2026-30", Code = "CSE-26-30", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2030, 6, 30), ProgramId = btechProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "MBA Finance Batch 2026-28", Code = "MBA-26-28", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2028, 6, 30), ProgramId = mbaProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "Nursing GNM Batch 2026-29", Code = "GNM-26-29", StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2029, 7, 31), ProgramId = gnmProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "Nursery Cohort 2026-27", Code = "NUR-26-27", StartDate = new DateTime(2026, 4, 1), EndDate = new DateTime(2027, 3, 31), ProgramId = nurseryProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "HSC Science Class 2026-28", Code = "HSC-26-28", StartDate = new DateTime(2026, 4, 1), EndDate = new DateTime(2028, 3, 31), ProgramId = hscProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "BCA Session 2026-29", Code = "BCA-26-29", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2029, 6, 30), ProgramId = bcaProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "MCA Session 2026-28", Code = "MCA-26-28", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2028, 6, 30), ProgramId = mcaProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "PhD Research Fellowship 2026-29", Code = "PHD-26-29", StartDate = new DateTime(2026, 9, 1), EndDate = new DateTime(2029, 8, 31), ProgramId = phdProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "B.Sc Nursing Batch 2026-30", Code = "BSCN-26-30", StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2030, 7, 31), ProgramId = bscNurseProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "ITI Electrician Batch 2026-28", Code = "ITI-26-28", StartDate = new DateTime(2026, 7, 15), EndDate = new DateTime(2028, 6, 14), ProgramId = itiElecProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "Polytechnic Civil Batch 2026-29", Code = "POLY-26-29", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2029, 6, 30), ProgramId = polyCivilProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id },
                    new Batch { Name = "M.Sc Biotech Cohort 2026-28", Code = "BIOTECH-26-28", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2028, 6, 30), ProgramId = biotechProg.Id, AcademicYearId = academicYear.Id, Status = "active", SchoolRegistrationId = school.Id }
                };
                await context.Batches.AddRangeAsync(batches);
                await context.SaveChangesAsync();
            }

            var btechBatch = await context.Batches.FirstAsync(b => b.Code == "CSE-26-30");
            var gnmBatch = await context.Batches.FirstAsync(b => b.Code == "GNM-26-29");
            var mcaBatch = await context.Batches.FirstAsync(b => b.Code == "MCA-26-28");
            var phdBatch = await context.Batches.FirstAsync(b => b.Code == "PHD-26-29");
            var bscNurseBatch = await context.Batches.FirstAsync(b => b.Code == "BSCN-26-30");
            var itiElecBatch = await context.Batches.FirstAsync(b => b.Code == "ITI-26-28");
            var polyCivilBatch = await context.Batches.FirstAsync(b => b.Code == "POLY-26-29");
            var biotechBatch = await context.Batches.FirstAsync(b => b.Code == "BIOTECH-26-28");

            // ─── 6. Seed YearSemesters (6 records) ──────────────────────────────────────
            if (!context.YearSemesters.Any())
            {
                var semesters = new List<YearSemester>
                {
                    new YearSemester { Name = "Semester 1", Code = "SEM1", Sequence = 1, Status = "active", SchoolRegistrationId = school.Id },
                    new YearSemester { Name = "Semester 2", Code = "SEM2", Sequence = 2, Status = "active", SchoolRegistrationId = school.Id },
                    new YearSemester { Name = "Year 1 (Annual)", Code = "YR1", Sequence = 1, Status = "active", SchoolRegistrationId = school.Id },
                    new YearSemester { Name = "Year 2 (Annual)", Code = "YR2", Sequence = 2, Status = "active", SchoolRegistrationId = school.Id },
                    new YearSemester { Name = "Semester 3", Code = "SEM3", Sequence = 3, Status = "active", SchoolRegistrationId = school.Id },
                    new YearSemester { Name = "Semester 4", Code = "SEM4", Sequence = 4, Status = "active", SchoolRegistrationId = school.Id }
                };
                await context.YearSemesters.AddRangeAsync(semesters);
                await context.SaveChangesAsync();
            }

            var sem1 = await context.YearSemesters.FirstAsync(s => s.Code == "SEM1");

            // ─── 7. Seed Form Configs (8 records) ───────────────────────────────────────
            if (!context.AdmissionFormConfigs.Any())
            {
                var stepConfigJson = "[{\"key\":\"personal\",\"label\":\"Personal Details\",\"active\":true,\"order\":1},{\"key\":\"parent\",\"label\":\"Parent Details\",\"active\":true,\"order\":2},{\"key\":\"address\",\"label\":\"Address Details\",\"active\":true,\"order\":3},{\"key\":\"education\",\"label\":\"Qualifications\",\"active\":true,\"order\":4},{\"key\":\"documents\",\"label\":\"Document uploads\",\"active\":true,\"order\":5},{\"key\":\"custom\",\"label\":\"Additional Info\",\"active\":true,\"order\":6}]";
                var checklistJson = "[{\"name\":\"Aadhaar Card Copy\",\"required\":true},{\"name\":\"10th Certificate\",\"required\":true},{\"name\":\"12th Certificate\",\"required\":true}]";
                var autoGenJson = "{\"admissionNoPrefix\":\"ADM-2026-\",\"enrollmentNoPrefix\":\"ENR-2026-\",\"studentCodePrefix\":\"STU-2026-\"}";

                var formConfigs = new List<AdmissionFormConfig>
                {
                    new AdmissionFormConfig { CampusId = mainCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = btechProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = checklistJson, CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = southCampus.Id, EducationLevelId = dipLevel.Id, ProgramId = gnmProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = checklistJson, CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = westCampus.Id, EducationLevelId = schoolLevel.Id, ProgramId = nurseryProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = "[{\"name\":\"Birth Certificate\",\"required\":true}]", CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = mainCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = mbaProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = checklistJson, CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = mainCampus.Id, EducationLevelId = highSchoolLevel.Id, ProgramId = hscProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = checklistJson, CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = techCampus.Id, EducationLevelId = phdLevel.Id, ProgramId = phdProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = "[{\"name\":\"PG Degree certificate\",\"required\":true},{\"name\":\"Synopsis Proposal\",\"required\":true}]", CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = southCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = bscNurseProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = checklistJson, CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionFormConfig { CampusId = northCampus.Id, EducationLevelId = dipLevel.Id, ProgramId = polyCivilProg.Id, FormStepsJson = stepConfigJson, DocumentChecklistJson = checklistJson, CustomFieldsJson = "[]", AutoGenRulesJson = autoGenJson, IsActive = true, SchoolRegistrationId = school.Id }
                };
                await context.AdmissionFormConfigs.AddRangeAsync(formConfigs);
                await context.SaveChangesAsync();
            }

            // ─── 8. Seed Criteria Rules (10 records) ────────────────────────────────────
            if (!context.AdmissionRules.Any())
            {
                var rules = new List<AdmissionRule>
                {
                    new AdmissionRule { CampusId = mainCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = btechProg.Id, RuleName = "B.Tech Age Limit", RuleType = "MinAge", RuleValue = "17", ErrorMessage = "Min age for B.Tech is 17 years", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = westCampus.Id, EducationLevelId = schoolLevel.Id, ProgramId = nurseryProg.Id, RuleName = "Nursery Age Bound", RuleType = "MinAge", RuleValue = "3", ErrorMessage = "Candidate must be at least 3 years old", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = southCampus.Id, EducationLevelId = dipLevel.Id, ProgramId = gnmProg.Id, RuleName = "GNM Age limit", RuleType = "MinAge", RuleValue = "17", ErrorMessage = "Min age for GNM is 17 years", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = mainCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = mbaProg.Id, RuleName = "MBA Entry Bound", RuleType = "MinAge", RuleValue = "20", ErrorMessage = "Minimum age is 20", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = mainCampus.Id, EducationLevelId = highSchoolLevel.Id, ProgramId = hscProg.Id, RuleName = "HSC Age bound", RuleType = "MinAge", RuleValue = "14", ErrorMessage = "Minimum age limit is 14", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = techCampus.Id, EducationLevelId = phdLevel.Id, ProgramId = phdProg.Id, RuleName = "PhD Age limit", RuleType = "MinAge", RuleValue = "22", ErrorMessage = "Minimum age for PhD entry is 22", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = southCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = bscNurseProg.Id, RuleName = "B.Sc Nursing Age limit", RuleType = "MinAge", RuleValue = "17", ErrorMessage = "Minimum age limit is 17", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = mainCampus.Id, EducationLevelId = vocLevel.Id, ProgramId = itiElecProg.Id, RuleName = "ITI Age bound", RuleType = "MinAge", RuleValue = "15", ErrorMessage = "Minimum age limit is 15", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = northCampus.Id, EducationLevelId = dipLevel.Id, ProgramId = polyCivilProg.Id, RuleName = "Polytechnic Age bound", RuleType = "MinAge", RuleValue = "15", ErrorMessage = "Minimum age limit is 15", IsActive = true, SchoolRegistrationId = school.Id },
                    new AdmissionRule { CampusId = techCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = biotechProg.Id, RuleName = "Biotech Age bound", RuleType = "MinAge", RuleValue = "20", ErrorMessage = "Minimum age limit is 20", IsActive = true, SchoolRegistrationId = school.Id }
                };
                await context.AdmissionRules.AddRangeAsync(rules);
                await context.SaveChangesAsync();
            }
            // ─── 8.5. Seed Fee Types (if none exist) ────────────────────────────────────
            if (!context.FeeTypes.Any())
            {
                var feeTypes = new List<FeeType>
                {
                    new FeeType { Name = "Tuition Fee", description = "Monthly/Semester tuition fee", IsActive = true, SchoolRegistrationId = school.Id },
                    new FeeType { Name = "Hostel Fee", description = "Hostel and lodging charges", IsActive = true, SchoolRegistrationId = school.Id },
                    new FeeType { Name = "Library Fee", description = "Library membership and usage", IsActive = true, SchoolRegistrationId = school.Id },
                    new FeeType { Name = "Examination Fee", description = "Semester examination fee", IsActive = true, SchoolRegistrationId = school.Id },
                    new FeeType { Name = "Transport Fee", description = "Bus and van commute charges", IsActive = true, SchoolRegistrationId = school.Id }
                };
                await context.FeeTypes.AddRangeAsync(feeTypes);
                await context.SaveChangesAsync();
            }

            var tuitionFee = await context.FeeTypes.FirstAsync(f => f.Name == "Tuition Fee");
            var hostelFee = await context.FeeTypes.FirstAsync(f => f.Name == "Hostel Fee");
            var libraryFee = await context.FeeTypes.FirstAsync(f => f.Name == "Library Fee");

            // ─── 9. Seed Fee Structures (12 records) ────────────────────────────────────
            if (!context.FeeStructures.Any())
            {
                var feeStructures = new List<FeeStructure>
                {
                    new FeeStructure
                    {
                        Name = "Standard B.Tech CSE Fee 2026 Batch", CampusId = mainCampus.Id, ProgramId = btechProg.Id, BatchId = btechBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 65000 },
                            new FeeStructureItem { FeeTypeId = libraryFee.Id, Amount = 4000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "GNM South Campus Fee 2026 Batch", CampusId = southCampus.Id, ProgramId = gnmProg.Id, BatchId = gnmBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 45000 },
                            new FeeStructureItem { FeeTypeId = hostelFee.Id, Amount = 12000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "Nursery Basic Fee Session 2026", CampusId = westCampus.Id, ProgramId = nurseryProg.Id, BatchId = context.Batches.First(b => b.Code == "NUR-26-27").Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 25000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "MBA Main Campus Fee Structure", CampusId = mainCampus.Id, ProgramId = mbaProg.Id, BatchId = context.Batches.First(b => b.Code == "MBA-26-28").Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 95000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "HSC Science Annual Fee", CampusId = mainCampus.Id, ProgramId = hscProg.Id, BatchId = context.Batches.First(b => b.Code == "HSC-26-28").Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 35000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "BCA Tech Campus Fee Structure", CampusId = techCampus.Id, ProgramId = bcaProg.Id, BatchId = context.Batches.First(b => b.Code == "BCA-26-29").Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 48000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "MCA Tech Campus Fee Structure", CampusId = techCampus.Id, ProgramId = mcaProg.Id, BatchId = mcaBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 75000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "PhD Fellowship Registration Fee", CampusId = techCampus.Id, ProgramId = phdProg.Id, BatchId = phdBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 15000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "B.Sc Nursing Fee Main Session 2026", CampusId = southCampus.Id, ProgramId = bscNurseProg.Id, BatchId = bscNurseBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 58000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "ITI Electrician Lab Fee Structure", CampusId = mainCampus.Id, ProgramId = itiElecProg.Id, BatchId = itiElecBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 18000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "Polytechnic Civil Fee North Branch", CampusId = northCampus.Id, ProgramId = polyCivilProg.Id, BatchId = polyCivilBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 32000 }
                        }
                    },
                    new FeeStructure
                    {
                        Name = "M.Sc Biotech Tuition Fee 2026", CampusId = techCampus.Id, ProgramId = biotechProg.Id, BatchId = biotechBatch.Id, IsActive = true, SchoolRegistrationId = school.Id,
                        FeeStructureItems = new List<FeeStructureItem>
                        {
                            new FeeStructureItem { FeeTypeId = tuitionFee.Id, Amount = 80000 }
                        }
                    }
                };
                await context.FeeStructures.AddRangeAsync(feeStructures);
                await context.SaveChangesAsync();
            }

            // ─── 10. Seed Admission Applications (15 records) ──────────────────────────
            if (!context.AdmissionApplications.Any())
            {
                var apps = new List<AdmissionApplication>
                {
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00001", RegistrationNo = "REG-2026-8001", Status = "Submitted",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = btechProg.Id, BranchId = branchAi.Id, BatchId = btechBatch.Id, YearSemesterId = sem1.Id,
                        FullName = "Aryan Verma", DateOfBirth = new DateTime(2008, 5, 12), Gender = "Male", Mobile = "9876543220", Email = "aryan.verma@email.com", AadhaarNo = "123456789012",
                        FathersName = "Rajesh Verma", MothersName = "Sunita Verma", GuardianName = "Rajesh Verma", GuardianMobile = "9876543220",
                        PermanentAddress = "102, Shanti Vihar", PermanentCity = "Noida", PermanentState = "Uttar Pradesh", PermanentPinCode = "201301", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "12th Standard", LastInstituteName = "DPS Noida", LastBoardUniversity = "CBSE", LastPassingYear = "2026", LastObtainedMarks = "450", LastTotalMarks = "500", LastPercentage = "90%", LastGrade = "A",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00002", RegistrationNo = null, Status = "Draft",
                        AcademicYearId = academicYear.Id, CampusId = westCampus.Id, EducationLevelId = schoolLevel.Id, ProgramId = nurseryProg.Id, BranchId = null, BatchId = context.Batches.First(b => b.Code == "NUR-26-27").Id, YearSemesterId = null,
                        FullName = "Kiara Sharma", DateOfBirth = new DateTime(2023, 2, 10), Gender = "Female", Mobile = "9876543221", Email = "kiara@email.com", AadhaarNo = "123456789013",
                        FathersName = "Amit Sharma", MothersName = "Priya Sharma", GuardianName = "Amit Sharma", GuardianMobile = "9876543221",
                        PermanentAddress = "Flat 4B, Bandra Heights", PermanentCity = "Mumbai", PermanentState = "Maharashtra", PermanentPinCode = "400050", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "None (Nursery Entry)", LastInstituteName = "N/A", LastBoardUniversity = "N/A", LastPassingYear = "2026", LastObtainedMarks = "0", LastTotalMarks = "0", LastPercentage = "0%", LastGrade = "N/A",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00003", RegistrationNo = "REG-2026-8002", Status = "Approved",
                        AcademicYearId = academicYear.Id, CampusId = southCampus.Id, EducationLevelId = dipLevel.Id, ProgramId = gnmProg.Id, BranchId = context.Branches.First(b => b.Code == "GNM-CCU").Id, BatchId = gnmBatch.Id, YearSemesterId = null,
                        FullName = "Preeti Reddy", DateOfBirth = new DateTime(2007, 8, 22), Gender = "Female", Mobile = "9876543222", Email = "preeti.reddy@email.com", AadhaarNo = "123456789014",
                        FathersName = "M. S. Reddy", MothersName = "M. K. Reddy", GuardianName = "M. S. Reddy", GuardianMobile = "9876543222",
                        PermanentAddress = "15/3, MG Road", PermanentCity = "Bangalore", PermanentState = "Karnataka", PermanentPinCode = "560001", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "10th Standard", LastInstituteName = "St. Joseph School", LastBoardUniversity = "Karnataka Board", LastPassingYear = "2025", LastObtainedMarks = "540", LastTotalMarks = "600", LastPercentage = "90%", LastGrade = "A+",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00004", RegistrationNo = "REG-2026-8003", Status = "Enrolled",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = mbaProg.Id, BranchId = branchFin.Id, BatchId = context.Batches.First(b => b.Code == "MBA-26-28").Id, YearSemesterId = sem1.Id,
                        FullName = "Vikram Aditya", DateOfBirth = new DateTime(2003, 11, 5), Gender = "Male", Mobile = "9876543223", Email = "vikram.aditya@email.com", AadhaarNo = "123456789015",
                        FathersName = "K. Aditya", MothersName = "V. Aditya", GuardianName = "K. Aditya", GuardianMobile = "9876543223",
                        PermanentAddress = "Sector 15", PermanentCity = "Noida", PermanentState = "Uttar Pradesh", PermanentPinCode = "201301", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "BBA Degree", LastInstituteName = "Amity Noida", LastBoardUniversity = "Amity University", LastPassingYear = "2026", LastObtainedMarks = "8.8", LastTotalMarks = "10", LastPercentage = "88%", LastGrade = "First",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00005", RegistrationNo = "REG-2026-8004", Status = "Waiting List",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = highSchoolLevel.Id, ProgramId = hscProg.Id, BranchId = context.Branches.First(b => b.Code == "SCI-MP").Id, BatchId = context.Batches.First(b => b.Code == "HSC-26-28").Id, YearSemesterId = null,
                        FullName = "Kabir Singh", DateOfBirth = new DateTime(2010, 4, 15), Gender = "Male", Mobile = "9876543224", Email = "kabir@email.com", AadhaarNo = "123456789016",
                        FathersName = "Sarbjit Singh", MothersName = "Harmeet Kaur", GuardianName = "Sarbjit Singh", GuardianMobile = "9876543224",
                        PermanentAddress = "Model Town", PermanentCity = "Ludhiana", PermanentState = "Punjab", PermanentPinCode = "141002", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "10th Standard", LastInstituteName = "Sacred Heart School", LastBoardUniversity = "ICSE", LastPassingYear = "2026", LastObtainedMarks = "420", LastTotalMarks = "500", LastPercentage = "84%", LastGrade = "B",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00006", RegistrationNo = "REG-2026-8005", Status = "Submitted",
                        AcademicYearId = academicYear.Id, CampusId = techCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = mcaProg.Id, BranchId = branchMcaMl.Id, BatchId = mcaBatch.Id, YearSemesterId = sem1.Id,
                        FullName = "Neha Kashyap", DateOfBirth = new DateTime(2004, 9, 18), Gender = "Female", Mobile = "9876543225", Email = "neha@email.com", AadhaarNo = "123456789017",
                        FathersName = "Satish Kashyap", MothersName = "Rajni Kashyap", GuardianName = "Satish Kashyap", GuardianMobile = "9876543225",
                        PermanentAddress = "34/A, Salt Lake Road", PermanentCity = "Kolkata", PermanentState = "West Bengal", PermanentPinCode = "700091", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "BCA Degree", LastInstituteName = "Heritage Institute", LastBoardUniversity = "MAKAUT", LastPassingYear = "2026", LastObtainedMarks = "820", LastTotalMarks = "1000", LastPercentage = "82%", LastGrade = "First",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00007", RegistrationNo = "REG-2026-8006", Status = "Submitted",
                        AcademicYearId = academicYear.Id, CampusId = techCampus.Id, EducationLevelId = phdLevel.Id, ProgramId = phdProg.Id, BranchId = branchPhdNlp.Id, BatchId = phdBatch.Id, YearSemesterId = null,
                        FullName = "Dr. Amit Roy", DateOfBirth = new DateTime(2000, 1, 15), Gender = "Male", Mobile = "9876543226", Email = "amit.roy@email.com", AadhaarNo = "123456789018",
                        FathersName = "S. N. Roy", MothersName = "Anjana Roy", GuardianName = "S. N. Roy", GuardianMobile = "9876543226",
                        PermanentAddress = "Lake Avenue", PermanentCity = "Kolkata", PermanentState = "West Bengal", PermanentPinCode = "700029", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "M.Tech CSE", LastInstituteName = "IIT Kharagpur", LastBoardUniversity = "IIT", LastPassingYear = "2025", LastObtainedMarks = "9.2", LastTotalMarks = "10", LastPercentage = "92%", LastGrade = "Honors",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00008", RegistrationNo = null, Status = "Draft",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = bcaProg.Id, BranchId = context.Branches.First(b => b.Code == "BCA-CC").Id, BatchId = context.Batches.First(b => b.Code == "BCA-26-29").Id, YearSemesterId = sem1.Id,
                        FullName = "Rahul Dwivedi", DateOfBirth = new DateTime(2008, 12, 10), Gender = "Male", Mobile = "9876543227", Email = "rahul.dwivedi@email.com", AadhaarNo = "123456789019",
                        FathersName = "S. K. Dwivedi", MothersName = "Kiran Dwivedi", GuardianName = "S. K. Dwivedi", GuardianMobile = "9876543227",
                        PermanentAddress = "234, Shanti Enclave", PermanentCity = "Lucknow", PermanentState = "Uttar Pradesh", PermanentPinCode = "226016", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "12th Commerce", LastInstituteName = "La Martiniere", LastBoardUniversity = "ISC", LastPassingYear = "2026", LastObtainedMarks = "410", LastTotalMarks = "500", LastPercentage = "82%", LastGrade = "B+",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00009", RegistrationNo = "REG-2026-8007", Status = "Submitted",
                        AcademicYearId = academicYear.Id, CampusId = southCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = bscNurseProg.Id, BranchId = branchBscObg.Id, BatchId = bscNurseBatch.Id, YearSemesterId = null,
                        FullName = "Sneha Patil", DateOfBirth = new DateTime(2008, 3, 14), Gender = "Female", Mobile = "9876543228", Email = "sneha.patil@email.com", AadhaarNo = "123456789020",
                        FathersName = "Gopal Patil", MothersName = "Lata Patil", GuardianName = "Gopal Patil", GuardianMobile = "9876543228",
                        PermanentAddress = "Budhwar Peth", PermanentCity = "Pune", PermanentState = "Maharashtra", PermanentPinCode = "411002", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "12th Science", LastInstituteName = "Fergusson Junior College", LastBoardUniversity = "HSC Maharashtra", LastPassingYear = "2026", LastObtainedMarks = "490", LastTotalMarks = "600", LastPercentage = "81%", LastGrade = "B",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00010", RegistrationNo = null, Status = "Draft",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = vocLevel.Id, ProgramId = itiElecProg.Id, BranchId = branchItiPower.Id, BatchId = itiElecBatch.Id, YearSemesterId = null,
                        FullName = "Rohan Malhotra", DateOfBirth = new DateTime(2009, 11, 2), Gender = "Male", Mobile = "9876543229", Email = "rohan.malhotra@email.com", AadhaarNo = "123456789021",
                        FathersName = "S. P. Malhotra", MothersName = "Vimla Malhotra", GuardianName = "S. P. Malhotra", GuardianMobile = "9876543229",
                        PermanentAddress = "12, Patel Nagar", PermanentCity = "New Delhi", PermanentState = "Delhi", PermanentPinCode = "110008", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "10th Standard", LastInstituteName = "DAV Public School", LastBoardUniversity = "CBSE", LastPassingYear = "2025", LastObtainedMarks = "380", LastTotalMarks = "500", LastPercentage = "76%", LastGrade = "B-",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00011", RegistrationNo = "REG-2026-8008", Status = "Approved",
                        AcademicYearId = academicYear.Id, CampusId = northCampus.Id, EducationLevelId = dipLevel.Id, ProgramId = polyCivilProg.Id, BranchId = branchPolyStruct.Id, BatchId = polyCivilBatch.Id, YearSemesterId = null,
                        FullName = "Tanya Sen", DateOfBirth = new DateTime(2009, 7, 25), Gender = "Female", Mobile = "9876543230", Email = "tanya@email.com", AadhaarNo = "123456789022",
                        FathersName = "Rajib Sen", MothersName = "Kaberi Sen", GuardianName = "Rajib Sen", GuardianMobile = "9876543230",
                        PermanentAddress = "Hill View Enclave", PermanentCity = "Guwahati", PermanentState = "Assam", PermanentPinCode = "781003", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "10th Standard", LastInstituteName = "Don Bosco School", LastBoardUniversity = "SEBA", LastPassingYear = "2025", LastObtainedMarks = "460", LastTotalMarks = "500", LastPercentage = "92%", LastGrade = "A+",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00012", RegistrationNo = "REG-2026-8009", Status = "Enrolled",
                        AcademicYearId = academicYear.Id, CampusId = techCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = biotechProg.Id, BranchId = branchBiotechGen.Id, BatchId = biotechBatch.Id, YearSemesterId = sem1.Id,
                        FullName = "Aditya Nair", DateOfBirth = new DateTime(2004, 2, 28), Gender = "Male", Mobile = "9876543231", Email = "aditya.nair@email.com", AadhaarNo = "123456789023",
                        FathersName = "Radhakrishnan Nair", MothersName = "Shailaja Nair", GuardianName = "Radhakrishnan Nair", GuardianMobile = "9876543231",
                        PermanentAddress = "Pattom Colony", PermanentCity = "Trivandrum", PermanentState = "Kerala", PermanentPinCode = "695004", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "B.Sc Biotechnology", LastInstituteName = "MG University", LastBoardUniversity = "MG Kerala", LastPassingYear = "2026", LastObtainedMarks = "8.9", LastTotalMarks = "10", LastPercentage = "89%", LastGrade = "First",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00013", RegistrationNo = null, Status = "Rejected",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = ugLevel.Id, ProgramId = btechProg.Id, BranchId = branchAi.Id, BatchId = btechBatch.Id, YearSemesterId = sem1.Id,
                        FullName = "Arjun Reddy", DateOfBirth = new DateTime(2010, 10, 10), Gender = "Male", Mobile = "9876543232", Email = "arjun.reddy@email.com", AadhaarNo = "123456789024",
                        FathersName = "S. N. Reddy", MothersName = "S. K. Reddy", GuardianName = "S. N. Reddy", GuardianMobile = "9876543232",
                        PermanentAddress = "Hitech City", PermanentCity = "Hyderabad", PermanentState = "Telangana", PermanentPinCode = "500081", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "12th Standard", LastInstituteName = "Chaitanya Academy", LastBoardUniversity = "BIE Telangana", LastPassingYear = "2026", LastObtainedMarks = "290", LastTotalMarks = "500", LastPercentage = "58%", LastGrade = "D",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00014", RegistrationNo = "REG-2026-8010", Status = "Waiting List",
                        AcademicYearId = academicYear.Id, CampusId = mainCampus.Id, EducationLevelId = pgLevel.Id, ProgramId = mbaProg.Id, BranchId = branchFin.Id, BatchId = context.Batches.First(b => b.Code == "MBA-26-28").Id, YearSemesterId = sem1.Id,
                        FullName = "Simran Kaur", DateOfBirth = new DateTime(2004, 6, 30), Gender = "Female", Mobile = "9876543233", Email = "simran@email.com", AadhaarNo = "123456789025",
                        FathersName = "Harpal Singh", MothersName = "Jaspreet Kaur", GuardianName = "Harpal Singh", GuardianMobile = "9876543233",
                        PermanentAddress = "GT Road", PermanentCity = "Amritsar", PermanentState = "Punjab", PermanentPinCode = "143001", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "B.Com", LastInstituteName = "Khalsa College", LastBoardUniversity = "GNDU", LastPassingYear = "2026", LastObtainedMarks = "760", LastTotalMarks = "1000", LastPercentage = "76%", LastGrade = "B",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    },
                    new AdmissionApplication
                    {
                        ApplicationNo = "APP-2026-00015", RegistrationNo = "REG-2026-8011", Status = "Submitted",
                        AcademicYearId = academicYear.Id, CampusId = techCampus.Id, EducationLevelId = phdLevel.Id, ProgramId = phdProg.Id, BranchId = branchPhdNlp.Id, BatchId = phdBatch.Id, YearSemesterId = null,
                        FullName = "Zoya Ahmed", DateOfBirth = new DateTime(2001, 12, 5), Gender = "Female", Mobile = "9876543234", Email = "zoya@email.com", AadhaarNo = "123456789026",
                        FathersName = "T. Ahmed", MothersName = "F. Ahmed", GuardianName = "T. Ahmed", GuardianMobile = "9876543234",
                        PermanentAddress = "Park Street", PermanentCity = "Kolkata", PermanentState = "West Bengal", PermanentPinCode = "700016", PermanentCountry = "India", SameAsPermAddress = true,
                        LastQualification = "MCA Degree", LastInstituteName = "Jadavpur University", LastBoardUniversity = "Jadavpur", LastPassingYear = "2025", LastObtainedMarks = "9.1", LastTotalMarks = "10", LastPercentage = "91%", LastGrade = "Honors",
                        PrevEducationJson = "[]", DocumentsJson = "[]", CustomFieldsDataJson = "{}",
                        CreatedBy = "seed", CreatedDate = DateTime.UtcNow, SchoolRegistrationId = school.Id
                    }
                };
                await context.AdmissionApplications.AddRangeAsync(apps);
                await context.SaveChangesAsync();
            }

            var verifiedApp = await context.AdmissionApplications.FirstAsync(a => a.ApplicationNo == "APP-2026-00001");

            // ─── 11. Seed Audit Logs (5 records) ────────────────────────────────────────
            if (!context.AdmissionAuditLogs.Any())
            {
                var logs = new List<AdmissionAuditLog>
                {
                    new AdmissionAuditLog { AdmissionApplicationId = verifiedApp.Id, Action = "Draft Saved", StatusFrom = "None", StatusTo = "Draft", PerformedBy = "aryan.verma@email.com", PerformedDate = DateTime.UtcNow.AddDays(-2), DetailsJson = "Initial draft saved by applicant" },
                    new AdmissionAuditLog { AdmissionApplicationId = verifiedApp.Id, Action = "Submitted", StatusFrom = "Draft", StatusTo = "Submitted", PerformedBy = "aryan.verma@email.com", PerformedDate = DateTime.UtcNow.AddDays(-2).AddHours(1), DetailsJson = "Successfully verified age rule constraints and submitted for verification" },
                    new AdmissionAuditLog { AdmissionApplicationId = verifiedApp.Id, Action = "Document Verification", StatusFrom = "Submitted", StatusTo = "Under Verification", PerformedBy = "admin@schoolsaas.com", PerformedDate = DateTime.UtcNow.AddDays(-1), DetailsJson = "10th Marksheet marked as Verified" },
                    new AdmissionAuditLog { AdmissionApplicationId = verifiedApp.Id, Action = "Document Verification", StatusFrom = "Under Verification", StatusTo = "Under Verification", PerformedBy = "admin@schoolsaas.com", PerformedDate = DateTime.UtcNow.AddDays(-1), DetailsJson = "Aadhaar Card Copy marked as Verified" },
                    new AdmissionAuditLog { AdmissionApplicationId = verifiedApp.Id, Action = "Document Verification", StatusFrom = "Under Verification", StatusTo = "Under Verification", PerformedBy = "admin@schoolsaas.com", PerformedDate = DateTime.UtcNow.AddDays(-1), DetailsJson = "12th Marksheet marked as Verified" }
                };
                await context.AdmissionAuditLogs.AddRangeAsync(logs);
                await context.SaveChangesAsync();
            }
        }
    }
}
