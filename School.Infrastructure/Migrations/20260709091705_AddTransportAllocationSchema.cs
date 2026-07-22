using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransportAllocationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TransportRouteId = table.Column<int>(type: "int", nullable: false),
                    MonthlyCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportAllocations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAllocations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAllocations_TransportRoutes_TransportRouteId",
                        column: x => x.TransportRouteId,
                        principalTable: "TransportRoutes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_SchoolRegistrationId",
                table: "TransportAllocations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_StudentId",
                table: "TransportAllocations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_TransportRouteId",
                table: "TransportAllocations",
                column: "TransportRouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportAllocations");
        }
    }
}
