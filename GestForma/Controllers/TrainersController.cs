using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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
        public IActionResult Actions()
        {
            return View();
        }

        public IActionResult Trainers()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterTrainer(TrainerRegister model)
        {
            if (ModelState.IsValid)
            {
                if (model.ProfileImage == null || model.ProfileImage.Length == 0)
                {
                    // Add an error message to ModelState
                    ModelState.AddModelError("ProfileImage", "Please upload a profile image.");
                    return View("AddTrainer", model); // Return to the form with the error message
                }
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
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ProfileImage.CopyToAsync(memoryStream);
                        var trainer = new Trainer
                        {
                            Id_user = user.Id,
                            Field = model.Field,
                            FileName = model.ProfileImage.FileName,
                            ContentType = model.ProfileImage.ContentType,
                            Data = memoryStream.ToArray()
                        };

                        // Sauvegarder Trainer dans la base de données (via DbContext)
                        _context.Trainers.Add(trainer);
                        await _context.SaveChangesAsync();

                    }
                    // Additional setup or logging in the user if needed
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Optionally, save any other data to the database if needed

                    // Redirect or return a success message
                    return RedirectToAction("Trainers", "Trainers"); // Or any other page
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

        public async Task<IActionResult> GetImage(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound("Trainer not found.");
            }

            return File(trainer.Data, trainer.ContentType);  // Return the image file with the content type
        }


    }
}
