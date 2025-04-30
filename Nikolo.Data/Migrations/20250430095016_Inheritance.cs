using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nikolo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeName",
                table: "InformationTypes",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "InformationGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "InformationGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "InformationGroups");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "InformationGroups");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "InformationTypes",
                newName: "TypeName");
        }
    }
}
