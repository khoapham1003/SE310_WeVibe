using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeVibe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNameOrdertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Orders");
        }
    }
}
