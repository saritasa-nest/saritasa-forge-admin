using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Net7.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTypesToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "opened_date",
                table: "shops",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<float>(
                name: "weight_in_grams",
                table: "products",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "products",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "products",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "average_purchase_count",
                table: "products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "barcode",
                table: "products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "end_of_sales_date",
                table: "products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_available",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_sales_ended",
                table: "products",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "max_price",
                table: "products",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "next_supply_date",
                table: "products",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "previous_supply_date",
                table: "products",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                table: "products",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "volume",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_birth",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average_purchase_count",
                table: "products");

            migrationBuilder.DropColumn(
                name: "barcode",
                table: "products");

            migrationBuilder.DropColumn(
                name: "end_of_sales_date",
                table: "products");

            migrationBuilder.DropColumn(
                name: "is_available",
                table: "products");

            migrationBuilder.DropColumn(
                name: "is_sales_ended",
                table: "products");

            migrationBuilder.DropColumn(
                name: "max_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "next_supply_date",
                table: "products");

            migrationBuilder.DropColumn(
                name: "previous_supply_date",
                table: "products");

            migrationBuilder.DropColumn(
                name: "removed_at",
                table: "products");

            migrationBuilder.DropColumn(
                name: "type",
                table: "products");

            migrationBuilder.DropColumn(
                name: "volume",
                table: "products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "opened_date",
                table: "shops",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<float>(
                name: "weight_in_grams",
                table: "products",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_birth",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
