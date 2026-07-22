using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentStudentMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPrimaryGuardian = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentStudentMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentStudentMappings_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentStudentMappings_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentStudentMappings_Users_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudentMappings_ParentUserId_StudentId",
                table: "ParentStudentMappings",
                columns: new[] { "ParentUserId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudentMappings_SchoolRegistrationId",
                table: "ParentStudentMappings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudentMappings_StudentId",
                table: "ParentStudentMappings",
                column: "StudentId");

            // Seed existing parent-student relationships from AdmissionApplications
            migrationBuilder.Sql(@"
                INSERT INTO ParentStudentMappings 
                    (ParentUserId, StudentId, SchoolRegistrationId, Relationship, IsPrimaryGuardian, IsDeleted, CreatedDate, CreatedBy)
                SELECT DISTINCT
                    aa.ParentUserId,
                    s.Id,
                    aa.SchoolRegistrationId,
                    'Parent',
                    1,
                    0,
                    GETUTCDATE(),
                    'System_Migration'
                FROM AdmissionApplications aa
                INNER JOIN Students s ON s.ApplicationUserId = aa.StudentUserId
                WHERE aa.ParentUserId IS NOT NULL
                  AND aa.ParentUserId <> ''
                  AND aa.StudentUserId IS NOT NULL
                  AND aa.StudentUserId <> ''
                  AND NOT EXISTS (
                      SELECT 1 FROM ParentStudentMappings psm
                      WHERE psm.ParentUserId = aa.ParentUserId
                        AND psm.StudentId = s.Id
                  )
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentStudentMappings");
        }
    }
}
