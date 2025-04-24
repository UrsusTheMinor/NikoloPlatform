using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nikolo.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_meta_tags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "placeholder",
                table: "InformationTypes",
                newName: "Placeholder");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "InformationTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "InformationTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "InformationTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "InformationTypes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "InformationTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "InformationTypes");

            migrationBuilder.RenameColumn(
                name: "Placeholder",
                table: "InformationTypes",
                newName: "placeholder");
        }
    }
}
