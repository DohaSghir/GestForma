using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using GestForma.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GestForma.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Formation> Formations { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<CommentairesDeFormation> CommentairesDeFormations { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<CommentairesEntiers> CommentairesEntiers { get; set; }
        public DbSet<StatistiqueFormation> StatistiqueFormations { get; set; }
        public DbSet<StatistiqueGlobale> StatistiqueGlobales { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            var administrateur = new IdentityRole("administrateur");
            administrateur.NormalizedName = "administrateur";


            var professeur = new IdentityRole("professeur");
            professeur.NormalizedName = "professeur";

            var participant = new IdentityRole("participant");
            participant.NormalizedName = "participant";

            var invité = new IdentityRole("invité");
            invité.NormalizedName = "invité";

            builder.Entity<IdentityRole>().HasData(administrateur, professeur, participant, invité);

            // Configuration pour CommentairesDeFormations
            builder.Entity<CommentairesDeFormation>()
                .HasOne(c => c.Formation)
                .WithMany(f => f.Commentaires)
                .HasForeignKey(c => c.ID_Formation)
                .OnDelete(DeleteBehavior.Restrict); // Empêche les suppressions en cascade

            builder.Entity<CommentairesDeFormation>()
                .HasOne(c => c.User)
                .WithMany() // Supposant que User n'a pas de collection
                .HasForeignKey(c => c.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration pour d'autres relations similaires
            builder.Entity<Formation>()
                .HasMany(f => f.Inscriptions)
                .WithOne(i => i.Formation)
                .HasForeignKey(i => i.ID_Formation)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Formation>()
                .HasMany(f => f.Evaluations)
                .WithOne(e => e.Formation)
                .HasForeignKey(e => e.ID_Formation)
                .OnDelete(DeleteBehavior.Restrict);


        }


        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
