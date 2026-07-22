using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWhatsAppGateway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhatsAppAccounts",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BusinessAccountId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermanentAccessToken = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    WebhookVerifyToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebhookSecret = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BaseUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsSandbox = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_WhatsAppAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppAuditLogs",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppConversations",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MetaConversationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppConversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppDeliveryLogs",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetaMessageId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecipientPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatusTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppDeliveryLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppMediaFiles",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetaMediaId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppMediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppMessages",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MetaMessageId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppQueues",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MessagePayload = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_WhatsAppQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppTemplates",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BodyTemplate = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
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
                    table.PrimaryKey("PK_WhatsAppTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppWebhookEvents",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppAccounts_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppAccounts",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppAuditLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppAuditLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppConversations_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppConversations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppDeliveryLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppDeliveryLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppMediaFiles_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppMediaFiles",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppMessages_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppMessages",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppQueues_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppQueues",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppTemplates_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppTemplates",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppWebhookEvents_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppWebhookEvents",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhatsAppAccounts",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppAuditLogs",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppConversations",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppDeliveryLogs",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppMediaFiles",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppMessages",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppQueues",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppTemplates",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "WhatsAppWebhookEvents",
                schema: "Communication");
        }
    }
}
