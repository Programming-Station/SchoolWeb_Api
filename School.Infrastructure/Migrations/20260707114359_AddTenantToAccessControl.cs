using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantToAccessControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "SubMenus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SubMenuName",
                table: "SubMenus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "SubMenus",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "SubMenus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "SubMenus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccesibleFor",
                table: "SubMenus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "SubMenus",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "ModulePermissions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Menus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MenuName",
                table: "Menus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MenuIcon",
                table: "Menus",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Menus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Menus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuPermessions",
                table: "MenuPermessions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MenuPermessions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MenuPermessions",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuPermessions",
                table: "MenuPermessions",
                column: "Id");

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "MenuPermessions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRegistrationId",
                table: "CategoryModules",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_SubMenus_SchoolRegistrationId",
                table: "SubMenus",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SchoolRegistrationId",
                table: "Modules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermissions_SchoolRegistrationId",
                table: "ModulePermissions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_SchoolRegistrationId",
                table: "Menus",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermessions_SchoolRegistrationId",
                table: "MenuPermessions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryModules_SchoolRegistrationId",
                table: "CategoryModules",
                column: "SchoolRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryModules_SchoolRegistrations_SchoolRegistrationId",
                table: "CategoryModules",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPermessions_SchoolRegistrations_SchoolRegistrationId",
                table: "MenuPermessions",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_SchoolRegistrations_SchoolRegistrationId",
                table: "Menus",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermissions_SchoolRegistrations_SchoolRegistrationId",
                table: "ModulePermissions",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_SchoolRegistrations_SchoolRegistrationId",
                table: "Modules",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubMenus_SchoolRegistrations_SchoolRegistrationId",
                table: "SubMenus",
                column: "SchoolRegistrationId",
                principalTable: "SchoolRegistrations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryModules_SchoolRegistrations_SchoolRegistrationId",
                table: "CategoryModules");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuPermessions_SchoolRegistrations_SchoolRegistrationId",
                table: "MenuPermessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_SchoolRegistrations_SchoolRegistrationId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermissions_SchoolRegistrations_SchoolRegistrationId",
                table: "ModulePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_SchoolRegistrations_SchoolRegistrationId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_SubMenus_SchoolRegistrations_SchoolRegistrationId",
                table: "SubMenus");

            migrationBuilder.DropIndex(
                name: "IX_SubMenus_SchoolRegistrationId",
                table: "SubMenus");

            migrationBuilder.DropIndex(
                name: "IX_Modules_SchoolRegistrationId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_ModulePermissions_SchoolRegistrationId",
                table: "ModulePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Menus_SchoolRegistrationId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_MenuPermessions_SchoolRegistrationId",
                table: "MenuPermessions");

            migrationBuilder.DropIndex(
                name: "IX_CategoryModules_SchoolRegistrationId",
                table: "CategoryModules");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "SubMenus");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "ModulePermissions");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "MenuPermessions");

            migrationBuilder.DropColumn(
                name: "SchoolRegistrationId",
                table: "CategoryModules");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "SubMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubMenuName",
                table: "SubMenus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "SubMenus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "SubMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "SubMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccesibleFor",
                table: "SubMenus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MenuName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "MenuIcon",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuPermessions",
                table: "MenuPermessions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MenuPermessions");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MenuPermessions",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuPermessions",
                table: "MenuPermessions",
                column: "Id");
        }
    }
}
