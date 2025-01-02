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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult Instructors()
        {
            return View();
        }
        public IActionResult Testimonial()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        // Admin

        [Authorize(Roles = "administrateur")]
        public IActionResult AdminDashboard()
        {
            /*if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Login", "Account");  // Rediriger vers la page de connexion si l'utilisateur n'est pas un admin
            }
            */
            ViewData["Layout"] = "_LayoutAdmin";
            return View();
        }

        //Formateur

        [Authorize(Roles = "professeur")]
        public IActionResult FormateurDashboard()
        {
            /*if (!User.IsInRole("Formateur"))
            {
                return RedirectToAction("Login", "Account");  // Rediriger vers la page de connexion si l'utilisateur n'est pas un formateur
            }*/

            ViewData["Layout"] = "_LayoutFormateur";
            return View();
        }

        [Authorize(Roles = "administrateur")]
        // Action pour rï¿½cupï¿½rer la liste des utilisateurs ayant le rï¿½le "invitï¿½"
        public async Task<IActionResult> GetUsersWithRole()
        {
            // Rï¿½cupï¿½rer tous les utilisateurs
            var users = _userManager.Users.ToList();

            // Crï¿½er une liste pour stocker les utilisateurs avec le rï¿½le "invitï¿½"
            var invitedUsers = new List<ApplicationUser>();

            // Vï¿½rifier pour chaque utilisateur s'il a le rï¿½le "invitï¿½"
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "invitÃ©"))
                {
                    invitedUsers.Add(user); // Ajouter ï¿½ la liste si l'utilisateur est dans le rï¿½le "invitï¿½"
                }
            }

            // Retourner la liste des utilisateurs "invitï¿½s" ï¿½ la vue
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
            return View(invitedUsers);
        }


        [Authorize(Roles = "administrateur")]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleToParticipant(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            // Supprimer l'utilisateur du rï¿½le "invitï¿½"
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "invitÃ©");
            if (!removeRoleResult.Succeeded)
            {
                // Gï¿½rer l'ï¿½chec de la suppression du rï¿½le
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            // Ajouter l'utilisateur au rï¿½le "participant"
            var addRoleResult = await _userManager.AddToRoleAsync(user, "participant");
            if (!addRoleResult.Succeeded)
            {
                // Gï¿½rer l'ï¿½chec de l'ajout du rï¿½le
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            // Rediriger aprï¿½s la modification du rï¿½le
            return RedirectToAction(nameof(GetUsersWithRole));
        }
    }
}
