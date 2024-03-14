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
                name: "building_photo",
                table: "shops",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "logo",
                table: "shops",
                type: "text",
                unicode: false,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "building_photo",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "logo",
                table: "shops");
        }
    }
}
