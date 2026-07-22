using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnterprisePayrollModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Payroll");

            migrationBuilder.CreateTable(
                name: "EmployeeBonuses",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BonusType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PayoutMonth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBonuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeBonuses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeBonuses_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLoans",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalInstallments = table.Column<int>(type: "int", nullable: false),
                    MonthlyInstallment = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLoans_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeLoans_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PayGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("PK_PayGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayGroups_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReimbursementClaims",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ClaimDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SettlementRef = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReimbursementClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReimbursementClaims_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReimbursementClaims_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryAdvances",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetRecoveryMonth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdvances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAdvances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalaryAdvances_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryArrears",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ArrearMonth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaidInMonth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryArrears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryArrears_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalaryArrears_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatutoryComplianceConfigs",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PfEmployerRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PfEmployeeRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PfMaxBasicLimit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EsiEmployerRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EsiEmployeeRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EsiMaxGrossLimit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProfessionalTaxSlabJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EnableGratuity = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatutoryComplianceConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatutoryComplianceConfigs_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoanRepaymentSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeLoanId = table.Column<int>(type: "int", nullable: false),
                    InstallmentNo = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrincipalComponent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InterestComponent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanRepaymentSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanRepaymentSchedules_EmployeeLoans_EmployeeLoanId",
                        column: x => x.EmployeeLoanId,
                        principalSchema: "Payroll",
                        principalTable: "EmployeeLoans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanRepaymentSchedules_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PayrollRuns",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayGroupId = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GrossSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NetSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentRef = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollRuns_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PayrollRuns_PayGroups_PayGroupId",
                        column: x => x.PayGroupId,
                        principalTable: "PayGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PayrollRuns_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryStructures",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PayGroupId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SalaryStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryStructures_PayGroups_PayGroupId",
                        column: x => x.PayGroupId,
                        principalTable: "PayGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalaryStructures_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PayrollRunDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayrollRunId = table.Column<int>(type: "int", nullable: false),
                    SalaryComponentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollRunDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollRunDetails_PayrollRuns_PayrollRunId",
                        column: x => x.PayrollRunId,
                        principalSchema: "Payroll",
                        principalTable: "PayrollRuns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PayrollRunDetails_SalaryComponents_SalaryComponentId",
                        column: x => x.SalaryComponentId,
                        principalTable: "SalaryComponents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PayrollRunDetails_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaryAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SalaryStructureId = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_EmployeeSalaryAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryAllocations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryAllocations_SalaryStructures_SalaryStructureId",
                        column: x => x.SalaryStructureId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryStructures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryAllocations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryStructureItems",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryStructureId = table.Column<int>(type: "int", nullable: false),
                    SalaryComponentId = table.Column<int>(type: "int", nullable: false),
                    CalculationType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Formula = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryStructureItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryStructureItems_SalaryComponents_SalaryComponentId",
                        column: x => x.SalaryComponentId,
                        principalTable: "SalaryComponents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalaryStructureItems_SalaryStructures_SalaryStructureId",
                        column: x => x.SalaryStructureId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryStructures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalaryStructureItems_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBonuses_EmployeeId",
                schema: "Payroll",
                table: "EmployeeBonuses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBonuses_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeBonuses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                schema: "Payroll",
                table: "EmployeeLoans",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeLoans",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryAllocations_EmployeeId",
                table: "EmployeeSalaryAllocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryAllocations_SalaryStructureId",
                table: "EmployeeSalaryAllocations",
                column: "SalaryStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryAllocations_SchoolRegistrationId",
                table: "EmployeeSalaryAllocations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRepaymentSchedules_EmployeeLoanId",
                table: "LoanRepaymentSchedules",
                column: "EmployeeLoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRepaymentSchedules_SchoolRegistrationId",
                table: "LoanRepaymentSchedules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGroups_SchoolRegistrationId",
                table: "PayGroups",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRunDetails_PayrollRunId",
                table: "PayrollRunDetails",
                column: "PayrollRunId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRunDetails_SalaryComponentId",
                table: "PayrollRunDetails",
                column: "SalaryComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRunDetails_SchoolRegistrationId",
                table: "PayrollRunDetails",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRuns_EmployeeId",
                schema: "Payroll",
                table: "PayrollRuns",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRuns_PayGroupId",
                schema: "Payroll",
                table: "PayrollRuns",
                column: "PayGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRuns_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayrollRuns",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReimbursementClaims_EmployeeId",
                schema: "Payroll",
                table: "ReimbursementClaims",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReimbursementClaims_SchoolRegistrationId",
                schema: "Payroll",
                table: "ReimbursementClaims",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdvances_EmployeeId",
                schema: "Payroll",
                table: "SalaryAdvances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdvances_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryAdvances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryArrears_EmployeeId",
                schema: "Payroll",
                table: "SalaryArrears",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryArrears_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryArrears",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureItems_SalaryComponentId",
                schema: "Payroll",
                table: "SalaryStructureItems",
                column: "SalaryComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureItems_SalaryStructureId",
                schema: "Payroll",
                table: "SalaryStructureItems",
                column: "SalaryStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureItems_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryStructureItems",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_PayGroupId",
                schema: "Payroll",
                table: "SalaryStructures",
                column: "PayGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryStructures",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StatutoryComplianceConfigs_SchoolRegistrationId",
                schema: "Payroll",
                table: "StatutoryComplianceConfigs",
                column: "SchoolRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeBonuses",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "EmployeeSalaryAllocations");

            migrationBuilder.DropTable(
                name: "LoanRepaymentSchedules");

            migrationBuilder.DropTable(
                name: "PayrollRunDetails");

            migrationBuilder.DropTable(
                name: "ReimbursementClaims",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "SalaryAdvances",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "SalaryArrears",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "SalaryStructureItems",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "StatutoryComplianceConfigs",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "EmployeeLoans",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "PayrollRuns",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "SalaryStructures",
                schema: "Payroll");

            migrationBuilder.DropTable(
                name: "PayGroups");
        }
    }
}
