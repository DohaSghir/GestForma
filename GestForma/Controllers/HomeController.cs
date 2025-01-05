using System.Diagnostics;
using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestForma.Controllers
{
    public class HomeController : Controller
    {
        private const string RoleProfesseur = "professeur";
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Injection des services UserManager et RoleManager
        public HomeController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Action Index
        public async Task<IActionResult> IndexAsync()
        {
          
            // Charger les formations avec leurs catégories et les formateurs
            var formations = _context.Formations
                .Include(f => f.Categorie) // Charger la catégorie associée
                .Include(f => f.User)      // Charger le formateur associé
                .AsQueryable();

            var par = await _userManager.GetUsersInRoleAsync("participant");

            var nbrPart = par.Count;

            ViewBag.nbrPart = nbrPart;

            var prof = await _userManager.GetUsersInRoleAsync("professeur");

            var nbrprof = prof.Count;

            ViewBag.nbrprof = nbrprof;


            var inv = await _userManager.GetUsersInRoleAsync("invité");


            var nbrinv = inv.Count;

            ViewBag.nbrinv = nbrinv;

            var nbrform = await _context.Formations.CountAsync();
            ViewBag.nbrform = nbrform;
            var nbrcat = await _context.Categories.CountAsync();
            ViewBag.nbrcat = nbrcat;

            var formations1 = await _context.Formations.ToListAsync();
            ViewBag.formations1 = formations1;



            // Récupérer l'ID du rôle "Professeur"
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == RoleProfesseur);

               var trainer = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == role.Id))
                .ToListAsync();

            ViewBag.trainers = trainer;

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
            return View(await formations.ToListAsync());
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

        public async Task<IActionResult> Statistic()
        {
            

            var par = await _userManager.GetUsersInRoleAsync("participant");

            var nbrPart = par.Count;

            ViewBag.nbrPart = nbrPart;

            var prof = await _userManager.GetUsersInRoleAsync("professeur");

            var nbrprof = prof.Count;

            ViewBag.nbrprof = nbrprof;


            var inv = await _userManager.GetUsersInRoleAsync("invité");

           
            var nbrinv = inv.Count;

            ViewBag.nbrinv = nbrinv;

            var totalNumberOfFormations = await _context.Formations.CountAsync();

            
            ViewBag.TotalNumberOfFormations = totalNumberOfFormations;

            var role = await _roleManager.FindByNameAsync("participant");   

            if (role != null)
            {
                // Obtenir les utilisateurs dans ce rôle
                var usersInRole = await _userManager.GetUsersInRoleAsync("participant");

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

       
        private string GetAgeGroup(int age)
        {

            if (age < 18) return "Moins de 18";
            if (age <= 25) return "18-25";
            if (age <= 35) return "26-35";
            if (age <= 45) return "36-45";
            if (age <= 60) return "46-60";
            return "60+";
        }


    }
}
