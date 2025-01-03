using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GestForma.Models;
using GestForma.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class TrainerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TrainerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult AddTrainer()
    {
        return View();
    }
    //[HttpPost]
    /* public async Task<IActionResult> RegisterTrainer(TrainerRegistration model)
     {
         if (ModelState.IsValid)
         {
             // Créer un nouvel utilisateur
             var user = new ApplicationUser
             {
                 UserName = model.Email,
                 Email = model.Email,
                 FirstName = model.FirstName,
                 LastName = model.LastName,
                 PhoneNumber = model.PhoneNumber,
                 CreatedAt = DateTime.Now
             };

             var result = await _userManager.CreateAsync(user, model.Password);

             if (result.Succeeded)
             {
                 // Ajouter le rôle Trainer (si applicable)
                 if (!await _roleManager.RoleExistsAsync("Trainer"))
                 {
                     await _roleManager.CreateAsync(new IdentityRole("Trainer"));
                 }
                 await _userManager.AddToRoleAsync(user, "Trainer");

                 // Gérer l'upload de l'image de profil
                 string profileImagePath = null;
                 if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                 {
                     // Dossier où les images seront stockées
                     string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "upload");
                     Directory.CreateDirectory(uploadsFolder);

                     // Générer un nom unique pour le fichier
                     string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);

                     // Chemin complet pour le fichier
                     profileImagePath = Path.Combine(uploadsFolder, uniqueFileName);

                     // Sauvegarder le fichier
                     using (var stream = new FileStream(profileImagePath, FileMode.Create))
                     {
                         await model.ProfileImage.CopyToAsync(stream);
                     }
                 }

                 // Enregistrer les informations supplémentaires dans Trainer
                 var trainer = new Trainer
                 {
                     Id_user = user.Id,
                     Field = model.Field,
                     ProfileImagePath = profileImagePath // Enregistrer le chemin de l'image
                 };

                 // Sauvegarder Trainer dans la base de données (via DbContext)
                 // Exemple :
                  _context.Trainers.Add(trainer);
                  await _context.SaveChangesAsync();

                 return RedirectToAction("Success");
             }

             foreach (var error in result.Errors)
             {
                 ModelState.AddModelError("", error.Description);
             }
         }

         return View(model);
     }
    */
}

