using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserIdToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "addresses");
        }
    }
}
