using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestForma.Models;
using GestForma.Services;

namespace GestForma.Controllers
{
    public class FormationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Formations.Include(f => f.User);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["ID_User"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Formations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Formation,Intitule,Description,Categorie,Duree,Cout,ID_User")] Formation formation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(formation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_User"] = new SelectList(_context.Users, "Id", "Id", formation.ID_User);
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
            ViewData["ID_User"] = new SelectList(
                _context.Users.Select(user => new
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName
                }),
                "Id",
                "FullName",
                formation.ID_User
            );
            return View(formation);
        }

        // POST: Formations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Formation,Intitule,Description,Categorie,Duree,Cout,ID_User")] Formation formation)
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
            ViewData["ID_User"] = new SelectList(
                 _context.Users.Select(user => new
                 {
                     Id = user.Id,
                     FullName = user.FirstName + " " + user.LastName
                 }),
                 "Id",
                 "FullName",
                 formation.ID_User
             );
            return View(formation);
        }

        // GET: Formations/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
