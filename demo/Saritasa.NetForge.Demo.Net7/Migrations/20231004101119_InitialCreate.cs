using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Saritasa.NetForge.Demo.Net7.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    street = table.Column<string>(type: "text", unicode: false, nullable: false),
                    city = table.Column<string>(type: "text", unicode: false, nullable: false),
                    postal_code = table.Column<string>(type: "text", unicode: false, nullable: false),
                    country = table.Column<string>(type: "text", unicode: false, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    contact_phone = table.Column<string>(type: "text", unicode: false, nullable: false)
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
                    id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    first_name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    last_name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", unicode: false, nullable: true),
                    security_stamp = table.Column<string>(type: "text", unicode: false, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", unicode: false, nullable: true),
                    phone_number = table.Column<string>(type: "text", unicode: false, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contact_infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    email = table.Column<string>(type: "text", unicode: false, nullable: false),
                    phone_number = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    description = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    claim_type = table.Column<string>(type: "text", unicode: false, nullable: true),
                    claim_value = table.Column<string>(type: "text", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    claim_type = table.Column<string>(type: "text", unicode: false, nullable: true),
                    claim_value = table.Column<string>(type: "text", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", unicode: false, nullable: false),
                    provider_key = table.Column<string>(type: "text", unicode: false, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", unicode: false, nullable: true),
                    user_id = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    role_id = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    login_provider = table.Column<string>(type: "text", unicode: false, nullable: false),
                    name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    value = table.Column<string>(type: "text", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shops",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: true),
                    opened_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_sales = table.Column<decimal>(type: "numeric", nullable: false),
                    is_open = table.Column<bool>(type: "boolean", nullable: false),
                    owner_contact_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shops", x => x.id);
                    table.ForeignKey(
                        name: "fk_shops_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_shops_contact_infos_owner_contact_id",
                        column: x => x.owner_contact_id,
                        principalTable: "contact_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    description = table.Column<string>(type: "text", unicode: false, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    stock_quantity = table.Column<int>(type: "integer", nullable: false),
                    weight_in_grams = table.Column<double>(type: "double precision", nullable: false),
                    length_in_centimeters = table.Column<double>(type: "double precision", nullable: false),
                    width_in_centimeters = table.Column<double>(type: "double precision", nullable: false),
                    height_in_centimeters = table.Column<double>(type: "double precision", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    shop_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents single product in the Shop.");

            migrationBuilder.CreateTable(
                name: "product_product_tag",
                columns: table => new
                {
                    products_id = table.Column<int>(type: "integer", nullable: false),
                    tags_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_product_tag", x => new { x.products_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_product_product_tag_product_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "product_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_product_product_tag_products_products_id",
                        column: x => x.products_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "addresses");

            migrationBuilder.DropTable(
                name: "contact_infos");
        }
    }
}
