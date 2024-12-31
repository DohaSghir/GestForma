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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

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


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
