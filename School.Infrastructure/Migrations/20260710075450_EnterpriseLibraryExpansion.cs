using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnterpriseLibraryExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubjectCategory",
                table: "LibraryBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RackLocation",
                table: "LibraryBooks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "LibraryBooks",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "AccessionNumber",
                table: "LibraryBooks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "LibraryBooks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "LibraryBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookImagePath",
                table: "LibraryBooks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookType",
                table: "LibraryBooks",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "LibraryBooks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoAuthor",
                table: "LibraryBooks",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cupboard",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LibraryBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "LibraryBooks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "LibraryBooks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumStock",
                table: "LibraryBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "LibraryBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PdfAttachmentPath",
                table: "LibraryBooks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "LibraryBooks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "LibraryBooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "LibraryBooks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "LibraryBooks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rack",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Row",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shelf",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "LibraryBooks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Volume",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookConditionOnIssue",
                table: "BookIssueLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookConditionOnReturn",
                table: "BookIssueLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FinePaid",
                table: "BookIssueLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRenewed",
                table: "BookIssueLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "BookIssueLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OriginalDueDate",
                table: "BookIssueLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "BookIssueLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RenewalCount",
                table: "BookIssueLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BooksCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BookAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookAuthors_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CategoryType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_BookCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCategories_BookCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "BookCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookCategories_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookPublishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GSTNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_BookPublishers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookPublishers_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookVendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GSTNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_BookVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookVendors_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DigitalResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StreamingUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DownloadAllowed = table.Column<bool>(type: "bit", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    SubjectCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_DigitalResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalResources_LibraryBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "LibraryBooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DigitalResources_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BorrowLimit = table.Column<int>(type: "int", nullable: false),
                    CurrentBorrowCount = table.Column<int>(type: "int", nullable: false),
                    MembershipBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MembershipQRCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_LibraryMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryMembers_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryMembers_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryFineRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PerDayFine = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxFine = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GraceDays = table.Column<int>(type: "int", nullable: false),
                    HolidayExemption = table.Column<bool>(type: "bit", nullable: false),
                    CategoryWise = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryFineRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryFineRules_BookCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BookCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryFineRules_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueuePosition = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NotifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_BookReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookReservations_LibraryBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "LibraryBooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookReservations_LibraryMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "LibraryMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookReservations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_AuthorId",
                table: "LibraryBooks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_CategoryId",
                table: "LibraryBooks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_PublisherId",
                table: "LibraryBooks",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_VendorId",
                table: "LibraryBooks",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIssueLogs_MemberId",
                table: "BookIssueLogs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_SchoolRegistrationId",
                table: "BookAuthors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_ParentCategoryId",
                table: "BookCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_SchoolRegistrationId",
                table: "BookCategories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookPublishers_SchoolRegistrationId",
                table: "BookPublishers",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_BookId",
                table: "BookReservations",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_MemberId",
                table: "BookReservations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_SchoolRegistrationId",
                table: "BookReservations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookVendors_SchoolRegistrationId",
                table: "BookVendors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalResources_BookId",
                table: "DigitalResources",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalResources_SchoolRegistrationId",
                table: "DigitalResources",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFineRules_CategoryId",
                table: "LibraryFineRules",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFineRules_SchoolRegistrationId",
                table: "LibraryFineRules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMembers_SchoolRegistrationId",
                table: "LibraryMembers",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMembers_StudentId",
                table: "LibraryMembers",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookIssueLogs_LibraryMembers_MemberId",
                table: "BookIssueLogs",
                column: "MemberId",
                principalTable: "LibraryMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryBooks_BookAuthors_AuthorId",
                table: "LibraryBooks",
                column: "AuthorId",
                principalTable: "BookAuthors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryBooks_BookCategories_CategoryId",
                table: "LibraryBooks",
                column: "CategoryId",
                principalTable: "BookCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryBooks_BookPublishers_PublisherId",
                table: "LibraryBooks",
                column: "PublisherId",
                principalTable: "BookPublishers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryBooks_BookVendors_VendorId",
                table: "LibraryBooks",
                column: "VendorId",
                principalTable: "BookVendors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookIssueLogs_LibraryMembers_MemberId",
                table: "BookIssueLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryBooks_BookAuthors_AuthorId",
                table: "LibraryBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryBooks_BookCategories_CategoryId",
                table: "LibraryBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryBooks_BookPublishers_PublisherId",
                table: "LibraryBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryBooks_BookVendors_VendorId",
                table: "LibraryBooks");

            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookPublishers");

            migrationBuilder.DropTable(
                name: "BookReservations");

            migrationBuilder.DropTable(
                name: "BookVendors");

            migrationBuilder.DropTable(
                name: "DigitalResources");

            migrationBuilder.DropTable(
                name: "LibraryFineRules");

            migrationBuilder.DropTable(
                name: "LibraryMembers");

            migrationBuilder.DropTable(
                name: "BookCategories");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_AuthorId",
                table: "LibraryBooks");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_CategoryId",
                table: "LibraryBooks");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_PublisherId",
                table: "LibraryBooks");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_VendorId",
                table: "LibraryBooks");

            migrationBuilder.DropIndex(
                name: "IX_BookIssueLogs_MemberId",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "AccessionNumber",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "BookImagePath",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "BookType",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "CoAuthor",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Cupboard",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "MaximumStock",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "PdfAttachmentPath",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Rack",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Shelf",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "LibraryBooks");

            migrationBuilder.DropColumn(
                name: "BookConditionOnIssue",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "BookConditionOnReturn",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "FinePaid",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "IsRenewed",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "OriginalDueDate",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "BookIssueLogs");

            migrationBuilder.DropColumn(
                name: "RenewalCount",
                table: "BookIssueLogs");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectCategory",
                table: "LibraryBooks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RackLocation",
                table: "LibraryBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "LibraryBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
