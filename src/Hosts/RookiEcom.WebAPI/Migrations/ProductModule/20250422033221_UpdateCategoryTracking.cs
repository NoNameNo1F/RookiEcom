using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookiEcom.WebAPI.Migrations.ProductModule
{
    /// <inheritdoc />
    public partial class UpdateCategoryTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                schema: "catalog",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                schema: "catalog",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                schema: "catalog",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                schema: "catalog",
                table: "Categories");
        }
    }
}
