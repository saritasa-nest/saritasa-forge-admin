using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saritasa.NetForge.Demo.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "close_time",
                table: "shops",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "open_time",
                table: "shops",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "availability_duration",
                table: "contact_infos",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "close_time",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "open_time",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "availability_duration",
                table: "contact_infos");
        }
    }
}
