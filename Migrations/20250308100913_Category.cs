using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_SPR311.Migrations
{
    /// <inheritdoc />
    public partial class Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "ASP",
                table: "UserAccesses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "ASP",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_RoleId",
                schema: "ASP",
                table: "UserAccesses",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_UserId",
                schema: "ASP",
                table: "UserAccesses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                schema: "ASP",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccesses_UserRoles_RoleId",
                schema: "ASP",
                table: "UserAccesses",
                column: "RoleId",
                principalSchema: "ASP",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccesses_UsersData_UserId",
                schema: "ASP",
                table: "UserAccesses",
                column: "UserId",
                principalSchema: "ASP",
                principalTable: "UsersData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccesses_UserRoles_RoleId",
                schema: "ASP",
                table: "UserAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccesses_UsersData_UserId",
                schema: "ASP",
                table: "UserAccesses");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "ASP");

            migrationBuilder.DropIndex(
                name: "IX_UserAccesses_RoleId",
                schema: "ASP",
                table: "UserAccesses");

            migrationBuilder.DropIndex(
                name: "IX_UserAccesses_UserId",
                schema: "ASP",
                table: "UserAccesses");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "ASP",
                table: "UserAccesses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
