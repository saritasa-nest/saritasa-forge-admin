using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Net7.Migrations
{
    /// <inheritdoc />
    public partial class AddSuppliersTableWithCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "supplier_city",
                table: "products",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "supplier_name",
                table: "products",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    city = table.Column<string>(type: "text", unicode: false, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => new { x.name, x.city });
                });

            migrationBuilder.CreateTable(
                name: "shop_supplier",
                columns: table => new
                {
                    shops_id = table.Column<int>(type: "integer", nullable: false),
                    suppliers_name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    suppliers_city = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shop_supplier", x => new { x.shops_id, x.suppliers_name, x.suppliers_city });
                    table.ForeignKey(
                        name: "fk_shop_supplier_shops_shops_id",
                        column: x => x.shops_id,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_shop_supplier_suppliers_suppliers_name_suppliers_city",
                        columns: x => new { x.suppliers_name, x.suppliers_city },
                        principalTable: "suppliers",
                        principalColumns: new[] { "name", "city" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_products_supplier_name_supplier_city",
                table: "products",
                columns: new[] { "supplier_name", "supplier_city" });

            migrationBuilder.CreateIndex(
                name: "ix_shop_supplier_suppliers_name_suppliers_city",
                table: "shop_supplier",
                columns: new[] { "suppliers_name", "suppliers_city" });

            migrationBuilder.AddForeignKey(
                name: "fk_products_suppliers_supplier_name_supplier_city",
                table: "products",
                columns: new[] { "supplier_name", "supplier_city" },
                principalTable: "suppliers",
                principalColumns: new[] { "name", "city" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_suppliers_supplier_name_supplier_city",
                table: "products");

            migrationBuilder.DropTable(
                name: "shop_supplier");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropIndex(
                name: "ix_products_supplier_name_supplier_city",
                table: "products");

            migrationBuilder.DropColumn(
                name: "supplier_city",
                table: "products");

            migrationBuilder.DropColumn(
                name: "supplier_name",
                table: "products");
        }
    }
}
