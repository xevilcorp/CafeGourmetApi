using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCafeteria.Migrations
{
    /// <inheritdoc />
    public partial class OptionalProductData3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductOrders_ProductOrderId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductOrderId",
                table: "OrderItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AddressDetail",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductOrders_ProductOrderId",
                table: "OrderItems",
                column: "ProductOrderId",
                principalTable: "ProductOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductOrders_ProductOrderId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductOrderId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressDetail",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductOrders_ProductOrderId",
                table: "OrderItems",
                column: "ProductOrderId",
                principalTable: "ProductOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
