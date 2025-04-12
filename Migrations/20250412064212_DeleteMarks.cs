using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_SPR311.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "ASP",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "ASP",
                table: "Categories",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "ASP",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "ASP",
                table: "Categories");
        }
    }
}
