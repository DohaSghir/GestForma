﻿using System;
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
            var applicationDbContext = _context.Formations.Include(f => f.Categorie).Include(f => f.User);
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
            ViewData["ID_User"] = new SelectList(_context.Users, "Id", "Id", formation.ID_User);
            return View(formation);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Formation,Intitule,Description,Id_Categorie,Duree,Cout,ID_User,FileName,ContentType,Size,Data")] Formation formation)
        {
            if (id != formation.ID_Formation)
            {
                return NotFound();
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
            ViewData["Id_Categorie"] = new SelectList(_context.Categories, "Id", "Title", formation.Id_Categorie);
            ViewData["ID_User"] = new SelectList(_context.Users, "Id", "Id", formation.ID_User);
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


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Aucun fichier sélectionné.");
            }

            using (var memoryStream = new MemoryStream())
            {
               
                
                await file.CopyToAsync(memoryStream);

                var formation = new Formation
                {
                    Intitule ="h",
                    Description = "h",
                    Id_Categorie =1,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    Data = memoryStream.ToArray()
                };

                _context.Formations.Add(formation);
                await _context.SaveChangesAsync();
            }

            // Ajouter un message à afficher dans la vue
            ViewData["Message"] = "Image téléchargée avec succès.";
            return RedirectToAction("Index");
        }
    }
}
