﻿// <auto-generated />
using System;
using GestForma.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestForma.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250103210356_First")]
    partial class First
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GestForma.Models.Actualite", b =>
                {
                    b.Property<int>("IdActualite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdActualite"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdActualite");

                    b.ToTable("Actualites");
                });

            modelBuilder.Entity("GestForma.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("GestForma.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GestForma.Models.CommentairesDeFormation", b =>
                {
                    b.Property<int>("IdCommentaire")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCommentaire"));

                    b.Property<string>("Commentaire")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("ID_Formation")
                        .HasColumnType("int");

                    b.Property<string>("ID_User")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdCommentaire");

                    b.HasIndex("ID_Formation");

                    b.HasIndex("ID_User");

                    b.ToTable("CommentairesDeFormations");
                });

            modelBuilder.Entity("GestForma.Models.CommentairesEntiers", b =>
                {
                    b.Property<int>("IdCommentaire")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCommentaire"));

                    b.Property<string>("ContenuCommentaire")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("IdCommentaire");

                    b.ToTable("CommentairesEntiers");
                });

            modelBuilder.Entity("GestForma.Models.Formation", b =>
                {
                    b.Property<int>("ID_Formation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Formation"));

                    b.Property<string>("ContentType")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<float>("Cout")
                        .HasColumnType("real");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Duree")
                        .HasColumnType("real");

                    b.Property<string>("FileName")
                        .HasMaxLength(2955)
                        .HasColumnType("nvarchar(2955)");

                    b.Property<string>("ID_User")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Id_Categorie")
                        .HasColumnType("int");

                    b.Property<string>("Intitule")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("ID_Formation");

                    b.HasIndex("ID_User");

                    b.HasIndex("Id_Categorie");

                    b.ToTable("Formations");
                });

            modelBuilder.Entity("GestForma.Models.Inscription", b =>
                {
                    b.Property<int>("ID_Inscription")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Inscription"));

                    b.Property<bool>("Certificat")
                        .HasColumnType("bit");

                    b.Property<bool>("Etat")
                        .HasColumnType("bit");

                    b.Property<bool>("Fin")
                        .HasColumnType("bit");

                    b.Property<int>("ID_Formation")
                        .HasColumnType("int");

                    b.Property<string>("ID_User")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Paiement")
                        .HasColumnType("bit");

                    b.HasKey("ID_Inscription");

                    b.HasIndex("ID_Formation");

                    b.HasIndex("ID_User");

                    b.ToTable("Inscriptions");
                });

            modelBuilder.Entity("GestForma.Models.Rate", b =>
                {
                    b.Property<int>("IdRate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRate"));

                    b.Property<double>("ContenuRate")
                        .HasColumnType("float");

                    b.Property<int>("ID_Formation")
                        .HasColumnType("int");

                    b.Property<string>("ID_User")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdRate");

                    b.HasIndex("ID_Formation");

                    b.HasIndex("ID_User");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("GestForma.Models.StatistiqueFormation", b =>
                {
                    b.Property<int>("IdStatistiqueFormation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStatistiqueFormation"));

                    b.Property<int>("IdFormation")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantsParFormation")
                        .HasColumnType("int");

                    b.Property<double>("RateMoyen")
                        .HasColumnType("float");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("IdStatistiqueFormation");

                    b.HasIndex("IdFormation");

                    b.ToTable("StatistiqueFormations");
                });

            modelBuilder.Entity("GestForma.Models.StatistiqueGlobale", b =>
                {
                    b.Property<int>("IdStatistiqueGlobale")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStatistiqueGlobale"));

                    b.Property<int>("NombreDeCategories")
                        .HasColumnType("int");

                    b.Property<int>("NombreDeFormations")
                        .HasColumnType("int");

                    b.Property<int>("NombreDeParticipants")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("IdStatistiqueGlobale");

                    b.ToTable("StatistiqueGlobales");
                });

            modelBuilder.Entity("GestForma.Models.Trainer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Field")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Id_user")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Id_user");

                    b.ToTable("Trainers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "bd00591b-f9d1-4641-a953-ab3e705ff654",
                            Name = "administrateur",
                            NormalizedName = "administrateur"
                        },
                        new
                        {
                            Id = "53f89cd5-b4b5-470d-a0fc-42a688c12288",
                            Name = "professeur",
                            NormalizedName = "professeur"
                        },
                        new
                        {
                            Id = "977593b5-f3bd-4464-bc7f-b1b57077661d",
                            Name = "participant",
                            NormalizedName = "participant"
                        },
                        new
                        {
                            Id = "40e20fc5-087c-4133-b304-f30a10f92d12",
                            Name = "invité",
                            NormalizedName = "invité"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("GestForma.Models.CommentairesDeFormation", b =>
                {
                    b.HasOne("GestForma.Models.Formation", "Formation")
                        .WithMany("Commentaires")
                        .HasForeignKey("ID_Formation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestForma.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ID_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Formation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GestForma.Models.Formation", b =>
                {
                    b.HasOne("GestForma.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ID_User");

                    b.HasOne("GestForma.Models.Category", "Categorie")
                        .WithMany()
                        .HasForeignKey("Id_Categorie");

                    b.Navigation("Categorie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GestForma.Models.Inscription", b =>
                {
                    b.HasOne("GestForma.Models.Formation", "Formation")
                        .WithMany("Inscriptions")
                        .HasForeignKey("ID_Formation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestForma.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ID_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Formation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GestForma.Models.Rate", b =>
                {
                    b.HasOne("GestForma.Models.Formation", "Formation")
                        .WithMany("Evaluations")
                        .HasForeignKey("ID_Formation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestForma.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ID_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Formation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GestForma.Models.StatistiqueFormation", b =>
                {
                    b.HasOne("GestForma.Models.Formation", "Formation")
                        .WithMany()
                        .HasForeignKey("IdFormation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Formation");
                });

            modelBuilder.Entity("GestForma.Models.Trainer", b =>
                {
                    b.HasOne("GestForma.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("Id_user");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GestForma.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GestForma.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestForma.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GestForma.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GestForma.Models.Formation", b =>
                {
                    b.Navigation("Commentaires");

                    b.Navigation("Evaluations");

                    b.Navigation("Inscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
