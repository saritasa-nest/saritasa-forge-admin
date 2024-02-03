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
            migrationBuilder.AddColumn<string>(
                name: "base64image",
                table: "shops",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "path_to_image",
                table: "shops",
                type: "text",
                unicode: false,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "base64image",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "path_to_image",
                table: "shops");
        }
    }
}
