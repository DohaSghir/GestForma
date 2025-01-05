using Azure;
using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GestForma.Controllers
{
    public class InscriptionsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        // Injection des services UserManager et RoleManager
        public InscriptionsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> CourseRegistration(int CourseId, string ParticipantId)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(ParticipantId) || CourseId <= 0)
            {
                // Return an error view or redirect to a page with an error message
                return BadRequest("Invalid CourseId or ParticipantId.");
            }

            var result = await _context.Inscriptions.FirstOrDefaultAsync(element => element.ID_Formation == CourseId);
            if (result != null && !result.Certificat) {
                TempData["Error"] = "You are already registered for this course and have not yet received a certificate. After receiving the certificate, you can register for this course again if you wish.";
                return RedirectToAction("Index", "Home");
            }
            // Create a new inscription
            var newInscription = new Inscription
            {
                ID_User = ParticipantId,
                ID_Formation = CourseId
            };

            // Add to the database and save changes
            _context.Add(newInscription);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Registration confirmed. Please visit the center to complete the payment for this course.";
            // Redirect to the index page or a confirmation view
            return RedirectToAction("Index","Home");
        }

    }
}
