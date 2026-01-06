using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Enquiry_tables_modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnquiryFromNo",
                table: "Enquiries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_CourseId",
                table: "Enquiries",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_Courses_CourseId",
                table: "Enquiries",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_Courses_CourseId",
                table: "Enquiries");

            migrationBuilder.DropIndex(
                name: "IX_Enquiries_CourseId",
                table: "Enquiries");

            migrationBuilder.AlterColumn<string>(
                name: "EnquiryFromNo",
                table: "Enquiries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
