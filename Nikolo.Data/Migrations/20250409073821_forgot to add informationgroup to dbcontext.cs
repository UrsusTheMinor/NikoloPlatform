using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nikolo.Data.Migrations
{
    /// <inheritdoc />
    public partial class forgottoaddinformationgrouptodbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InformationTypes_InformationGroup_GroupId",
                table: "InformationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InformationGroup",
                table: "InformationGroup");

            migrationBuilder.RenameTable(
                name: "InformationGroup",
                newName: "InformationGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InformationGroups",
                table: "InformationGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationTypes_InformationGroups_GroupId",
                table: "InformationTypes",
                column: "GroupId",
                principalTable: "InformationGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InformationTypes_InformationGroups_GroupId",
                table: "InformationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InformationGroups",
                table: "InformationGroups");

            migrationBuilder.RenameTable(
                name: "InformationGroups",
                newName: "InformationGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InformationGroup",
                table: "InformationGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationTypes_InformationGroup_GroupId",
                table: "InformationTypes",
                column: "GroupId",
                principalTable: "InformationGroup",
                principalColumn: "Id");
        }
    }
}
