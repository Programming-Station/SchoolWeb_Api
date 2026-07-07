using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "Faculties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_SchoolRegistrationId",
                table: "Faculties",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SchoolRegistrationId",
                table: "Departments",
                column: "SchoolRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_SchoolRegistrations_SchoolRegistrationId",
                table: "Departments",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_SchoolRegistrations_SchoolRegistrationId",
                table: "Faculties",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_SchoolRegistrations_SchoolRegistrationId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_SchoolRegistrations_SchoolRegistrationId",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_SchoolRegistrationId",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Departments_SchoolRegistrationId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "Departments");
        }
    }
}
