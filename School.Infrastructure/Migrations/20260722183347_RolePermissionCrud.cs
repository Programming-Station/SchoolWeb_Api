using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RolePermissionCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT * 
                    FROM sys.foreign_keys 
                    WHERE name = 'FK_RolePermissions_AppRoles_RoleId' 
                      AND parent_object_id = OBJECT_ID('RolePermissions')
                )
                BEGIN
                    ALTER TABLE [RolePermissions] DROP CONSTRAINT [FK_RolePermissions_AppRoles_RoleId];
                END
            ");

            migrationBuilder.Sql(@"
                IF OBJECT_ID('AppRoles', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE [AppRoles];
                END
            ");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "RolePermissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "RolePermissions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AppRoles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id");
        }
    }
}
