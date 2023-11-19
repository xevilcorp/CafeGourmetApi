using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCafeteria.Migrations
{
    /// <inheritdoc />
    public partial class OptionalProductData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Transactions_TransactionId",
                table: "ProductOrders");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "ProductOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_Transactions_TransactionId",
                table: "ProductOrders",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Transactions_TransactionId",
                table: "ProductOrders");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "ProductOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_Transactions_TransactionId",
                table: "ProductOrders",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
