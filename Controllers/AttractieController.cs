using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WdprPretparkDenhaag.Areas.Identity.Data;
using WdprPretparkDenhaag.Models;

namespace WdprPretparkDenhaag.Controllers
{
    [Authorize(Roles= "Admin")]
    public class AttractieController : Controller
    {
        private readonly WdprPretparkDenhaagIdentityDbContext _context;
        
        public AttractieController(WdprPretparkDenhaagIdentityDbContext context)
        {
           _context = context;
        }

        // GET: Attractie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attracties.ToListAsync());
        }

        // GET: Attractie/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractie = await _context.Attracties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attractie == null)
            {
                return NotFound();
            }

            return View(attractie);
        }

        // GET: Attractie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attractie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,MinimaleLeeftijd,AngstFactor,Foto,MinimaleLengte,Prijs,AantalLikes,Inschrijfplicht,OpeningsTijd,SluitingsTijd,Reserveercapaciteit")] Attractie attractie)
        {
            if (ModelState.IsValid)
            {
                attractie.Id = Guid.NewGuid();
                _context.Add(attractie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attractie);
        }

        // GET: Attractie/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractie = await _context.Attracties.FindAsync(id);
            if (attractie == null)
            {
                return NotFound();
            }
            return View(attractie);
        }

        // POST: Attractie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Naam,MinimaleLeeftijd,AngstFactor,Foto,MinimaleLengte,Prijs,AantalLikes,Inschrijfplicht,OpeningsTijd,SluitingsTijd,Reserveercapaciteit")] Attractie attractie)
        {
            if (id != attractie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attractie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractieExists(attractie.Id))
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
            return View(attractie);
        }

        // GET: Attractie/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractie = await _context.Attracties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attractie == null)
            {
                return NotFound();
            }

            return View(attractie);
        }

        // POST: Attractie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attractie = await _context.Attracties.FindAsync(id);
            _context.Attracties.Remove(attractie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttractieExists(Guid id)
        {
            return _context.Attracties.Any(e => e.Id == id);
        }
    }
}
