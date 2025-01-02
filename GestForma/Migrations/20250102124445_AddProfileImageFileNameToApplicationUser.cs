using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestForma.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileImageFileNameToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32b45288-b61b-463f-a823-e96bda0889b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81ca9fc2-bfe9-460f-908c-a27e794d5c17");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "988036e3-d457-4b80-b611-b730f11303aa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9a253bb-744c-45b8-bade-b667f7d7561a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d19b291-3a43-4fff-93e6-a86d8c8e4cf8", null, "participant", "participant" },
                    { "c1006e73-6251-4578-b64f-acfae1f40e55", null, "professeur", "professeur" },
                    { "ceadc12a-d868-4a56-8f3b-1c318c778cb9", null, "administrateur", "administrateur" },
                    { "f14f05cd-b6c2-47cc-8fc4-1d5130801235", null, "invité", "invité" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d19b291-3a43-4fff-93e6-a86d8c8e4cf8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1006e73-6251-4578-b64f-acfae1f40e55");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ceadc12a-d868-4a56-8f3b-1c318c778cb9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f14f05cd-b6c2-47cc-8fc4-1d5130801235");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32b45288-b61b-463f-a823-e96bda0889b6", null, "invité", "invité" },
                    { "81ca9fc2-bfe9-460f-908c-a27e794d5c17", null, "professeur", "professeur" },
                    { "988036e3-d457-4b80-b611-b730f11303aa", null, "administrateur", "administrateur" },
                    { "c9a253bb-744c-45b8-bade-b667f7d7561a", null, "participant", "participant" }
                });
        }
    }
}
