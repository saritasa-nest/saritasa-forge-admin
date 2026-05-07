using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Migrations
{
    /// <inheritdoc />
    public partial class AddTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "token_id",
                table: "shops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tokens", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_shops_token_id",
                table: "shops",
                column: "token_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_shops_tokens_token_id",
                table: "shops",
                column: "token_id",
                principalTable: "tokens",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_shops_tokens_token_id",
                table: "shops");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropIndex(
                name: "ix_shops_token_id",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "token_id",
                table: "shops");
        }
    }
}
