using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentGateway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsResultPublished",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResultPublishedBy",
                table: "Exams",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResultPublishedDate",
                table: "Exams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OnlinePaymentOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FeeInstallmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlinePaymentOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlinePaymentOrders_FeeInstallments_FeeInstallmentId",
                        column: x => x.FeeInstallmentId,
                        principalTable: "FeeInstallments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnlinePaymentOrders_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnlinePaymentOrders_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentGateways",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GatewayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SecretKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WebhookSecret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGateways", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentGateways_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePaymentOrders_FeeInstallmentId",
                table: "OnlinePaymentOrders",
                column: "FeeInstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePaymentOrders_SchoolRegistrationId",
                table: "OnlinePaymentOrders",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePaymentOrders_StudentId",
                table: "OnlinePaymentOrders",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGateways_SchoolRegistrationId",
                table: "PaymentGateways",
                column: "SchoolRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlinePaymentOrders");

            migrationBuilder.DropTable(
                name: "PaymentGateways");

            migrationBuilder.DropColumn(
                name: "IsResultPublished",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ResultPublishedBy",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ResultPublishedDate",
                table: "Exams");
        }
    }
}
