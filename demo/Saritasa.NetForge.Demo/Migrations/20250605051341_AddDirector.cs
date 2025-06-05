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
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "director_age",
                table: "suppliers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "director_birthday",
                table: "suppliers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "director_company_employee_count",
                table: "suppliers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_company_name",
                table: "suppliers",
                type: "TEXT",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "director_department",
                table: "suppliers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_description",
                table: "suppliers",
                type: "TEXT",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "director_director_since",
                table: "suppliers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "director_is_active",
                table: "suppliers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "director_last_work_day",
                table: "suppliers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_name",
                table: "suppliers",
                type: "TEXT",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "director_photo",
                table: "suppliers",
                type: "TEXT",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "director_start_work_time",
                table: "suppliers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_director_address_id",
                table: "suppliers",
                column: "director_address_id",
                unique: true);

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
                name: "director_company_employee_count",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "director_company_name",
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
