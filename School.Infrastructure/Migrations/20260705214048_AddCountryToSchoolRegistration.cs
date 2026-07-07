using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryToSchoolRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "SchoolRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Update existing SchoolRegistrations to point to a valid country
            migrationBuilder.Sql("UPDATE SchoolRegistrations SET CountryId = (SELECT TOP 1 Id FROM Countries) WHERE CountryId = 0 OR CountryId NOT IN (SELECT Id FROM Countries)");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolRegistrations_CountryId",
                table: "SchoolRegistrations",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolRegistrations_Countries_CountryId",
                table: "SchoolRegistrations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolRegistrations_Countries_CountryId",
                table: "SchoolRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_SchoolRegistrations_CountryId",
                table: "SchoolRegistrations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "SchoolRegistrations");
        }
    }
}
