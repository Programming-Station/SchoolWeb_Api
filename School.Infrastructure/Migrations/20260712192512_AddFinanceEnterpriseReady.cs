using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinanceEnterpriseReady : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                schema: "Finance",
                table: "JournalEntries",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                schema: "Finance",
                table: "JournalEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                schema: "Finance",
                table: "JournalEntries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                schema: "Finance",
                table: "JournalEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Finance",
                table: "JournalEntries",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoucherType",
                schema: "Finance",
                table: "JournalEntries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChequeNo",
                schema: "Finance",
                table: "CashBankTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClearedDate",
                schema: "Finance",
                table: "CashBankTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                schema: "Finance",
                table: "CashBankTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChequeBooks",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    SeriesPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartChequeNo = table.Column<int>(type: "int", nullable: false),
                    EndChequeNo = table.Column<int>(type: "int", nullable: false),
                    NextChequeNo = table.Column<int>(type: "int", nullable: false),
                    IsExhausted = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChequeBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChequeBooks_CoaAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Finance",
                        principalTable: "CoaAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChequeBooks_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCenters_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinancialYears",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialYears_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_CostCenterId",
                schema: "Finance",
                table: "JournalEntries",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBankTransactions_CostCenterId",
                schema: "Finance",
                table: "CashBankTransactions",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_ChequeBooks_BankAccountId",
                schema: "Finance",
                table: "ChequeBooks",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChequeBooks_SchoolRegistrationId",
                schema: "Finance",
                table: "ChequeBooks",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_SchoolRegistrationId",
                schema: "Finance",
                table: "CostCenters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialYears_SchoolRegistrationId",
                schema: "Finance",
                table: "FinancialYears",
                column: "SchoolRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashBankTransactions_CostCenters_CostCenterId",
                schema: "Finance",
                table: "CashBankTransactions",
                column: "CostCenterId",
                principalSchema: "Finance",
                principalTable: "CostCenters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_CostCenters_CostCenterId",
                schema: "Finance",
                table: "JournalEntries",
                column: "CostCenterId",
                principalSchema: "Finance",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashBankTransactions_CostCenters_CostCenterId",
                schema: "Finance",
                table: "CashBankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_CostCenters_CostCenterId",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropTable(
                name: "ChequeBooks",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "CostCenters",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "FinancialYears",
                schema: "Finance");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_CostCenterId",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_CashBankTransactions_CostCenterId",
                schema: "Finance",
                table: "CashBankTransactions");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "VoucherType",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ChequeNo",
                schema: "Finance",
                table: "CashBankTransactions");

            migrationBuilder.DropColumn(
                name: "ClearedDate",
                schema: "Finance",
                table: "CashBankTransactions");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                schema: "Finance",
                table: "CashBankTransactions");
        }
    }
}
