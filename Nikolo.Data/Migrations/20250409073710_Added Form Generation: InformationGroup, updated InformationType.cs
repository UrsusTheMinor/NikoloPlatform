using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nikolo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFormGenerationInformationGroupupdatedInformationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "InformationTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "InformationTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "InformationTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InformationGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElementsPerRow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InformationTypes_GroupId",
                table: "InformationTypes",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationTypes_InformationGroup_GroupId",
                table: "InformationTypes",
                column: "GroupId",
                principalTable: "InformationGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InformationTypes_InformationGroup_GroupId",
                table: "InformationTypes");

            migrationBuilder.DropTable(
                name: "InformationGroup");

            migrationBuilder.DropIndex(
                name: "IX_InformationTypes_GroupId",
                table: "InformationTypes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "InformationTypes");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "InformationTypes");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "InformationTypes");
        }
    }
}
