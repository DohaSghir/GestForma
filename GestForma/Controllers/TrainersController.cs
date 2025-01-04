using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        [Authorize(Roles = "administrateur")]
        public IActionResult AddTrainer()
        {
            return View();
        }
  

        [Authorize(Roles = "administrateur")]
        public IActionResult Trainers()
        {
            return View();
        }

        [Authorize(Roles = "administrateur")]
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

        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> GetImage(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound("Trainer not found.");
            }

            return File(trainer.Data, trainer.ContentType);  // Return the image file with the content type
        }
        
        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> Actions()
        {
            var users = _userManager.Users.ToList();
            
            List<List<Object>> trainers = new List<List<Object>>();
            
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "professeur"))
                {
                    var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id_user == user.Id);
                    string imageUrl = trainer != null ? Url.Action("GetImage", "Trainers", new { id = trainer.Id }) : null;
                    trainers.Add(new List<Object> {user.Id, user.LastName,user.FirstName,user.Email,user.PhoneNumber,trainer.Field, imageUrl });
                }
            }
            

            return View(trainers);
        }

        [Authorize(Roles = "administrateur")]
        [HttpPost]
        public async Task<IActionResult> DeleteTrainer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Invalid ID.";
                return RedirectToAction(nameof(Actions));
            }

            var user = await _userManager.FindByIdAsync(id); 
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id_user == user.Id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Actions));
            }
            _context.Trainers.Remove(trainer);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = $"The user {user.UserName} has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "An error occurred while deleting the user.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction(nameof(Actions));
        }

        public async Task<IActionResult> EditFormTrainer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Invalid ID.";
                return RedirectToAction(nameof(Actions));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Actions));
            }

            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id_user == user.Id);
            if (trainer == null)
            {
                TempData["Error"] = "Trainer details not found.";
                return RedirectToAction(nameof(Actions));
            }

            var model = new TrainerRegister
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Field = trainer.Field
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "administrateur")]
  
        public async Task<IActionResult> EditTrainer(TrainerRegister model)
        {
            if (ModelState.IsValid)
            {
                if (model.ProfileImage == null || model.ProfileImage.Length == 0)
                {
                    // Add an error message to ModelState
                    ModelState.AddModelError("ProfileImage", "Please upload a profile image.");
                    return View("EditFormTrainer", model); // Return to the form with the error message
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction(nameof(Actions));
                }

                // Update ApplicationUser (identity user) details
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Phone;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("EditFormTrainer", model);  // Return the form with update errors
                }

                // If a new password is provided, update the password
                if (!string.IsNullOrEmpty(model.Password))
                {

                    var result = await _userManager.ChangePasswordAsync(user, user.PasswordHash, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("EditFormTrainer", model);  // Return the form with password change errors
                    }
                }
                var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id_user == user.Id);
                if (trainer == null)
                {
                    TempData["Error"] = "Trainer details not found.";
                    return RedirectToAction(nameof(Actions));
                }

                // Handle profile image upload if it exists
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ProfileImage.CopyToAsync(memoryStream);
                        trainer.FileName = model.ProfileImage.FileName;
                        trainer.ContentType = model.ProfileImage.ContentType;
                        trainer.Data = memoryStream.ToArray();
                    }
                }

                // Save changes to Trainer entity
                _context.Trainers.Update(trainer);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Trainer details updated successfully.";
                return RedirectToAction(nameof(Actions));  // Redirect to the Trainers list or another page as needed
            }

            // If we reach here, something went wrong. Return to the edit form with validation errors.
            TempData["Error"] = "Invalid data submitted.";
            return View("Trainers");

        }
        }





    }
}
