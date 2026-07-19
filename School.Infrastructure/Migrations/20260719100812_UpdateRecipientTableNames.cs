using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipientTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipientActivity_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientBlacklist_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientBlacklist");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientGroup_RecipientCategory_CategoryId",
                schema: "Communication",
                table: "RecipientGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientGroupMember_RecipientGroup_GroupId",
                schema: "Communication",
                table: "RecipientGroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientGroupMember_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientGroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientHistory_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientPreference_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientTag_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientTag",
                schema: "Communication",
                table: "RecipientTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientPreference",
                schema: "Communication",
                table: "RecipientPreference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientHistory",
                schema: "Communication",
                table: "RecipientHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientGroupMember",
                schema: "Communication",
                table: "RecipientGroupMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientGroup",
                schema: "Communication",
                table: "RecipientGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientCategory",
                schema: "Communication",
                table: "RecipientCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientBlacklist",
                schema: "Communication",
                table: "RecipientBlacklist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientActivity",
                schema: "Communication",
                table: "RecipientActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipient",
                schema: "Communication",
                table: "Recipient");

            migrationBuilder.RenameTable(
                name: "RecipientTag",
                schema: "Communication",
                newName: "RecipientTags",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientPreference",
                schema: "Communication",
                newName: "RecipientPreferences",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientHistory",
                schema: "Communication",
                newName: "RecipientHistories",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientGroupMember",
                schema: "Communication",
                newName: "RecipientGroupMembers",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientGroup",
                schema: "Communication",
                newName: "RecipientGroups",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientCategory",
                schema: "Communication",
                newName: "RecipientCategories",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientBlacklist",
                schema: "Communication",
                newName: "RecipientBlacklists",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientActivity",
                schema: "Communication",
                newName: "RecipientActivities",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "Recipient",
                schema: "Communication",
                newName: "Recipients",
                newSchema: "Communication");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientTag_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientTags",
                newName: "IX_RecipientTags_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientTag_RecipientId",
                schema: "Communication",
                table: "RecipientTags",
                newName: "IX_RecipientTags_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientPreference_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientPreferences",
                newName: "IX_RecipientPreferences_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientPreference_RecipientId",
                schema: "Communication",
                table: "RecipientPreferences",
                newName: "IX_RecipientPreferences_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientHistory_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientHistories",
                newName: "IX_RecipientHistories_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientHistory_RecipientId",
                schema: "Communication",
                table: "RecipientHistories",
                newName: "IX_RecipientHistories_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroupMember_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientGroupMembers",
                newName: "IX_RecipientGroupMembers_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroupMember_RecipientId",
                schema: "Communication",
                table: "RecipientGroupMembers",
                newName: "IX_RecipientGroupMembers_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroupMember_GroupId",
                schema: "Communication",
                table: "RecipientGroupMembers",
                newName: "IX_RecipientGroupMembers_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroup_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientGroups",
                newName: "IX_RecipientGroups_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroup_CategoryId",
                schema: "Communication",
                table: "RecipientGroups",
                newName: "IX_RecipientGroups_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientCategory_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientCategories",
                newName: "IX_RecipientCategories_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientBlacklist_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientBlacklists",
                newName: "IX_RecipientBlacklists_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientBlacklist_RecipientId",
                schema: "Communication",
                table: "RecipientBlacklists",
                newName: "IX_RecipientBlacklists_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientActivity_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientActivities",
                newName: "IX_RecipientActivities_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientActivity_RecipientId",
                schema: "Communication",
                table: "RecipientActivities",
                newName: "IX_RecipientActivities_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipient_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "Recipients",
                newName: "IX_Recipients_SchoolRegistrationId_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientTags",
                schema: "Communication",
                table: "RecipientTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientPreferences",
                schema: "Communication",
                table: "RecipientPreferences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientHistories",
                schema: "Communication",
                table: "RecipientHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientGroupMembers",
                schema: "Communication",
                table: "RecipientGroupMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientGroups",
                schema: "Communication",
                table: "RecipientGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientCategories",
                schema: "Communication",
                table: "RecipientCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientBlacklists",
                schema: "Communication",
                table: "RecipientBlacklists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientActivities",
                schema: "Communication",
                table: "RecipientActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipients",
                schema: "Communication",
                table: "Recipients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientActivities_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientActivities",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientBlacklists_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientBlacklists",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientGroupMembers_RecipientGroups_GroupId",
                schema: "Communication",
                table: "RecipientGroupMembers",
                column: "GroupId",
                principalSchema: "Communication",
                principalTable: "RecipientGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientGroupMembers_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientGroupMembers",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientGroups_RecipientCategories_CategoryId",
                schema: "Communication",
                table: "RecipientGroups",
                column: "CategoryId",
                principalSchema: "Communication",
                principalTable: "RecipientCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientHistories_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientHistories",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientPreferences_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientPreferences",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientTags_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientTags",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipientActivities_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientBlacklists_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientBlacklists");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientGroupMembers_RecipientGroups_GroupId",
                schema: "Communication",
                table: "RecipientGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientGroupMembers_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientGroups_RecipientCategories_CategoryId",
                schema: "Communication",
                table: "RecipientGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientHistories_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientPreferences_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientTags_Recipients_RecipientId",
                schema: "Communication",
                table: "RecipientTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientTags",
                schema: "Communication",
                table: "RecipientTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipients",
                schema: "Communication",
                table: "Recipients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientPreferences",
                schema: "Communication",
                table: "RecipientPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientHistories",
                schema: "Communication",
                table: "RecipientHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientGroups",
                schema: "Communication",
                table: "RecipientGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientGroupMembers",
                schema: "Communication",
                table: "RecipientGroupMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientCategories",
                schema: "Communication",
                table: "RecipientCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientBlacklists",
                schema: "Communication",
                table: "RecipientBlacklists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientActivities",
                schema: "Communication",
                table: "RecipientActivities");

            migrationBuilder.RenameTable(
                name: "RecipientTags",
                schema: "Communication",
                newName: "RecipientTag",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "Recipients",
                schema: "Communication",
                newName: "Recipient",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientPreferences",
                schema: "Communication",
                newName: "RecipientPreference",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientHistories",
                schema: "Communication",
                newName: "RecipientHistory",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientGroups",
                schema: "Communication",
                newName: "RecipientGroup",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientGroupMembers",
                schema: "Communication",
                newName: "RecipientGroupMember",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientCategories",
                schema: "Communication",
                newName: "RecipientCategory",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientBlacklists",
                schema: "Communication",
                newName: "RecipientBlacklist",
                newSchema: "Communication");

            migrationBuilder.RenameTable(
                name: "RecipientActivities",
                schema: "Communication",
                newName: "RecipientActivity",
                newSchema: "Communication");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientTags_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientTag",
                newName: "IX_RecipientTag_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientTags_RecipientId",
                schema: "Communication",
                table: "RecipientTag",
                newName: "IX_RecipientTag_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipients_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "Recipient",
                newName: "IX_Recipient_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientPreferences_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientPreference",
                newName: "IX_RecipientPreference_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientPreferences_RecipientId",
                schema: "Communication",
                table: "RecipientPreference",
                newName: "IX_RecipientPreference_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientHistories_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientHistory",
                newName: "IX_RecipientHistory_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientHistories_RecipientId",
                schema: "Communication",
                table: "RecipientHistory",
                newName: "IX_RecipientHistory_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroups_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientGroup",
                newName: "IX_RecipientGroup_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroups_CategoryId",
                schema: "Communication",
                table: "RecipientGroup",
                newName: "IX_RecipientGroup_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroupMembers_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientGroupMember",
                newName: "IX_RecipientGroupMember_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroupMembers_RecipientId",
                schema: "Communication",
                table: "RecipientGroupMember",
                newName: "IX_RecipientGroupMember_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientGroupMembers_GroupId",
                schema: "Communication",
                table: "RecipientGroupMember",
                newName: "IX_RecipientGroupMember_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientCategories_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientCategory",
                newName: "IX_RecipientCategory_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientBlacklists_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientBlacklist",
                newName: "IX_RecipientBlacklist_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientBlacklists_RecipientId",
                schema: "Communication",
                table: "RecipientBlacklist",
                newName: "IX_RecipientBlacklist_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientActivities_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "RecipientActivity",
                newName: "IX_RecipientActivity_SchoolRegistrationId_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_RecipientActivities_RecipientId",
                schema: "Communication",
                table: "RecipientActivity",
                newName: "IX_RecipientActivity_RecipientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientTag",
                schema: "Communication",
                table: "RecipientTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipient",
                schema: "Communication",
                table: "Recipient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientPreference",
                schema: "Communication",
                table: "RecipientPreference",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientHistory",
                schema: "Communication",
                table: "RecipientHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientGroup",
                schema: "Communication",
                table: "RecipientGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientGroupMember",
                schema: "Communication",
                table: "RecipientGroupMember",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientCategory",
                schema: "Communication",
                table: "RecipientCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientBlacklist",
                schema: "Communication",
                table: "RecipientBlacklist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientActivity",
                schema: "Communication",
                table: "RecipientActivity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientActivity_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientActivity",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientBlacklist_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientBlacklist",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientGroup_RecipientCategory_CategoryId",
                schema: "Communication",
                table: "RecipientGroup",
                column: "CategoryId",
                principalSchema: "Communication",
                principalTable: "RecipientCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientGroupMember_RecipientGroup_GroupId",
                schema: "Communication",
                table: "RecipientGroupMember",
                column: "GroupId",
                principalSchema: "Communication",
                principalTable: "RecipientGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientGroupMember_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientGroupMember",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientHistory_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientHistory",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientPreference_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientPreference",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientTag_Recipient_RecipientId",
                schema: "Communication",
                table: "RecipientTag",
                column: "RecipientId",
                principalSchema: "Communication",
                principalTable: "Recipient",
                principalColumn: "Id");
        }
    }
}
