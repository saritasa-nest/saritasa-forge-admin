using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "fk_product_product_tag_product_tags_tags_id",
                table: "product_product_tag");

            migrationBuilder.DropForeignKey(
                name: "fk_product_product_tag_products_products_id",
                table: "product_product_tag");

            migrationBuilder.DropForeignKey(
                name: "fk_products_shops_shop_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_products_suppliers_supplier_name_supplier_city",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_shop_products_counts_shops_shop_id",
                table: "shop_products_counts");

            migrationBuilder.DropForeignKey(
                name: "fk_shop_supplier_shops_shops_id",
                table: "shop_supplier");

            migrationBuilder.DropForeignKey(
                name: "fk_shop_supplier_suppliers_suppliers_name_suppliers_city",
                table: "shop_supplier");

            migrationBuilder.DropForeignKey(
                name: "fk_shops_addresses_address_id",
                table: "shops");

            migrationBuilder.DropForeignKey(
                name: "fk_shops_contact_infos_owner_contact_id",
                table: "shops");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "AspNetRoleClaims",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "AspNetUserClaims",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "AspNetUserLogins",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "AspNetUserRoles",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "AspNetUserTokens",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_product_product_tag_product_tags_tags_id",
                table: "product_product_tag",
                column: "tags_id",
                principalTable: "product_tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_product_product_tag_products_products_id",
                table: "product_product_tag",
                column: "products_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_products_shops_shop_id",
                table: "products",
                column: "shop_id",
                principalTable: "shops",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_products_suppliers_supplier_name_supplier_city",
                table: "products",
                columns: new[] { "supplier_name", "supplier_city" },
                principalTable: "suppliers",
                principalColumns: new[] { "name", "city" });

            migrationBuilder.AddForeignKey(
                name: "fk_shop_products_counts_shops_shop_id",
                table: "shop_products_counts",
                column: "shop_id",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_shop_supplier_shops_shops_id",
                table: "shop_supplier",
                column: "shops_id",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_shop_supplier_suppliers_suppliers_name_suppliers_city",
                table: "shop_supplier",
                columns: new[] { "suppliers_name", "suppliers_city" },
                principalTable: "suppliers",
                principalColumns: new[] { "name", "city" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_shops_addresses_address_id",
                table: "shops",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_shops_contact_infos_owner_contact_id",
                table: "shops",
                column: "owner_contact_id",
                principalTable: "contact_infos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "fk_product_product_tag_product_tags_tags_id",
                table: "product_product_tag");

            migrationBuilder.DropForeignKey(
                name: "fk_product_product_tag_products_products_id",
                table: "product_product_tag");

            migrationBuilder.DropForeignKey(
                name: "fk_products_shops_shop_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_products_suppliers_supplier_name_supplier_city",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_shop_products_counts_shops_shop_id",
                table: "shop_products_counts");

            migrationBuilder.DropForeignKey(
                name: "fk_shop_supplier_shops_shops_id",
                table: "shop_supplier");

            migrationBuilder.DropForeignKey(
                name: "fk_shop_supplier_suppliers_suppliers_name_suppliers_city",
                table: "shop_supplier");

            migrationBuilder.DropForeignKey(
                name: "fk_shops_addresses_address_id",
                table: "shops");

            migrationBuilder.DropForeignKey(
                name: "fk_shops_contact_infos_owner_contact_id",
                table: "shops");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "AspNetRoleClaims",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "AspNetUserClaims",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "AspNetUserLogins",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "AspNetUserRoles",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "AspNetUserTokens",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_product_product_tag_product_tags_tags_id",
                table: "product_product_tag",
                column: "tags_id",
                principalTable: "product_tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_product_product_tag_products_products_id",
                table: "product_product_tag",
                column: "products_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_products_shops_shop_id",
                table: "products",
                column: "shop_id",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_products_suppliers_supplier_name_supplier_city",
                table: "products",
                columns: new[] { "supplier_name", "supplier_city" },
                principalTable: "suppliers",
                principalColumns: new[] { "name", "city" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shop_products_counts_shops_shop_id",
                table: "shop_products_counts",
                column: "shop_id",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shop_supplier_shops_shops_id",
                table: "shop_supplier",
                column: "shops_id",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shop_supplier_suppliers_suppliers_name_suppliers_city",
                table: "shop_supplier",
                columns: new[] { "suppliers_name", "suppliers_city" },
                principalTable: "suppliers",
                principalColumns: new[] { "name", "city" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shops_addresses_address_id",
                table: "shops",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shops_contact_infos_owner_contact_id",
                table: "shops",
                column: "owner_contact_id",
                principalTable: "contact_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
