using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_BloodGroupMasters_BloodGroupMasterId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_QualificationMasters_QualificationMasterId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_ReligionMasters_ReligionMasterId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_BloodGroupMasterId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_QualificationMasterId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ReligionMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AadhaarNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AlternateMobile",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BiometricID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BloodGroupMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DrivingLicense",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeNotes",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PANNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PreviousOrganization",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "QualificationMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ReligionMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Employees",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BloodGroupMasterId = table.Column<int>(type: "int", nullable: true),
                    ReligionMasterId = table.Column<int>(type: "int", nullable: true),
                    QualificationMasterId = table.Column<int>(type: "int", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AadhaarNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PANNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DrivingLicense = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AlternateMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PinCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreviousOrganization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BiometricID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDetails_BloodGroupMasters_BloodGroupMasterId",
                        column: x => x.BloodGroupMasterId,
                        principalTable: "BloodGroupMasters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeDetails_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeDetails_QualificationMasters_QualificationMasterId",
                        column: x => x.QualificationMasterId,
                        principalTable: "QualificationMasters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeDetails_ReligionMasters_ReligionMasterId",
                        column: x => x.ReligionMasterId,
                        principalTable: "ReligionMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetails_BloodGroupMasterId",
                table: "EmployeeDetails",
                column: "BloodGroupMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetails_EmployeeId",
                table: "EmployeeDetails",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetails_QualificationMasterId",
                table: "EmployeeDetails",
                column: "QualificationMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetails_ReligionMasterId",
                table: "EmployeeDetails",
                column: "ReligionMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeDetails");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "AadhaarNumber",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternateMobile",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BiometricID",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BloodGroupMasterId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicense",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNotes",
                table: "Employees",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Employees",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "Employees",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PANNumber",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousOrganization",
                table: "Employees",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QualificationMasterId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReligionMasterId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BloodGroupMasterId",
                table: "Employees",
                column: "BloodGroupMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_QualificationMasterId",
                table: "Employees",
                column: "QualificationMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReligionMasterId",
                table: "Employees",
                column: "ReligionMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_BloodGroupMasters_BloodGroupMasterId",
                table: "Employees",
                column: "BloodGroupMasterId",
                principalTable: "BloodGroupMasters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_QualificationMasters_QualificationMasterId",
                table: "Employees",
                column: "QualificationMasterId",
                principalTable: "QualificationMasters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_ReligionMasters_ReligionMasterId",
                table: "Employees",
                column: "ReligionMasterId",
                principalTable: "ReligionMasters",
                principalColumn: "Id");
        }
    }
}
