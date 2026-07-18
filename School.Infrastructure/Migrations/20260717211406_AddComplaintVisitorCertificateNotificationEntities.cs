using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintVisitorCertificateNotificationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificateIssuanceLogs",
                schema: "Administration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CertificateType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdmissionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IssuedByName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PdfUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QrVerificationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrintCount = table.Column<int>(type: "int", nullable: false),
                    LastPrintedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RevocationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateIssuanceLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                schema: "Administration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplaintNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RaisedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RaisedByRole = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AssignedToName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolutionDetails = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ResolvedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EscalationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FeedbackRating = table.Column<int>(type: "int", nullable: true),
                    FeedbackComments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ActionUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPushed = table.Column<bool>(type: "bit", nullable: false),
                    PushedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SenderUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                schema: "Administration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VisitorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdProofType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdProofNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdProofUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PersonToMeet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    NumberOfVisitors = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VisitorBadgeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateIssuanceLogs_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "CertificateIssuanceLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "Complaints",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "NotificationLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "Visitors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateIssuanceLogs",
                schema: "Administration");

            migrationBuilder.DropTable(
                name: "Complaints",
                schema: "Administration");

            migrationBuilder.DropTable(
                name: "NotificationLogs",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "Visitors",
                schema: "Administration");
        }
    }
}
