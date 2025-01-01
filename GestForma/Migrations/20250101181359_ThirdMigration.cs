using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestForma.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "CommentairesEntiers",
                columns: table => new
                {
                    IdCommentaire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContenuCommentaire = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentairesEntiers", x => x.IdCommentaire);
                });

            migrationBuilder.CreateTable(
                name: "Formations",
                columns: table => new
                {
                    ID_Formation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intitule = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duree = table.Column<float>(type: "real", nullable: false),
                    Cout = table.Column<float>(type: "real", nullable: false),
                    ID_User = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.ID_Formation);
                    table.ForeignKey(
                        name: "FK_Formations_AspNetUsers_ID_User",
                        column: x => x.ID_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatistiqueGlobales",
                columns: table => new
                {
                    IdStatistiqueGlobale = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreDeFormations = table.Column<int>(type: "int", nullable: false),
                    NombreDeParticipants = table.Column<int>(type: "int", nullable: false),
                    NombreDeCategories = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiqueGlobales", x => x.IdStatistiqueGlobale);
                });

            migrationBuilder.CreateTable(
                name: "CommentairesDeFormations",
                columns: table => new
                {
                    IdCommentaire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Formation = table.Column<int>(type: "int", nullable: false),
                    ID_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentairesDeFormations", x => x.IdCommentaire);
                    table.ForeignKey(
                        name: "FK_CommentairesDeFormations_AspNetUsers_ID_User",
                        column: x => x.ID_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentairesDeFormations_Formations_ID_Formation",
                        column: x => x.ID_Formation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    ID_Inscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_Formation = table.Column<int>(type: "int", nullable: false),
                    Etat = table.Column<bool>(type: "bit", nullable: false),
                    Paiement = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.ID_Inscription);
                    table.ForeignKey(
                        name: "FK_Inscriptions_AspNetUsers_ID_User",
                        column: x => x.ID_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Formations_ID_Formation",
                        column: x => x.ID_Formation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    IdRate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Formation = table.Column<int>(type: "int", nullable: false),
                    ID_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContenuRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.IdRate);
                    table.ForeignKey(
                        name: "FK_Rates_AspNetUsers_ID_User",
                        column: x => x.ID_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rates_Formations_ID_Formation",
                        column: x => x.ID_Formation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatistiqueFormations",
                columns: table => new
                {
                    IdStatistiqueFormation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFormation = table.Column<int>(type: "int", nullable: false),
                    ParticipantsParFormation = table.Column<int>(type: "int", nullable: false),
                    RateMoyen = table.Column<double>(type: "float", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiqueFormations", x => x.IdStatistiqueFormation);
                    table.ForeignKey(
                        name: "FK_StatistiqueFormations_Formations_IdFormation",
                        column: x => x.IdFormation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "150d866e-2015-4109-a3a8-5ca8e149e3ff", null, "invité", "invité" },
                    { "5c368016-f98f-41f0-9a62-5fe3e66b056d", null, "participant", "participant" },
                    { "d609ec45-9228-4d07-aa80-1917e4631d17", null, "professeur", "professeur" },
                    { "fb7f3a4b-6cfd-4451-a8ec-c6d69f5ca9e6", null, "administrateur", "administrateur" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentairesDeFormations_ID_Formation",
                table: "CommentairesDeFormations",
                column: "ID_Formation");

            migrationBuilder.CreateIndex(
                name: "IX_CommentairesDeFormations_ID_User",
                table: "CommentairesDeFormations",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_ID_User",
                table: "Formations",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_ID_Formation",
                table: "Inscriptions",
                column: "ID_Formation");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_ID_User",
                table: "Inscriptions",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ID_Formation",
                table: "Rates",
                column: "ID_Formation");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ID_User",
                table: "Rates",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_StatistiqueFormations_IdFormation",
                table: "StatistiqueFormations",
                column: "IdFormation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentairesDeFormations");

            migrationBuilder.DropTable(
                name: "CommentairesEntiers");

            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "StatistiqueFormations");

            migrationBuilder.DropTable(
                name: "StatistiqueGlobales");

            migrationBuilder.DropTable(
                name: "Formations");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "150d866e-2015-4109-a3a8-5ca8e149e3ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c368016-f98f-41f0-9a62-5fe3e66b056d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d609ec45-9228-4d07-aa80-1917e4631d17");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb7f3a4b-6cfd-4451-a8ec-c6d69f5ca9e6");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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
    }
}
