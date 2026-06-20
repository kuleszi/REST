using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L5_2025N_02.Migrations
{
    /// <inheritdoc />
    public partial class NameDescrepencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "Auctions",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "Auctions",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Auctions",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Auctions",
                newName: "ItemDescription");
        }
    }
}
