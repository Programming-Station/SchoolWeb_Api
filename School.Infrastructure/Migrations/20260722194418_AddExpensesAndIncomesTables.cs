using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpensesAndIncomesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoaAccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiptUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    AcademicSessionId = table.Column<int>(type: "int", nullable: true),
                    FinancialYearId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_CoaAccounts_CoaAccountId",
                        column: x => x.CoaAccountId,
                        principalSchema: "Finance",
                        principalTable: "CoaAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalSchema: "School",
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "Inventory",
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoaAccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    AcademicSessionId = table.Column<int>(type: "int", nullable: true),
                    FinancialYearId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_CoaAccounts_CoaAccountId",
                        column: x => x.CoaAccountId,
                        principalSchema: "Finance",
                        principalTable: "CoaAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incomes_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalSchema: "School",
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CoaAccountId",
                schema: "Finance",
                table: "Expenses",
                column: "CoaAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "Expenses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_VendorId",
                schema: "Finance",
                table: "Expenses",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_CoaAccountId",
                schema: "Finance",
                table: "Incomes",
                column: "CoaAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "Incomes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "Incomes",
                schema: "Finance");
        }
    }
}
