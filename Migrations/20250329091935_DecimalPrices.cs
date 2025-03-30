using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_SPR311.Migrations
{
    /// <inheritdoc />
    public partial class DecimalPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "ASP",
                table: "Products",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "ASP",
                table: "Carts",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "ASP",
                table: "CartItems",
                type: "decimal(14,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "ASP",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                schema: "ASP",
                table: "Carts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                schema: "ASP",
                table: "CartItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)");
        }
    }
}
