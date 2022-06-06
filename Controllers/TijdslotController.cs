using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WdprPretparkDenhaag.Areas.Identity.Data;
using WdprPretparkDenhaag.Models;

namespace WdprPretparkDenhaag.Controllers
{
    [Authorize(Roles= "Admin")]
    public class TijdslotController : Controller
    {
        private readonly WdprPretparkDenhaagIdentityDbContext _context;

        public TijdslotController(WdprPretparkDenhaagIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Tijdslot
        public async Task<IActionResult> Index(string datum)
        {
            DateTime date = DateTime.Now.Date;

            if(!string.IsNullOrEmpty(datum)){
                date = DateTime.Parse(datum).Date;
            }

            var tijdsloten = _context.Tijdsloten.Where(u => u.BeginTijd.Date == date);
            ViewData["zoekDatum"] = date;

            return View(await tijdsloten.ToListAsync());
        }

        // GET: Tijdslot/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tijdslot = await _context.Tijdsloten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tijdslot == null)
            {
                return NotFound();
            }

            return View(tijdslot);
        }

        // GET: Tijdslot/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tijdslot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Datum")] TijdslotViewModel TijdslotModel)
        {
            if (ModelState.IsValid)
            {
                DateTime date = new DateTime(
                    TijdslotModel.Datum.Year,
                    TijdslotModel.Datum.Month,
                    TijdslotModel.Datum.Day,
                    9,0,0
                );

                for (int i = 0; i < 36; i++)
                {

                    int minutes = i * 15;
                    Tijdslot tijdslot = new Tijdslot();
                    tijdslot.BeginTijd = date.AddMinutes(minutes);
                    tijdslot.EindTijd = tijdslot.BeginTijd.AddMinutes(15);

                    _context.Tijdsloten.Add(tijdslot);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(TijdslotModel);
        }

        // GET: Tijdslot/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tijdslot = await _context.Tijdsloten.FindAsync(id);
            if (tijdslot == null)
            {
                return NotFound();
            }
            return View(tijdslot);
        }

        // POST: Tijdslot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,BeginTijd,EindTijd")] Tijdslot tijdslot)
        {
            if (id != tijdslot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tijdslot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TijdslotExists(tijdslot.Id))
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
            return View(tijdslot);
        }

        // GET: Tijdslot/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tijdslot = await _context.Tijdsloten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tijdslot == null)
            {
                return NotFound();
            }

            return View(tijdslot);
        }

        // POST: Tijdslot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tijdslot = await _context.Tijdsloten.FindAsync(id);
            _context.Tijdsloten.Remove(tijdslot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TijdslotExists(Guid id)
        {
            return _context.Tijdsloten.Any(e => e.Id == id);
        }
    }

}