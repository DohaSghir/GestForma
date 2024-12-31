using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestForma.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "51b010ee-fe0e-42d8-aeb1-deeed6fc892b", null, "participant", "participant" },
                    { "5f46036f-2903-4ddb-be78-2239e8c349dd", null, "administrateur", "administrateur" },
                    { "c3daebd3-010a-409b-a268-046b887740c2", null, "invité", "invité" },
                    { "ceab2aee-7bce-4e2b-9536-520dc01ef683", null, "professeur", "professeur" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51b010ee-fe0e-42d8-aeb1-deeed6fc892b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f46036f-2903-4ddb-be78-2239e8c349dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3daebd3-010a-409b-a268-046b887740c2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ceab2aee-7bce-4e2b-9536-520dc01ef683");
        }
    }
}
