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

           
        }
    }
}
