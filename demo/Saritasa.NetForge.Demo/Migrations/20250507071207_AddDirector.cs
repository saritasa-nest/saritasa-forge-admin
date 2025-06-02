using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Migrations
{
    /// <inheritdoc />
    public partial class AddDirector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "director_address_id",
                table: "suppliers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "director_age",
                table: "suppliers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "director_birthday",
                table: "suppliers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "director_department",
                table: "suppliers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_description",
                table: "suppliers",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "director_director_since",
                table: "suppliers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "director_is_active",
                table: "suppliers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "director_last_work_day",
                table: "suppliers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_name",
                table: "suppliers",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_photo",
                table: "suppliers",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "director_start_work_time",
                table: "suppliers",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_director_address_id",
                table: "suppliers",
                column: "director_address_id");

            migrationBuilder.AddForeignKey(
                name: "fk_suppliers_addresses_director_address_id",
                table: "suppliers",
                column: "director_address_id",
                principalTable: "addresses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_suppliers_addresses_director_address_id",
                table: "suppliers");

            migrationBuilder.DropIndex(
                name: "ix_suppliers_director_address_id",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_address_id",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_age",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_birthday",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_department",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_description",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_director_since",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_is_active",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_last_work_day",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_name",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_photo",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_start_work_time",
                table: "suppliers");
        }
    }
}
