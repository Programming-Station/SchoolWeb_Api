using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrganizationProfileBrandingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccentColor",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountantName",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "School",
                table: "OrganizationProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChairmanSignature",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentAcademicSession",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DarkTheme",
                schema: "School",
                table: "OrganizationProfiles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateFormat",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Disclaimer",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailLogo",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialYear",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontFamily",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontSize",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HelpdeskEmail",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Landmark",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LightTheme",
                schema: "School",
                table: "OrganizationProfiles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginLogo",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoDark",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoLight",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileAppIcon",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PDFLogo",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RectangleSeal",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrarSignature",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoundSeal",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SplashScreenLogo",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportPhone",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TANNumber",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telegram",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeFormat",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VicePrincipalName",
                schema: "School",
                table: "OrganizationProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccentColor",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "AccountantName",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "ChairmanSignature",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "CurrentAcademicSession",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "DarkTheme",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "DateFormat",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "Disclaimer",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "EmailLogo",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "FinancialYear",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "FontFamily",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "FontSize",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "HelpdeskEmail",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "Landmark",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "LightTheme",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "LoginLogo",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "LogoDark",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "LogoLight",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "MobileAppIcon",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "PDFLogo",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "RectangleSeal",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "RegistrarSignature",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "RoundSeal",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "SplashScreenLogo",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "SupportPhone",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "TANNumber",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "Telegram",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "TimeFormat",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "VicePrincipalName",
                schema: "School",
                table: "OrganizationProfiles");
        }
    }
}
