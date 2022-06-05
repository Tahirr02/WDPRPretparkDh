using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WdprPretparkDenhaag.Areas.Identity.Data;
using WdprPretparkDenhaag.Models;

namespace WdprPretparkDenhaag.Controllers
{
    public class PlanningController : Controller
    {
        private readonly WdprPretparkDenhaagIdentityDbContext _context;

        public PlanningController(WdprPretparkDenhaagIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Planning
        public async Task<IActionResult> Index()
        {
            return View(await _context.Planningen.ToListAsync());
        }

        // GET: Planning/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planning = await _context.Planningen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planning == null)
            {
                return NotFound();
            }

            return View(planning);
        }

        // GET: Planning/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planning/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dag,TotaleKosten,BezoekersId")] Planning planning)
        {
            if (ModelState.IsValid)
            {
                planning.Id = Guid.NewGuid();
                _context.Add(planning);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planning);
        }

        // GET: Planning/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planning = await _context.Planningen.FindAsync(id);
            if (planning == null)
            {
                return NotFound();
            }
            return View(planning);
        }

        // POST: Planning/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Dag,TotaleKosten,BezoekersId")] Planning planning)
        {
            if (id != planning.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planning);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanningExists(planning.Id))
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
            return View(planning);
        }

        // GET: Planning/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planning = await _context.Planningen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planning == null)
            {
                return NotFound();
            }

            return View(planning);
        }

        // POST: Planning/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var planning = await _context.Planningen.FindAsync(id);
            _context.Planningen.Remove(planning);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanningExists(Guid id)
        {
            return _context.Planningen.Any(e => e.Id == id);
        }
    }
}
