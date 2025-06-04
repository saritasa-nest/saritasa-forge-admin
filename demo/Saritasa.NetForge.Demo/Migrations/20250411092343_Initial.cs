using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    street = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    city = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    display_name = table.Column<string>(type: "TEXT", unicode: false, nullable: false, computedColumnSql: "city || ', ' || street", stored: true),
                    postal_code = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    country = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    latitude = table.Column<double>(type: "REAL", nullable: false),
                    longitude = table.Column<double>(type: "REAL", nullable: false),
                    contact_phone = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    created_by_user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    updated_by_user_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                },
                comment: "Represents the address of the shop.");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    first_name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    user_name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "TEXT", unicode: false, maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "TEXT", unicode: false, maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    security_stamp = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    phone_number = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    lockout_end = table.Column<long>(type: "INTEGER", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    access_failed_count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contact_infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    full_name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    email = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    phone_number = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    availability_duration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    description = table.Column<string>(type: "TEXT", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    city = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => new { x.name, x.city });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role_id = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    claim_type = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    claim_value = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    claim_type = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    claim_value = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    provider_key = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    provider_display_name = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    user_id = table.Column<string>(type: "TEXT", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    role_id = table.Column<string>(type: "TEXT", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    login_provider = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    value = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shops",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    address_id = table.Column<int>(type: "INTEGER", nullable: true),
                    opened_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    open_time = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    close_time = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    total_sales = table.Column<decimal>(type: "TEXT", nullable: false),
                    is_open = table.Column<bool>(type: "INTEGER", nullable: false),
                    owner_contact_id = table.Column<int>(type: "INTEGER", nullable: true),
                    logo = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    building_photo = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shops", x => x.id);
                    table.ForeignKey(
                        name: "fk_shops_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_shops_contact_infos_owner_contact_id",
                        column: x => x.owner_contact_id,
                        principalTable: "contact_infos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    description = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    price = table.Column<decimal>(type: "TEXT", nullable: false),
                    max_price = table.Column<decimal>(type: "TEXT", nullable: true),
                    stock_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    average_purchase_count = table.Column<int>(type: "INTEGER", nullable: true),
                    weight_in_grams = table.Column<float>(type: "REAL", nullable: true),
                    length_in_centimeters = table.Column<float>(type: "REAL", nullable: false),
                    width_in_centimeters = table.Column<float>(type: "REAL", nullable: false),
                    height_in_centimeters = table.Column<float>(type: "REAL", nullable: false),
                    volume = table.Column<double>(type: "REAL", nullable: false),
                    barcode = table.Column<long>(type: "INTEGER", nullable: false),
                    is_available = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_sales_ended = table.Column<bool>(type: "INTEGER", nullable: true),
                    created_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    removed_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    end_of_sales_date = table.Column<long>(type: "INTEGER", nullable: true),
                    previous_supply_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    next_supply_date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    supplier_name = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    supplier_city = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    category = table.Column<int>(type: "INTEGER", nullable: false),
                    shop_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_products_suppliers_supplier_name_supplier_city",
                        columns: x => new { x.supplier_name, x.supplier_city },
                        principalTable: "suppliers",
                        principalColumns: new[] { "name", "city" });
                },
                comment: "Represents single product in the Shop.");

            migrationBuilder.CreateTable(
                name: "shop_products_counts",
                columns: table => new
                {
                    shop_id = table.Column<int>(type: "INTEGER", nullable: false),
                    products_count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_shop_products_counts_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_supplier",
                columns: table => new
                {
                    shops_id = table.Column<int>(type: "INTEGER", nullable: false),
                    suppliers_name = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    suppliers_city = table.Column<string>(type: "TEXT", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shop_supplier", x => new { x.shops_id, x.suppliers_name, x.suppliers_city });
                    table.ForeignKey(
                        name: "fk_shop_supplier_shops_shops_id",
                        column: x => x.shops_id,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_shop_supplier_suppliers_suppliers_name_suppliers_city",
                        columns: x => new { x.suppliers_name, x.suppliers_city },
                        principalTable: "suppliers",
                        principalColumns: new[] { "name", "city" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_product_tag",
                columns: table => new
                {
                    products_id = table.Column<int>(type: "INTEGER", nullable: false),
                    tags_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_product_tag", x => new { x.products_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_product_product_tag_product_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "product_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_product_tag_products_products_id",
                        column: x => x.products_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_product_tag_tags_id",
                table: "product_product_tag",
                column: "tags_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_shop_id",
                table: "products",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_supplier_name_supplier_city",
                table: "products",
                columns: new[] { "supplier_name", "supplier_city" });

            migrationBuilder.CreateIndex(
                name: "ix_shop_products_counts_shop_id",
                table: "shop_products_counts",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "ix_shop_supplier_suppliers_name_suppliers_city",
                table: "shop_supplier",
                columns: new[] { "suppliers_name", "suppliers_city" });

            migrationBuilder.CreateIndex(
                name: "ix_shops_address_id",
                table: "shops",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_shops_owner_contact_id",
                table: "shops",
                column: "owner_contact_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "product_product_tag");

            migrationBuilder.DropTable(
                name: "shop_products_counts");

            migrationBuilder.DropTable(
                name: "shop_supplier");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "product_tags");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "shops");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "contact_infos");
        }
    }
}
