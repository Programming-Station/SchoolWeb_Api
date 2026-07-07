using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreHrMasters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Qualification",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "BloodGroupMasterId",
                table: "Employees",
                type: "int",
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

            migrationBuilder.CreateTable(
                name: "BloodGroupMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodGroupMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodGroupMasters_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QualificationMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualificationMasters_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReligionMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligionMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligionMasters_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BloodGroupMasters_SchoolRegistrationId",
                table: "BloodGroupMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMasters_SchoolRegistrationId",
                table: "QualificationMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligionMasters_SchoolRegistrationId",
                table: "ReligionMasters",
                column: "SchoolRegistrationId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "BloodGroupMasters");

            migrationBuilder.DropTable(
                name: "QualificationMasters");

            migrationBuilder.DropTable(
                name: "ReligionMasters");

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
                name: "BloodGroupMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "QualificationMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ReligionMasterId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "Employees",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
