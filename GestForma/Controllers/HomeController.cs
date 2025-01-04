using System.Diagnostics;
using GestForma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> ParticipantRoleSummary()
        {
            // Vérifier si le rôle "participant" existe
            var roleName = "participant";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                // Obtenir le nombre d'utilisateurs dans ce rôle
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                var userCount = usersInRole.Count();

                // Passer le résultat à la vue
                ViewBag.RoleName = roleName;
                ViewBag.UserCount = userCount;

                return View();
            }

            // Si le rôle "participant" n'existe pas
            ViewBag.ErrorMessage = "Le rôle 'participant' n'existe pas.";
            return View();
      
    }


        public async Task<IActionResult> ParticipantsAgeDistribution()
        {
            // Vérifier si le rôle "participant" existe
            var roleName = "participant";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                // Obtenir le nombre d'utilisateurs dans ce rôle
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                var userCount = usersInRole.Count();

                // Passer le résultat à la vue
                ViewBag.RoleName = roleName;
                ViewBag.UserCount = userCount;

               
            }
            // Vérifier si le rôle "participant" existe
           

            if (role != null)
            {
                // Obtenir les utilisateurs dans ce rôle
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

                // Extraire l'âge des utilisateurs
                var ageGroups = usersInRole
                    .Where(user => user.Age != null)  // Assurez-vous que la propriété d'âge est dans la base de données, par exemple BirthDate
                    .GroupBy(user => GetAgeGroup(user.Age))
                    .Select(group => new
                    {
                        AgeGroup = group.Key,
                        Count = group.Count()
                    })
                    .ToList();

                return View(ageGroups);
            }

            ViewBag.ErrorMessage = "Le rôle 'participant' n'existe pas.";
            return View();
        }

        // Fonction pour déterminer le groupe d'âge d'un utilisateur
        private string GetAgeGroup(int age)
        {
            
            if (age < 18) return "Moins de 18";
            if (age <= 25) return "18-25";
            if (age <= 35) return "26-35";
            if (age <= 45) return "36-45";
            if (age <= 60) return "46-60";
            return "60+";
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

        public IActionResult Courses()
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

        // Action pour r�cup�rer les utilisateurs ayant le r�le "invit�"
        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> GetUsersWithRole()
        {
            var users = _userManager.Users.ToList();
            var invitedUsers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "invité"))
                {
                    invitedUsers.Add(user);
                }
            }

            return View(invitedUsers);
        }

        // Action pour changer le r�le d'un utilisateur en "participant"
        [Authorize(Roles = "administrateur")]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleToParticipant(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Invalid ID.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "invité");
            if (!removeRoleResult.Succeeded)
            {
                TempData["Error"] = "An error occurred while deleting the 'guest' role.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "participant");
            if (!addRoleResult.Succeeded)
            {
                TempData["Error"] = "An error occurred while adding the 'participant' role.";
                return RedirectToAction(nameof(GetUsersWithRole));
            }

            TempData["Success"] = $"The user {user.UserName} has been promoted to participant.";
            return RedirectToAction(nameof(GetUsersWithRole));
        }

        // Action pour supprimer un utilisateur
        [Authorize(Roles = "administrateur")]
        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Invalid ID.";
                return RedirectToAction(nameof(GetUsersWithRoleP));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(GetUsersWithRoleP));
            }

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

            return RedirectToAction(nameof(GetUsersWithRoleP));
        }

        // Action pour r�cup�rer la liste des utilisateurs ayant le r�le "participant"
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
