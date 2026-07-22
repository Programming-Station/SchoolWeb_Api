using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailRecipientAttachmentAndSchemaMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributionList",
                schema: "Communication",
                table: "DistributionList");

            migrationBuilder.RenameTable(
                name: "EmailTemplates",
                newName: "EmailTemplates",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "EmailServerSettings",
                newName: "EmailServerSettings",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "EmailLogs",
                newName: "EmailLogs",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "EmailBrandings",
                newName: "EmailBrandings",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "DistributionList",
                schema: "Communication",
                newName: "DistributionLists",
                newSchema: "Communication");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionList_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "DistributionLists",
                newName: "IX_DistributionLists_SchoolRegistrationId_IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                schema: "Communication",
                table: "EmailLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScheduled",
                schema: "Communication",
                table: "EmailLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledTime",
                schema: "Communication",
                table: "EmailLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributionLists",
                schema: "Communication",
                table: "DistributionLists",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailAttachments",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    EmailLogId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
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
                    table.PrimaryKey("PK_EmailAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAttachments_EmailLogs_EmailLogId",
                        column: x => x.EmailLogId,
                        principalSchema: "Communication",
                        principalTable: "EmailLogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailAttachments_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalSchema: "School",
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailRecipients",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    EmailMessageId = table.Column<int>(type: "int", nullable: false),
                    AddressBookId = table.Column<int>(type: "int", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_EmailRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailRecipients_EmailLogs_EmailMessageId",
                        column: x => x.EmailMessageId,
                        principalSchema: "Communication",
                        principalTable: "EmailLogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailRecipients_Recipients_AddressBookId",
                        column: x => x.AddressBookId,
                        principalSchema: "Communication",
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailRecipients_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalSchema: "School",
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachments_EmailLogId",
                schema: "Communication",
                table: "EmailAttachments",
                column: "EmailLogId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachments_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "EmailAttachments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipients_AddressBookId",
                schema: "Communication",
                table: "EmailRecipients",
                column: "AddressBookId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipients_EmailMessageId",
                schema: "Communication",
                table: "EmailRecipients",
                column: "EmailMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipients_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "EmailRecipients",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAttachments",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "EmailRecipients",
                schema: "Communication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributionLists",
                schema: "Communication",
                table: "DistributionLists");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                schema: "Communication",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "IsScheduled",
                schema: "Communication",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "ScheduledTime",
                schema: "Communication",
                table: "EmailLogs");

            migrationBuilder.RenameTable(
                name: "EmailTemplates",
                schema: "Communication",
                newName: "EmailTemplates");

            migrationBuilder.RenameTable(
                name: "EmailServerSettings",
                schema: "Communication",
                newName: "EmailServerSettings");

            migrationBuilder.RenameTable(
                name: "EmailLogs",
                schema: "Communication",
                newName: "EmailLogs");

            migrationBuilder.RenameTable(
                name: "EmailBrandings",
                schema: "Communication",
                newName: "EmailBrandings");

            migrationBuilder.RenameTable(
                name: "DistributionLists",
                schema: "Communication",
                newName: "DistributionList",
                newSchema: "Communication");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionLists_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "DistributionList",
                newName: "IX_DistributionList_SchoolRegistrationId_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributionList",
                schema: "Communication",
                table: "DistributionList",
                column: "Id");
        }
    }
}
