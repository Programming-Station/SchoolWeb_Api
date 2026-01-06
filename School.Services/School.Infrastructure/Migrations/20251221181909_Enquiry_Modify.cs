using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Enquiry_Modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Enquiries");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Enquiries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_StatusId",
                table: "Enquiries",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_Statuses_StatusId",
                table: "Enquiries",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_Statuses_StatusId",
                table: "Enquiries");

            migrationBuilder.DropIndex(
                name: "IX_Enquiries_StatusId",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Enquiries");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Enquiries",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
