using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Net7.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePropertiesToShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "image_bytes",
                table: "shops",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "shops",
                type: "text",
                unicode: false,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_bytes",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "image_path",
                table: "shops");
        }
    }
}
