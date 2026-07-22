using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "School");

            migrationBuilder.RenameTable(
                name: "SchoolTypes",
                newName: "SchoolTypes",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "SchoolSubscriptions",
                newName: "SchoolSubscriptions",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "SchoolRegistrations",
                newName: "SchoolRegistrations",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "SchoolProfileSettings",
                newName: "SchoolProfileSettings",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "SchoolOwners",
                newName: "SchoolOwners",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "SchoolMediums",
                newName: "SchoolMediums",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "Campuses",
                newName: "Campuses",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "AffiliationBoards",
                newName: "AffiliationBoards",
                newSchema: "School");

            migrationBuilder.CreateTable(
                name: "Announcements",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentralNotifications",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ActionUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsStarred = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_CentralNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationMeetings",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MeetingLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MeetingId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MeetingPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Agenda = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MinutesOfMeeting = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                    RecordingLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetAudience = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationMeetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTemplates",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubjectTemplate = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BodyTemplate = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
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
                    table.PrimaryKey("PK_CommunicationTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatRooms",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TargetReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationProfiles",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SchoolName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CollegeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UniversityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CampusName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AffiliationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecognitionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SchoolCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CollegeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UniversityCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Board = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GSTNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PANNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UDISENumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AISHECode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrincipalName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DirectorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChairmanName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SecretaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RegistrarName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrincipalSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DigitalSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficialSeal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoundStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SquareStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FooterLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportWatermark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportBackground = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarcodePrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Favicon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SecondaryColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReportFooterText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CopyrightText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationProfiles_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalSchema: "School",
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuickPolls",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OptionsJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TargetAudience = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickPolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharedDocuments",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetAudience = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    IsPublicLink = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RaisedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AssignedStaffId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SLAExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FeedbackRating = table.Column<int>(type: "int", nullable: true),
                    FeedbackComments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatMembers",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChatMembers_GroupChatRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Communication",
                        principalTable: "GroupChatRooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupChatMessages",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    SenderUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChatMessages_GroupChatRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Communication",
                        principalTable: "GroupChatRooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PollVotes",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SelectedOption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollVotes_QuickPolls_PollId",
                        column: x => x.PollId,
                        principalSchema: "Communication",
                        principalTable: "QuickPolls",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketResponses",
                schema: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    SenderUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsInternalNote = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketResponses_SupportTickets_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "Communication",
                        principalTable: "SupportTickets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_RoomId",
                schema: "Communication",
                table: "GroupChatMembers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMessages_RoomId",
                schema: "Communication",
                table: "GroupChatMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationProfiles_SchoolRegistrationId",
                schema: "School",
                table: "OrganizationProfiles",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PollVotes_PollId",
                schema: "Communication",
                table: "PollVotes",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketResponses_TicketId",
                schema: "Communication",
                table: "TicketResponses",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "CentralNotifications",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "CommunicationMeetings",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "CommunicationTemplates",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "GroupChatMembers",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "GroupChatMessages",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "OrganizationProfiles",
                schema: "School");

            migrationBuilder.DropTable(
                name: "PollVotes",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "SharedDocuments",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "TicketResponses",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "GroupChatRooms",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "QuickPolls",
                schema: "Communication");

            migrationBuilder.DropTable(
                name: "SupportTickets",
                schema: "Communication");

            migrationBuilder.RenameTable(
                name: "SchoolTypes",
                schema: "School",
                newName: "SchoolTypes");

            migrationBuilder.RenameTable(
                name: "SchoolSubscriptions",
                schema: "School",
                newName: "SchoolSubscriptions");

            migrationBuilder.RenameTable(
                name: "SchoolRegistrations",
                schema: "School",
                newName: "SchoolRegistrations");

            migrationBuilder.RenameTable(
                name: "SchoolProfileSettings",
                schema: "School",
                newName: "SchoolProfileSettings");

            migrationBuilder.RenameTable(
                name: "SchoolOwners",
                schema: "School",
                newName: "SchoolOwners");

            migrationBuilder.RenameTable(
                name: "SchoolMediums",
                schema: "School",
                newName: "SchoolMediums");

            migrationBuilder.RenameTable(
                name: "Campuses",
                schema: "School",
                newName: "Campuses");

            migrationBuilder.RenameTable(
                name: "AffiliationBoards",
                schema: "School",
                newName: "AffiliationBoards");
        }
    }
}
