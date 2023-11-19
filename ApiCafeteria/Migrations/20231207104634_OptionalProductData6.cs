using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCafeteria.Migrations
{
    /// <inheritdoc />
    public partial class OptionalProductData6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProductOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductOrders");
        }
    }
}
