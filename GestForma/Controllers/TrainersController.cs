using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestForma.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TrainersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult AddTrainer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterTrainer(TrainerRegister model)
        {
            if (ModelState.IsValid)
            {
                // Create the ApplicationUser object
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.Phone
                };

                // Create the user with the provided password
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "professeur");

                    // Additional setup or logging in the user if needed
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Optionally, save any other data to the database if needed

                    // Redirect or return a success message
                    return RedirectToAction("FormateurDashboard", "Home"); // Or any other page
                }

                // If creation failed, show errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we reach here, something went wrong, so return the view with validation errors
            return View("AddTrainer");
        }
    }
}
