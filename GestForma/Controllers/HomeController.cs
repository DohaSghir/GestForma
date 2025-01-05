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
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        // Injection des services UserManager et RoleManager
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // Action Index
        public async Task<IActionResult> Index()
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
            // Si l'utilisateur n'est ni administrateur ni professeur, alors on récupère les formations disponibles
            var formations = await _context.Formations.ToListAsync();
            var actualites = await _context.Actualites.ToListAsync();
            var commentaires = await _context.CommentairesEntiers.Include(c => c.User).ToListAsync();

            // Passer la liste des formations à la vue via ViewBag
            ViewBag.Formations = formations;
            ViewBag.Actualites = actualites;
            ViewBag.Commentaires = commentaires;



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
        //Inscription d'un participant a une formation
        public async Task<IActionResult> Inscription(int formationId)
        {
            // Vérifier si l'utilisateur est connecté et a le rôle "participant"
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "participant"))
            {
                return RedirectToAction("ErreurInscription", "Home"); // Rediriger vers une page d'erreur si l'utilisateur n'est pas un participant
            }

            // Vérifier si la formation choisie existe
            var formation = await _context.Formations.FindAsync(formationId);
            if (formation == null)
            {
                return NotFound(); // Si la formation n'existe pas, renvoyer une erreur
            }

            // Créer l'inscription dans la table Inscription
            var inscription = new Inscription
            {
                ID_User = user.Id,           // ID de l'utilisateur
                ID_Formation = formationId,  // ID de la formation choisie
                Etat = false,                 // Vous pouvez personnaliser l'état de l'inscription si nécessaire
                Paiement = false,            // Paiement par défaut à false
            };

            // Ajouter l'inscription à la base de données
            _context.Inscriptions.Add(inscription);

            // Sauvegarder les changements dans la base de données
            await _context.SaveChangesAsync();

            // Rediriger l'utilisateur vers une autre page, par exemple le tableau de bord ou la confirmation
            return RedirectToAction("Confirmation", "Home"); // Ou une autre vue de confirmation
        }
        public IActionResult Confirmation()
        {
            return View();
        }
        public IActionResult ErreurInscription()
        {
            return View();
        }

        [Authorize(Roles = "participant")]
        [HttpPost]
        public async Task<IActionResult> AjouterCommentaire(string ContenuCommentaire)
        {
            // Vérifier que le contenu du commentaire n'est pas vide
            if (string.IsNullOrWhiteSpace(ContenuCommentaire))
            {
                TempData["Error"] = "Le commentaire ne peut pas être vide.";
                return RedirectToAction("Index"); // Redirige vers la page actuelle
            }

            // Obtenir l'utilisateur actuel
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Vous devez être connecté pour ajouter un commentaire.";
                return RedirectToAction("Index");
            }

            // Créer un nouveau commentaire
            var commentaire = new CommentairesEntiers
            {
                ContenuCommentaire = ContenuCommentaire,
                Id_User = user.Id,
                
            };

            // Ajouter le commentaire à la base de données
            _context.CommentairesEntiers.Add(commentaire);
            await _context.SaveChangesAsync();

            // Message de succès
            TempData["Success"] = "Votre commentaire a été ajouté avec succès.";
            return RedirectToAction("Index"); // Redirige vers la page actuelle
        }


    }
}
