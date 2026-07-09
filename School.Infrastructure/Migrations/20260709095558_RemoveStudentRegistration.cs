using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudentRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop FK on EducationalDetails only if table and constraint exist
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                    WHERE TABLE_NAME = 'EducationalDetails'
                      AND CONSTRAINT_NAME = 'FK_EducationalDetails_StudentRegistrations_StudentRegistrationId'
                )
                    ALTER TABLE [EducationalDetails] DROP CONSTRAINT [FK_EducationalDetails_StudentRegistrations_StudentRegistrationId];
            ");

            // Drop index on EducationalDetails only if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'IX_EducationalDetails_StudentRegistrationId'
                      AND object_id = OBJECT_ID('EducationalDetails')
                )
                    DROP INDEX [IX_EducationalDetails_StudentRegistrationId] ON [EducationalDetails];
            ");

            // Drop StudentExperienceCertificates if it exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID('StudentExperienceCertificates', 'U') IS NOT NULL
                    DROP TABLE [StudentExperienceCertificates];
            ");

            // Drop StudentRegistrations if it exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID('StudentRegistrations', 'U') IS NOT NULL
                    DROP TABLE [StudentRegistrations];
            ");

            // Make StudentRegistrationId nullable on EducationalDetails if column exists and is NOT NULL
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'EducationalDetails'
                      AND COLUMN_NAME = 'StudentRegistrationId'
                      AND IS_NULLABLE = 'NO'
                )
                    ALTER TABLE [EducationalDetails] ALTER COLUMN [StudentRegistrationId] int NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'EducationalDetails'
                      AND COLUMN_NAME = 'StudentRegistrationId'
                      AND IS_NULLABLE = 'YES'
                )
                    ALTER TABLE [EducationalDetails] ALTER COLUMN [StudentRegistrationId] int NOT NULL;
            ");

            migrationBuilder.CreateTable(
                name: "StudentRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    AadhaarNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CouncilEnrollmentNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FathersName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GuardianMobile = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    InstituteName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    MothersName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PassYear = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PassportPhoto = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentTransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PinCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RegistrationStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRegistrations_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentRegistrations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentExperienceCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    StudentRegistrationId = table.Column<int>(type: "int", nullable: false),
                    Certificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HospitalLabName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExperienceCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentExperienceCertificates_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentExperienceCertificates_StudentRegistrations_StudentRegistrationId",
                        column: x => x.StudentRegistrationId,
                        principalTable: "StudentRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationalDetails_StudentRegistrationId",
                table: "EducationalDetails",
                column: "StudentRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExperienceCertificates_SchoolRegistrationId",
                table: "StudentExperienceCertificates",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExperienceCertificates_StudentRegistrationId",
                table: "StudentExperienceCertificates",
                column: "StudentRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_CourseId",
                table: "StudentRegistrations",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_SchoolRegistrationId",
                table: "StudentRegistrations",
                column: "SchoolRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationalDetails_StudentRegistrations_StudentRegistrationId",
                table: "EducationalDetails",
                column: "StudentRegistrationId",
                principalTable: "StudentRegistrations",
                principalColumn: "Id");
        }
    }
}
