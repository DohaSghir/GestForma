using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestForma.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4be6328c-f7f3-4568-9e04-ad0c13c780fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d74eadd-d2f0-4ec5-932b-be40cb98a1d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d30a452b-e286-4fbc-9501-59fe4b34dbff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e65e7f6a-301d-472b-8156-7904e914face");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32b2e1a0-c609-45a3-8a31-1a703a29988e", null, "administrateur", "administrateur" },
                    { "519c98e8-26d2-47e9-9dff-216997d1aa6e", null, "professeur", "professeur" },
                    { "a55d68b8-d8e0-434e-939f-c6f1d8825839", null, "invité", "invité" },
                    { "cc7e7f19-09ad-4f78-b74e-a3fcc0fc94e8", null, "participant", "participant" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32b2e1a0-c609-45a3-8a31-1a703a29988e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "519c98e8-26d2-47e9-9dff-216997d1aa6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55d68b8-d8e0-434e-939f-c6f1d8825839");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc7e7f19-09ad-4f78-b74e-a3fcc0fc94e8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4be6328c-f7f3-4568-9e04-ad0c13c780fc", null, "professeur", "professeur" },
                    { "5d74eadd-d2f0-4ec5-932b-be40cb98a1d2", null, "participant", "participant" },
                    { "d30a452b-e286-4fbc-9501-59fe4b34dbff", null, "invité", "invité" },
                    { "e65e7f6a-301d-472b-8156-7904e914face", null, "administrateur", "administrateur" }
                });
        }
    }
}
