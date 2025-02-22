using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP_SPR311.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ASP",
                table: "UserRoles",
                columns: new[] { "Id", "CanCreate", "CanDelete", "CanRead", "CanUpdate", "Description" },
                values: new object[,]
                {
                    { "admin", 1, 1, 1, 1, "Адміністратор БД" },
                    { "editor", 0, 0, 1, 1, "З правом редагування контенту" },
                    { "guest", 0, 0, 0, 0, "Самостійно зареєстрований користувач" },
                    { "moderator", 0, 1, 1, 0, "З правом блокування контенту" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ASP",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                schema: "ASP",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: "editor");

            migrationBuilder.DeleteData(
                schema: "ASP",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: "guest");

            migrationBuilder.DeleteData(
                schema: "ASP",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: "moderator");
        }
    }
}
