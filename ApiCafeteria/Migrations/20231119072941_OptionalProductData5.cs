using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCafeteria.Migrations
{
    /// <inheritdoc />
    public partial class OptionalProductData5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressDetail",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "AddressDetail",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
