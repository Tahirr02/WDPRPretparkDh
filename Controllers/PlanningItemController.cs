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
    public class PlanningItemController : Controller
    {
        private readonly WdprPretparkDenhaagIdentityDbContext _context;

        public PlanningItemController(WdprPretparkDenhaagIdentityDbContext context)
        {
            _context = context;
        }

        // GET: PlanningItem
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlanningItems.ToListAsync());
        }

        // GET: PlanningItem/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planningItem = await _context.PlanningItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planningItem == null)
            {
                return NotFound();
            }

            return View(planningItem);
        }

        // GET: PlanningItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanningItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanningId,TijdSlotId,AttractieId,Dag")] PlanningItem planningItem)
        {
            if (ModelState.IsValid)
            {
                planningItem.Id = Guid.NewGuid();
                _context.Add(planningItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planningItem);
        }

        // GET: PlanningItem/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planningItem = await _context.PlanningItems.FindAsync(id);
            if (planningItem == null)
            {
                return NotFound();
            }
            return View(planningItem);
        }

        // POST: PlanningItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PlanningId,TijdSlotId,AttractieId,Dag")] PlanningItem planningItem)
        {
            if (id != planningItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planningItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanningItemExists(planningItem.Id))
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
            return View(planningItem);
        }

        // GET: PlanningItem/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planningItem = await _context.PlanningItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planningItem == null)
            {
                return NotFound();
            }

            return View(planningItem);
        }

        // POST: PlanningItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var planningItem = await _context.PlanningItems.FindAsync(id);
            _context.PlanningItems.Remove(planningItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanningItemExists(Guid id)
        {
            return _context.PlanningItems.Any(e => e.Id == id);
        }
    }
}
