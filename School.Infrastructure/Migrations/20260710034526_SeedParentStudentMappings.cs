using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedParentStudentMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        }
    }
}
