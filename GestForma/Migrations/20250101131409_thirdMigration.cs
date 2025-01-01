using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestForma.Migrations
{
    /// <inheritdoc />
    public partial class thirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer les clés primaires sur AspNetUserTokens
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            // Supprimer les clés primaires sur AspNetUserLogins
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            // Modifier les colonnes de AspNetUserTokens
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

            // Modifier les colonnes de AspNetUserLogins
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

            // Recréer les clés primaires
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            // Suppression des rôles existants
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

            // Créer les nouvelles tables
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
                    ID_User = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.ID_Formation);
                    table.ForeignKey(
                        name: "FK_Formations_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentairesDeFormation",
                columns: table => new
                {
                    IdCommentaire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Formation = table.Column<int>(type: "int", nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Commentaire = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentairesDeFormation", x => x.IdCommentaire);
                    table.ForeignKey(
                        name: "FK_CommentairesDeFormation_AspNetUsers_ID",
                        column: x => x.ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentairesDeFormation_Formations_ID_Formation",
                        column: x => x.ID_Formation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    ID_Inscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_User = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ID_Formation = table.Column<int>(type: "int", nullable: false),
                    Etat = table.Column<bool>(type: "bit", nullable: false),
                    Paiement = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.ID_Inscription);
                    table.ForeignKey(
                        name: "FK_Inscriptions_AspNetUsers_ID",
                        column: x => x.ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscriptions_Formations_ID_Formation",
                        column: x => x.ID_Formation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    IdRate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Formation = table.Column<int>(type: "int", nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.IdRate);
                    table.ForeignKey(
                        name: "FK_Rates_AspNetUsers_ID",
                        column: x => x.ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rates_Formations_ID_Formation",
                        column: x => x.ID_Formation,
                        principalTable: "Formations",
                        principalColumn: "ID_Formation",
                        onDelete: ReferentialAction.Cascade);
                });

            // Insérer les nouveaux rôles
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f3b96b8-097e-4da5-98be-1cf91d483915", null, "administrateur", "administrateur" },
                    { "13d8e556-b1c2-4c75-a101-9baa6ce50c1d", null, "invité", "invité" },
                    { "df046747-7b2a-4853-af29-e3f4e44a481c", null, "professeur", "professeur" },
                    { "f47070e8-7944-4c15-898d-e90ebd20f644", null, "participant", "participant" }
                });

            // Ajouter des index
            migrationBuilder.CreateIndex(
                name: "IX_CommentairesDeFormation_ID",
                table: "CommentairesDeFormation",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentairesDeFormation_ID_Formation",
                table: "CommentairesDeFormation",
                column: "ID_Formation");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_Id",
                table: "Formations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_ID",
                table: "Inscriptions",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_ID_Formation",
                table: "Inscriptions",
                column: "ID_Formation");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ID",
                table: "Rates",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ID_Formation",
                table: "Rates",
                column: "ID_Formation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Réimplémentation de la méthode `Down` avec suppression des clés primaires
        }
    }
}
