using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookiEcom.WebAPI.Migrations.ProductModule
{
    /// <inheritdoc />
    public partial class UpdateProductModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                schema: "catalog",
                table: "ProductRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                schema: "catalog",
                table: "ProductRatings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                schema: "catalog",
                table: "ProductRatings",
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
                table: "ProductRatings");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                schema: "catalog",
                table: "ProductRatings");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                schema: "catalog",
                table: "ProductRatings");
        }
    }
}
