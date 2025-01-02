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
        // Action pour récupérer la liste des utilisateurs ayant le rôle "invité"
        public async Task<IActionResult> GetUsersWithRole()
        {
            // Récupérer tous les utilisateurs
            var users = _userManager.Users.ToList();

            // Créer une liste pour stocker les utilisateurs avec le rôle "invité"
            var invitedUsers = new List<ApplicationUser>();

            // Vérifier pour chaque utilisateur s'il a le rôle "invité"
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "invité"))
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
            return RedirectToAction(nameof(GetUsersWithRole));
        }
    }
}
