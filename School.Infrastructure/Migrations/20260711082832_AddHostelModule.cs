using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHostelModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Hostel");

            migrationBuilder.CreateTable(
                name: "HostelAttendances",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_HostelAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelAttendances_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelComplaints",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedStaffId = table.Column<int>(type: "int", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResolutionDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelComplaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelComplaints_Employees_AssignedStaffId",
                        column: x => x.AssignedStaffId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelComplaints_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelComplaints_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelDisciplines",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Offense = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionTaken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WardenRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelDisciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelDisciplines_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelDisciplines_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hostels",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HostelType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BuildingCount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GeoLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hostels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hostels_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelVisitors",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    VisitorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Relation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdProofType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdProofNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_HostelVisitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelVisitors_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelVisitors_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LaundryTransactions",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TokenNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: false),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDelivery = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Charges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_LaundryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaundryTransactions_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LaundryTransactions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealAttendances",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScannedVia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TokenNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealAttendances_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MealAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomCategories",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAc = table.Column<bool>(type: "bit", nullable: false),
                    HasAttachedBathroom = table.Column<bool>(type: "bit", nullable: false),
                    HasWifi = table.Column<bool>(type: "bit", nullable: false),
                    DefaultFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomCategories_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelMaintenances",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplaintId = table.Column<int>(type: "int", nullable: true),
                    AssetId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TechnicianName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelMaintenances_HostelComplaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalSchema: "Hostel",
                        principalTable: "HostelComplaints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelMaintenances_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumberOfFloors = table.Column<int>(type: "int", nullable: false),
                    ConstructionYear = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Hostels_HostelId",
                        column: x => x.HostelId,
                        principalSchema: "Hostel",
                        principalTable: "Hostels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Buildings_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelWardens",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    HostelId = table.Column<int>(type: "int", nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_HostelWardens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelWardens_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelWardens_Hostels_HostelId",
                        column: x => x.HostelId,
                        principalSchema: "Hostel",
                        principalTable: "Hostels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelWardens_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessMenus",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostelId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FoodItems = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SpecialItems = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessMenus_Hostels_HostelId",
                        column: x => x.HostelId,
                        principalSchema: "Hostel",
                        principalTable: "Hostels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessMenus_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Floors",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Floors_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalSchema: "Hostel",
                        principalTable: "Buildings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Floors_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HostelId = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    FloorId = table.Column<int>(type: "int", nullable: false),
                    RoomCategoryId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    OccupiedBeds = table.Column<int>(type: "int", nullable: false),
                    AvailableBeds = table.Column<int>(type: "int", nullable: false),
                    FurnitureDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalSchema: "Hostel",
                        principalTable: "Buildings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_Floors_FloorId",
                        column: x => x.FloorId,
                        principalSchema: "Hostel",
                        principalTable: "Floors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_Hostels_HostelId",
                        column: x => x.HostelId,
                        principalSchema: "Hostel",
                        principalTable: "Hostels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_RoomCategories_RoomCategoryId",
                        column: x => x.RoomCategoryId,
                        principalSchema: "Hostel",
                        principalTable: "RoomCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Beds",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    BedNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LockerNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CupboardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MattressNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RfidTag = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CleaningStatus = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Hostel",
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Beds_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelInventories",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AssetTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelInventories_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Hostel",
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelInventories_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelMedicalLogs",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    IncidentDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Temperature = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DoctorVisited = table.Column<bool>(type: "bit", nullable: false),
                    MedicinesGiven = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsolationRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsolationRoomId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelMedicalLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelMedicalLogs_Rooms_IsolationRoomId",
                        column: x => x.IsolationRoomId,
                        principalSchema: "Hostel",
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelMedicalLogs_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelMedicalLogs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BedReservations",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_BedReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedReservations_Beds_BedId",
                        column: x => x.BedId,
                        principalSchema: "Hostel",
                        principalTable: "Beds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BedReservations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BedReservations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelAdmissions",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    HostelId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    AdmissionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualCheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SecurityDepositRefunded = table.Column<bool>(type: "bit", nullable: false),
                    DocumentsUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MedicalDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SpecialNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BiometricId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RfidTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelAdmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelAdmissions_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelAdmissions_Beds_BedId",
                        column: x => x.BedId,
                        principalSchema: "Hostel",
                        principalTable: "Beds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelAdmissions_Hostels_HostelId",
                        column: x => x.HostelId,
                        principalSchema: "Hostel",
                        principalTable: "Hostels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelAdmissions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Hostel",
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelAdmissions_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelAdmissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomTransferHistories",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FromRoomId = table.Column<int>(type: "int", nullable: false),
                    ToRoomId = table.Column<int>(type: "int", nullable: false),
                    FromBedId = table.Column<int>(type: "int", nullable: false),
                    ToBedId = table.Column<int>(type: "int", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTransferHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTransferHistories_Beds_FromBedId",
                        column: x => x.FromBedId,
                        principalSchema: "Hostel",
                        principalTable: "Beds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomTransferHistories_Beds_ToBedId",
                        column: x => x.ToBedId,
                        principalSchema: "Hostel",
                        principalTable: "Beds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomTransferHistories_Rooms_FromRoomId",
                        column: x => x.FromRoomId,
                        principalSchema: "Hostel",
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomTransferHistories_Rooms_ToRoomId",
                        column: x => x.ToRoomId,
                        principalSchema: "Hostel",
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomTransferHistories_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomTransferHistories_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelFeeAllocations",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmissionId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FeePlanName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_HostelFeeAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelFeeAllocations_HostelAdmissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalSchema: "Hostel",
                        principalTable: "HostelAdmissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelFeeAllocations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelFeeAllocations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelGatePasses",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmissionId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    OutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedReturn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    WardenApproval = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParentApproval = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_HostelGatePasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelGatePasses_HostelAdmissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalSchema: "Hostel",
                        principalTable: "HostelAdmissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelGatePasses_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelGatePasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HostelFeePayments",
                schema: "Hostel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocationId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_HostelFeePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostelFeePayments_HostelFeeAllocations_AllocationId",
                        column: x => x.AllocationId,
                        principalSchema: "Hostel",
                        principalTable: "HostelFeeAllocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HostelFeePayments_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BedReservations_BedId",
                schema: "Hostel",
                table: "BedReservations",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_BedReservations_SchoolRegistrationId",
                schema: "Hostel",
                table: "BedReservations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BedReservations_StudentId",
                schema: "Hostel",
                table: "BedReservations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_RoomId",
                schema: "Hostel",
                table: "Beds",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_SchoolRegistrationId",
                schema: "Hostel",
                table: "Beds",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_HostelId",
                schema: "Hostel",
                table: "Buildings",
                column: "HostelId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_SchoolRegistrationId",
                schema: "Hostel",
                table: "Buildings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_BuildingId",
                schema: "Hostel",
                table: "Floors",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_SchoolRegistrationId",
                schema: "Hostel",
                table: "Floors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_AcademicYearId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_BedId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_HostelId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "HostelId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_RoomId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_StudentId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAttendances_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelAttendances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAttendances_StudentId",
                schema: "Hostel",
                table: "HostelAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelComplaints_AssignedStaffId",
                schema: "Hostel",
                table: "HostelComplaints",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelComplaints_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelComplaints",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelComplaints_StudentId",
                schema: "Hostel",
                table: "HostelComplaints",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelDisciplines_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelDisciplines",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelDisciplines_StudentId",
                schema: "Hostel",
                table: "HostelDisciplines",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeeAllocations_AdmissionId",
                schema: "Hostel",
                table: "HostelFeeAllocations",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeeAllocations_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelFeeAllocations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeeAllocations_StudentId",
                schema: "Hostel",
                table: "HostelFeeAllocations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeePayments_AllocationId",
                schema: "Hostel",
                table: "HostelFeePayments",
                column: "AllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeePayments_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelFeePayments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelGatePasses_AdmissionId",
                schema: "Hostel",
                table: "HostelGatePasses",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelGatePasses_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelGatePasses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelGatePasses_StudentId",
                schema: "Hostel",
                table: "HostelGatePasses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelInventories_RoomId",
                schema: "Hostel",
                table: "HostelInventories",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelInventories_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelInventories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMaintenances_ComplaintId",
                schema: "Hostel",
                table: "HostelMaintenances",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMaintenances_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelMaintenances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMedicalLogs_IsolationRoomId",
                schema: "Hostel",
                table: "HostelMedicalLogs",
                column: "IsolationRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMedicalLogs_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelMedicalLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMedicalLogs_StudentId",
                schema: "Hostel",
                table: "HostelMedicalLogs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Hostels_SchoolRegistrationId",
                schema: "Hostel",
                table: "Hostels",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelVisitors_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelVisitors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelVisitors_StudentId",
                schema: "Hostel",
                table: "HostelVisitors",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelWardens_EmployeeId",
                schema: "Hostel",
                table: "HostelWardens",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelWardens_HostelId",
                schema: "Hostel",
                table: "HostelWardens",
                column: "HostelId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelWardens_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelWardens",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LaundryTransactions_SchoolRegistrationId",
                schema: "Hostel",
                table: "LaundryTransactions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LaundryTransactions_StudentId",
                schema: "Hostel",
                table: "LaundryTransactions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAttendances_SchoolRegistrationId",
                schema: "Hostel",
                table: "MealAttendances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAttendances_StudentId",
                schema: "Hostel",
                table: "MealAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MessMenus_HostelId",
                schema: "Hostel",
                table: "MessMenus",
                column: "HostelId");

            migrationBuilder.CreateIndex(
                name: "IX_MessMenus_SchoolRegistrationId",
                schema: "Hostel",
                table: "MessMenus",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategories_SchoolRegistrationId",
                schema: "Hostel",
                table: "RoomCategories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                schema: "Hostel",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FloorId",
                schema: "Hostel",
                table: "Rooms",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HostelId",
                schema: "Hostel",
                table: "Rooms",
                column: "HostelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomCategoryId",
                schema: "Hostel",
                table: "Rooms",
                column: "RoomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SchoolRegistrationId",
                schema: "Hostel",
                table: "Rooms",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_FromBedId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "FromBedId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_FromRoomId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "FromRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_SchoolRegistrationId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_StudentId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_ToBedId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "ToBedId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_ToRoomId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "ToRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BedReservations",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelAttendances",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelDisciplines",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelFeePayments",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelGatePasses",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelInventories",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelMaintenances",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelMedicalLogs",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelVisitors",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelWardens",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "LaundryTransactions",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "MealAttendances",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "MessMenus",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "RoomTransferHistories",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelFeeAllocations",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelComplaints",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "HostelAdmissions",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "Beds",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "Floors",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "RoomCategories",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "Buildings",
                schema: "Hostel");

            migrationBuilder.DropTable(
                name: "Hostels",
                schema: "Hostel");
        }
    }
}
