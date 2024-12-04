using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeVibe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Sizes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Sizes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ProductImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Colors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Colors",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Colors");
        }
    }
}
