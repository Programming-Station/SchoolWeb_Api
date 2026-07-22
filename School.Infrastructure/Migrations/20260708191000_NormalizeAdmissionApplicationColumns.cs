using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeAdmissionApplicationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationalDetails");

            migrationBuilder.DropTable(
                name: "StudentExperienceCertificates");

            migrationBuilder.DropTable(
                name: "StudentRegistrations");

            migrationBuilder.DropColumn(
                name: "AddressInfoJson",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "ParentInfoJson",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PersonalInfoJson",
                table: "AdmissionApplications");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentsJson",
                table: "AdmissionApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AadhaarNo",
                table: "AdmissionApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "AdmissionApplications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "AdmissionApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrespondenceAddress",
                table: "AdmissionApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrespondenceCity",
                table: "AdmissionApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrespondencePinCode",
                table: "AdmissionApplications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrespondenceState",
                table: "AdmissionApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FathersName",
                table: "AdmissionApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianMobile",
                table: "AdmissionApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianName",
                table: "AdmissionApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastBoardUniversity",
                table: "AdmissionApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastGrade",
                table: "AdmissionApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastInstituteName",
                table: "AdmissionApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastObtainedMarks",
                table: "AdmissionApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPassingYear",
                table: "AdmissionApplications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPercentage",
                table: "AdmissionApplications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastQualification",
                table: "AdmissionApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastTotalMarks",
                table: "AdmissionApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "AdmissionApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MothersName",
                table: "AdmissionApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AdmissionApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentUserId",
                table: "AdmissionApplications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "AdmissionApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentCity",
                table: "AdmissionApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentCountry",
                table: "AdmissionApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentPinCode",
                table: "AdmissionApplications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentState",
                table: "AdmissionApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "AdmissionApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "AdmissionApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SameAsPermAddress",
                table: "AdmissionApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_AadhaarNo",
                table: "AdmissionApplications",
                column: "AadhaarNo",
                unique: true,
                filter: "[AadhaarNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_ApplicationNo",
                table: "AdmissionApplications",
                column: "ApplicationNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_BloodGroup",
                table: "AdmissionApplications",
                column: "BloodGroup");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_Category",
                table: "AdmissionApplications",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_Mobile",
                table: "AdmissionApplications",
                column: "Mobile");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_Status",
                table: "AdmissionApplications",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_AadhaarNo",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_ApplicationNo",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_BloodGroup",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_Category",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_Mobile",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_Status",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "AadhaarNo",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "CorrespondenceAddress",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "CorrespondenceCity",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "CorrespondencePinCode",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "CorrespondenceState",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "FathersName",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "GuardianMobile",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "GuardianName",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastBoardUniversity",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastGrade",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastInstituteName",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastObtainedMarks",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastPassingYear",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastPercentage",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastQualification",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "LastTotalMarks",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "MothersName",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "ParentUserId",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PermanentCity",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PermanentCountry",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PermanentPinCode",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PermanentState",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "AdmissionApplications");

            migrationBuilder.DropColumn(
                name: "SameAsPermAddress",
                table: "AdmissionApplications");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentsJson",
                table: "AdmissionApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressInfoJson",
                table: "AdmissionApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentInfoJson",
                table: "AdmissionApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalInfoJson",
                table: "AdmissionApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "EducationalDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    StudentRegistrationId = table.Column<int>(type: "int", nullable: false),
                    Certificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstituteAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstituteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ObtainedMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PassingYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationalDetails_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationalDetails_StudentRegistrations_StudentRegistrationId",
                        column: x => x.StudentRegistrationId,
                        principalTable: "StudentRegistrations",
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
                name: "IX_EducationalDetails_SchoolRegistrationId",
                table: "EducationalDetails",
                column: "SchoolRegistrationId");

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
        }
    }
}
