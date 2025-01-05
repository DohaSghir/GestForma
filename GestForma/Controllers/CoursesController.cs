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
using Microsoft.VisualBasic;

namespace GestForma.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
            {
                _context = context;
                _userManager = userManager;
                _webHostEnvironment = webHostEnvironment;
            }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            
            var userId = _userManager.GetUserId(User);

            var applicationDbContext = _context.Formations
                .Where(f => f.ID_User == userId) 
                .Include(f => f.Categorie)       
                .Include(f => f.User)           
                .AsQueryable();

      
            return View(await applicationDbContext.ToListAsync());
        }

      

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formations
                .Include(f => f.Categorie)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ID_Formation == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var formation = await _context.Formations.FindAsync(id);
            if (formation == null)
            {
                return NotFound();
            }

            return File(formation.Data,formation.ContentType);  // Retourner l'image avec le type MIME
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["Id_Categorie"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("ID_Formation,Intitule,Description,Id_Categorie,Duree,Cout")] Formation formation)
        {


            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        formation.FileName = file.FileName;
                        formation.ContentType = file.ContentType;
                        formation.Size = file.Length;
                        formation.Data = memoryStream.ToArray();
                    }
                }

                formation.ID_User = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(formation.ID_User))
                {
                    return Unauthorized();
                }
                
                _context.Add(formation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
                ModelState.AddModelError("", error.ErrorMessage);
            }

            ViewData["Id_Categorie"] = new SelectList(_context.Categories, "Id", "Title", formation.Id_Categorie);
            return View(formation);
        }



        // GET: Courses/Edit/5
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
            ViewData["Id_Categorie"] = new SelectList(_context.Categories, "Id", "Title", formation.Id_Categorie);
            return View(formation);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,IFormFile file, [Bind("ID_Formation,Intitule,Description,Id_Categorie,Duree,Cout")] Formation formation)
        {
            if (id != formation.ID_Formation)
            {
                return NotFound();
            }
            if (file != null && file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            formation.FileName = file.FileName;
                            formation.ContentType = file.ContentType;
                            formation.Size = file.Length;
                            formation.Data = memoryStream.ToArray();
                        }
                    }

            if (ModelState.IsValid)
            {
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
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
                ModelState.AddModelError("", error.ErrorMessage);
            }
            ViewData["Id_Categorie"] = new SelectList(_context.Categories, "Id", "Title", formation.Id_Categorie);
            return View(formation);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formations
                .Include(f => f.Categorie)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ID_Formation == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }

        // POST: Courses/Delete/5
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


        public async Task<IActionResult> Courses(string category, string keyword)
        {
            // Charger toutes les catégories pour la liste déroulante
            var categories = await _context.Categories
                .Select(c => c.Title) // Accéder directement au modèle Category
                .Distinct()
                .ToListAsync();
            ViewBag.Categories = categories;

            // Charger les formations avec leurs catégories et les formateurs
            var formations = _context.Formations
                .Include(f => f.Categorie) // Charger la catégorie associée
                .Include(f => f.User)      // Charger le formateur associé
                .AsQueryable();

            // Appliquer les filtres si les paramètres sont présents
            if (!string.IsNullOrEmpty(category))
            {
                formations = formations.Where(f => f.Categorie.Title.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                formations = formations.Where(f => f.Intitule.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            // Retourner les formations filtrées à la vue
            return View(await formations.ToListAsync());
        }





    }
}
