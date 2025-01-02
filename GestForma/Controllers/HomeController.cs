using System.Diagnostics;
using GestForma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestForma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Injection des services UserManager et RoleManager
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Action Index
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("administrateur"))
                {
                    return RedirectToAction("AdminDashboard");
                }

                if (User.IsInRole("professeur"))
                {
                    return RedirectToAction("FormateurDashboard");
                }
            }
            return View();
        }

        // Action Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Action Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Actions Dashboard Admin et Formateur
        [Authorize(Roles = "administrateur")]
        public IActionResult AdminDashboard()
        {
            ViewData["Layout"] = "_LayoutAdmin";
            return View();
        }

        [Authorize(Roles = "professeur")]
        public IActionResult FormateurDashboard()
        {
            ViewData["Layout"] = "_LayoutFormateur";
            return View();
        }

        // Action pour récupérer les utilisateurs ayant le rôle "invité"
        [Authorize(Roles = "administrateur")]
<<<<<<< HEAD
        // Action pour récupérer la liste des utilisateurs ayant le rôle "invité"
        public async Task<IActionResult> GetUsersWithRole()
        {
            // Récupérer tous les utilisateurs
            var users = _userManager.Users.ToList();

            // Créer une liste pour stocker les utilisateurs avec le rôle "invité"
            var invitedUsers = new List<ApplicationUser>();

            // Vérifier pour chaque utilisateur s'il a le rôle "invité"
=======
        public async Task<IActionResult> GetUsersWithRole()
        {
            var users = _userManager.Users.ToList();
            var invitedUsers = new List<ApplicationUser>();

>>>>>>> bde12b6c5d9686bfe0e65a3b351afa0f51353e7c
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "invité"))
                {
<<<<<<< HEAD
                    invitedUsers.Add(user); // Ajouter à la liste si l'utilisateur est dans le rôle "invité"
                }
            }

            // Retourner la liste des utilisateurs "invités" à la vue
            return View(invitedUsers);
        }
        
                [Authorize(Roles = "administrateur")] 
                [HttpPost]
                public async Task<IActionResult> DeleteParticipant(string id)
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        TempData["Error"] = "ID invalide.";
                        return RedirectToAction("GetUsersWithRoleP"); 
                    }

                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        TempData["Error"] = "Utilisateur introuvable.";
                        return RedirectToAction("GetUsersWithRoleP");
                    }


                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = $" {user.UserName} deleted.";
                    }
                    else
                    {
                        TempData["Error"] = "Une erreur est survenue lors de la suppression de l'utilisateur.";
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                    return RedirectToAction("GetUsersWithRoleP");
                }

          
     

        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> GetUsersWithRoleP()
        {
            // Récupérer tous les utilisateurs
            var users = _userManager.Users.ToList();

            // Créer une liste pour stocker les utilisateurs avec le rôle "invité"
            var invitedUsers = new List<ApplicationUser>();

            // Vérifier pour chaque utilisateur s'il a le rôle "invité"
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "participant"))
                {
                    invitedUsers.Add(user); // Ajouter à la liste si l'utilisateur est dans le rôle "invité"
                }
            }

            // Retourner la liste des utilisateurs "invités" à la vue
=======
                    invitedUsers.Add(user);
                }
            }

>>>>>>> bde12b6c5d9686bfe0e65a3b351afa0f51353e7c
            return View(invitedUsers);
        }

        // Action pour changer le rôle d'un utilisateur en "participant"
        [Authorize(Roles = "administrateur")]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleToParticipant(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID invalide.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Utilisateur introuvable.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

<<<<<<< HEAD
            // Supprimer l'utilisateur du rôle "invité"
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "invité");
            if (!removeRoleResult.Succeeded)
            {
                // Gérer l'échec de la suppression du rôle
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            // Ajouter l'utilisateur au rôle "participant"
            var addRoleResult = await _userManager.AddToRoleAsync(user, "participant");
            if (!addRoleResult.Succeeded)
            {
                // Gérer l'échec de l'ajout du rôle
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            // Rediriger après la modification du rôle
=======
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "invité");
            if (!removeRoleResult.Succeeded)
            {
                TempData["Error"] = "Une erreur est survenue lors de la suppression du rôle 'invité'.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "participant");
            if (!addRoleResult.Succeeded)
            {
                TempData["Error"] = "Une erreur est survenue lors de l'ajout du rôle 'participant'.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            TempData["Success"] = $"L'utilisateur {user.UserName} a été promu en tant que participant.";
>>>>>>> bde12b6c5d9686bfe0e65a3b351afa0f51353e7c
            return RedirectToAction(nameof(GetUsersWithRole));
        }

        // Action pour supprimer un utilisateur
        [Authorize(Roles = "administrateur")]
        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "ID invalide.";
                return RedirectToAction(nameof(GetUsersWithRoleP));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Utilisateur introuvable.";
                return RedirectToAction(nameof(GetUsersWithRoleP));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = $"L'utilisateur {user.UserName} a été supprimé avec succès.";
            }
            else
            {
                TempData["Error"] = "Une erreur est survenue lors de la suppression de l'utilisateur.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction(nameof(GetUsersWithRoleP));
        }

        // Action pour récupérer la liste des utilisateurs ayant le rôle "participant"
        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> GetUsersWithRoleP()
        {
            var users = _userManager.Users.ToList();
            var participants = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "participant"))
                {
                    participants.Add(user);
                }
            }

            return View(participants);
        }
    }
}
