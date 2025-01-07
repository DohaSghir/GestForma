using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
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
        /*
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

            var users = _userManager.Users.ToList();

            List<List<Object>> trainers = new List<List<Object>>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "professeur"))
                {
                    var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id_user == user.Id);
                    string imageUrl = trainer != null ? Url.Action("GetImage", "Home", new { id = trainer.Id }) : null;
                    trainers.Add(new List<Object> { user.Id, user.LastName, user.FirstName, user.Email, user.PhoneNumber, trainer.Field, imageUrl });
                }
            }


            return View(trainers);
        }
        */
        public async Task<IActionResult> Index()
        {
            // Fetch and count users in different roles
            var participants = await _userManager.GetUsersInRoleAsync("participant");
            ViewBag.nbrPart = participants.Count;

            var professors = await _userManager.GetUsersInRoleAsync("professeur");
            ViewBag.nbrprof = professors.Count;

            var guests = await _userManager.GetUsersInRoleAsync("invité");
            ViewBag.nbrinv = guests.Count;

            // Count formations and categories
            ViewBag.nbrform = await _context.Formations.CountAsync();
            ViewBag.nbrcat = await _context.Categories.CountAsync();

            // Fetch formations, categories, and instructors
            var formations = await _context.Formations
                .Include(f => f.Categorie)
                .Include(f => f.User)
                .ToListAsync();
            ViewBag.formations1 = formations;

            var latestActualites = await _context.Actualites
                    .OrderByDescending(a => a.IdActualite) // Remplacez IdActualite par DatePublication si vous avez une colonne de date
                    .Take(5) // Limiter aux 5 derniers enregistrements
                    .ToListAsync();


            // Fetch latest news and comments
            ViewBag.Actualites = latestActualites ;
            var latestCommentaires = await _context.CommentairesEntiers
                .Include(c => c.User) // Assurez-vous de charger les données associées
                .OrderByDescending(c => c.IdCommentaire)
                .Take(5)
                .ToListAsync();

            ViewBag.Commentaires = latestCommentaires;


            // Fetch trainers
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "professeur");
            var trainers = await (from user in _context.Users
                                  join trainer in _context.Trainers on user.Id equals trainer.Id_user
                                  where _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id)
                                  select new
                                  {
                                      user.UserName,
                                      user.Email,
                                      user.FirstName,
                                      user.LastName,
                                      trainer.Data,
                                      trainer.ContentType,
                                      trainer.Field
                                  }).ToListAsync();
            ViewBag.Trainers = trainers;

            // Redirect based on user role
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

            // Calculate average ratings for formations
            var averageRates = await _context.Rates
                .GroupBy(r => r.ID_Formation)
                .Select(g => new
                {
                    ID_Formation = g.Key,
                    AverageRate = g.Average(r => r.ContenuRate),
                    TotalVotes = g.Count()
                })
                .ToDictionaryAsync(x => x.ID_Formation, x => new { x.AverageRate, x.TotalVotes });

            // Create model for the view
            var model = formations.Select(f => new
            {
                f.ID_Formation,
                Intitule = f.Intitule ?? "No Title Available",
                f.Duree,
                f.Cout,
                CategorieTitle = f.Categorie.Title ?? "Uncategorized",
                FormateurName = f.User.FirstName ?? "No Instructor Assigned",
                AverageRate = averageRates.ContainsKey(f.ID_Formation) ? averageRates[f.ID_Formation].AverageRate : 0,
                f.Data,
                f.ContentType
            }).ToList();

            return View(model);
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

        // Action Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {

            var participants = await _userManager.GetUsersInRoleAsync("participant");
            ViewBag.nbrPart = participants.Count;

            var professors = await _userManager.GetUsersInRoleAsync("professeur");
            ViewBag.nbrprof = professors.Count;

            var guests = await _userManager.GetUsersInRoleAsync("invité");
            ViewBag.nbrinv = guests.Count;

            // Count formations and categories
            ViewBag.nbrform = await _context.Formations.CountAsync();
            ViewBag.nbrcat = await _context.Categories.CountAsync();


            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> TestimonialAsync()
        {
            var latestCommentaires = await _context.CommentairesEntiers
                .Include(c => c.User) // Assurez-vous de charger les données associées
                .OrderByDescending(c => c.IdCommentaire)
                .Take(5)
                .ToListAsync();

            ViewBag.Commentaires = latestCommentaires;

            // Passer les données à la vue via ViewBag

            ViewBag.LatestCommentaires = latestCommentaires;
            return View();
        }
        public async Task<IActionResult> Instructors()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "professeur");
            var trainers = await (from user in _context.Users
                                  join trainer in _context.Trainers
                                  on user.Id equals trainer.Id_user
                                  where _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id)
                                  select new
                                  {
                                      user.UserName,
                                      user.Email,
                                      user.FirstName,
                                      user.LastName,
                                      trainer.Data,
                                      trainer.ContentType,
                                      trainer.Field
                                  }).ToListAsync();

            ViewBag.Trainers = trainers;
            return View();
        }

        public async Task<IActionResult> Payement()
        {
            var unpaidInscriptions = await _context.Inscriptions
        .Include(i => i.User)   // Make sure User is included in the query
        .Include(i => i.Formation)  // Make sure Formation is included in the query
        .Where(element => element.Paiement == false)
        .ToListAsync();
            return View(unpaidInscriptions);
        }

        // Action Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Actions Dashboard Admin 
        public IActionResult AdminDashboard(string search)
        {
            // Récupérer les inscriptions qui ne sont pas certifiées et qui n'ont pas la fin de formation
            var inscriptions = _context.Inscriptions
                .Include(i => i.User) // Inclure les données de l'utilisateur
                .Include(i => i.Formation) // Inclure les données de la formation
                .ThenInclude(f => f.Categorie) // Inclure les données de la catégorie
                .Where(i => i.Fin && i.Certificat == false)  // Exclure les inscriptions dont la formation est marquée comme finie
                .AsQueryable();

            // Si une recherche est effectuée, filtrer par nom de formation
            if (!string.IsNullOrEmpty(search))
            {
                inscriptions = inscriptions.Where(i => i.Formation.Intitule.Contains(search));
            }

            // Passer la liste filtrée à la vue
            return View(inscriptions.ToList());
        }

        // marquer la certificat pour chaque participant
        [HttpPost]
        public IActionResult MarkAsCertified(int inscriptionId)
        {
            try
            {
                // Récupérer l'inscription par son ID
                var inscription = _context.Inscriptions.FirstOrDefault(i => i.ID_Inscription == inscriptionId);

                if (inscription != null)
                {
                    // Marquer l'inscription comme certifiée
                    inscription.Certificat = true;

                    // Sauvegarder les modifications
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Training marked as certified successfully.";

                // Rediriger vers la page d'administration
                return RedirectToAction("AdminDashboard");
            }
            catch (Exception ex)
            {
                // Ajouter un message d'erreur en cas d'exception
                TempData["ErrorMessage"] = "An error occurred while marking the training as certified.";

                // Rediriger vers la page d'administration
                return RedirectToAction("AdminDashboard");
            }
        }



        // Actions Dashboard Professeur
        [Authorize(Roles = "professeur")]
        public IActionResult FormateurDashboard(string search)
        {
            // Récupérer toutes les inscriptions sauf celles certifiées
            var inscriptions = _context.Inscriptions
                .Include(i => i.User) // Inclure les données de l'utilisateur
                .Include(i => i.Formation) // Inclure les données de la formation
                .ThenInclude(f => f.Categorie) // Inclure les données de la catégorie
                .Where(i => !i.Fin && i.Paiement == true)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                // Filtrer par nom du cours
                inscriptions = inscriptions.Where(i => i.Formation.Intitule.Contains(search));
            }

            // Passer les données à la vue
            return View(inscriptions.ToList());


        }

        //marquer la fin de Formation par le formateur
        [HttpPost]
        public IActionResult MarkAsComplete(int inscriptionId)
        { 
            try
            {
                var inscription = _context.Inscriptions.FirstOrDefault(i => i.ID_Inscription == inscriptionId);
                if (inscription != null)
                {
                    inscription.Fin = true;
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Training marked as completed successfully.";
                return RedirectToAction("FormateurDashboard"); // Remplacez "Index" par la vue où vous voulez revenir
            }
            catch (Exception ex)
            {
                // Ajouter un message d'erreur en cas d'exception
                TempData["ErrorMessage"] = "An error occurred while marking the training as complete.";
                return RedirectToAction("FormateurDashboard"); // Redirigez vers la vue appropriée
            }
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
                TempData["Error"] = "The comment cannot be empty.";
                return RedirectToAction("Index"); // Redirige vers la page actuelle
            }

            // Obtenir l'utilisateur actuel
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "You must be logged in to add a comment.";
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
            TempData["Success"] = "Your comment has been successfully added.";
            return RedirectToAction("Index"); // Redirige vers la page actuelle
        }

        [Authorize(Roles = "participant")]
        [HttpPost]
        public async Task<IActionResult> AjouterCommentaireT(string ContenuCommentaire)
        {
            // Vérifier que le contenu du commentaire n'est pas vide
            if (string.IsNullOrWhiteSpace(ContenuCommentaire))
            {
                TempData["Error"] = "The comment cannot be empty.";
                return RedirectToAction("Testimonial"); // Redirige vers la page actuelle
            }

            // Obtenir l'utilisateur actuel
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "You must be logged in to add a comment.";
                return RedirectToAction("Testimonial");
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
            TempData["Success"] = "Your comment has been successfully added.";
            return RedirectToAction("Testimonial"); // Redirige vers la page actuelle
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
