using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestForma.Models;
using GestForma.Services;
using Microsoft.VisualBasic;

namespace GestForma.Controllers
{
    public class ActualitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ActualitesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Actualites
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actualites.ToListAsync());
        }

        // GET: Actualites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actualite = await _context.Actualites
                .FirstOrDefaultAsync(m => m.IdActualite == id);
            if (actualite == null)
            {
                return NotFound();
            }

            return View(actualite);
        }

        // GET: Actualites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actualites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("IdActualite,Titre,Description")] Actualite actualite)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        actualite.FileName = file.FileName;
                        actualite.ContentType = file.ContentType;
                        actualite.Size = file.Length;
                        actualite.Data = memoryStream.ToArray();
                    }
                }
                _context.Add(actualite);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "News successfully created.";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
                ModelState.AddModelError("", error.ErrorMessage);
            }
            return View(actualite);
        }

        // GET: Actualites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actualite = await _context.Actualites.FindAsync(id);
            if (actualite == null)
            {
                return NotFound();
            }
            return View(actualite);
        }

        // POST: Actualites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile file, [Bind("IdActualite,Titre,Description")] Actualite actualite)
        {
            if (id != actualite.IdActualite)
            {
                return NotFound();
            }

            ModelState.Remove("file"); // Supprime la validation du fichier

            if (ModelState.IsValid)
            {
                try
                {
                    var existingActualite = await _context.Actualites.FirstOrDefaultAsync(a => a.IdActualite == id);

                    if (existingActualite == null)
                    {
                        return NotFound();
                    }

                    existingActualite.Titre = actualite.Titre;
                    existingActualite.Description = actualite.Description;

                    if (file != null && file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            existingActualite.FileName = file.FileName;
                            existingActualite.ContentType = file.ContentType;
                            existingActualite.Size = file.Length;
                            existingActualite.Data = memoryStream.ToArray();
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "News successfully edited.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActualiteExists(actualite.IdActualite))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(actualite);
        }

        // GET: Actualites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actualite = await _context.Actualites
                .FirstOrDefaultAsync(m => m.IdActualite == id);
            if (actualite == null)
            {
                return NotFound();
            }

            return View(actualite);
        }

        // POST: Actualites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actualite = await _context.Actualites.FindAsync(id);
            if (actualite != null)
            {
                _context.Actualites.Remove(actualite);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "News successfully deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ActualiteExists(int id)
        {
            return _context.Actualites.Any(e => e.IdActualite == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var actualite = await _context.Actualites.FindAsync(id);
            if (actualite == null)
            {
                return NotFound();
            }

            return File(actualite.Data, actualite.ContentType); // Retourner l'image avec le type MIME
        }
    }
}
