using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestForma.Models;
using GestForma.Services;
using Microsoft.AspNetCore.Identity;

namespace GestForma.Controllers
{
    public class FormationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
       

        public FormationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Formations
        public async Task<IActionResult> Index()
        {
            
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(); // Retourner une erreur si l'utilisateur n'est pas connecté
                }

                var formations = _context.Formations
                    .Include(f => f.User)
                    .Where(f => f.ID_User == userId);

                return View(await formations.ToListAsync());
        }

        // GET: Formations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formations
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ID_Formation == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }

        // GET: Formations/Create
        public IActionResult Create()
        {
            // Pas besoin de ViewData pour ID_User
            return View();
        }

        // POST: Formations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Formation,Intitule,Description,Categorie,Duree,Cout")] Formation formation)
        {
            if (ModelState.IsValid)
            {
                // Récupérer l'ID de l'utilisateur connecté
                formation.ID_User = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(formation.ID_User))
                {
                    // Si l'utilisateur n'est pas authentifié, retournez une erreur
                    return Unauthorized();
                }

                // Ajouter la formation au contexte
                _context.Add(formation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Retourner la vue avec les données invalides
            return View(formation);
        }


        // GET: Formations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formations.FindAsync(id);
            if (formation == null)
            {
                return NotFound();
            }
            
            return View(formation);
        }

        // POST: Formations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Formation,Intitule,Description,Categorie,Duree,Cout")] Formation formation)
        {
            if (id != formation.ID_Formation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Récupérer l'ID de l'utilisateur connecté
                formation.ID_User = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(formation.ID_User))
                {
                    // Si l'utilisateur n'est pas authentifié, retournez une erreur
                    return Unauthorized();
                }

                try
                {
                    _context.Update(formation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormationExists(formation.ID_Formation))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(formation);
        }

        // GET: Formations/Delete/5
      /*  public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formations
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ID_Formation == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }*/

        public async Task<IActionResult> Delete(int? id)
        {
            // Vérification si l'ID est valide
            if (!id.HasValue)
            {
                TempData["Error"] = "Invalid ID.";
                return RedirectToAction(nameof(Index));
            }

            // Recherche de la formation à supprimer
            var formation = await _context.Formations
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ID_Formation == id.Value);

            if (formation == null)
            {
                TempData["Error"] = "Formation not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Suppression de l'entité
                _context.Formations.Remove(formation);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"The formation {formation.Intitule} has been successfully deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the formation.";
                // Vous pouvez ajouter des logs ici si nécessaire
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: Formations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formation = await _context.Formations.FindAsync(id);
            if (formation != null)
            {
                _context.Formations.Remove(formation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormationExists(int id)
        {
            return _context.Formations.Any(e => e.ID_Formation == id);
        }
    }
}
