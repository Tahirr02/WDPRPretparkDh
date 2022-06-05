using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WdprPretparkDenhaag.Areas.Identity.Data;
using WdprPretparkDenhaag.Models;

namespace WdprPretparkDenhaag.Controllers
{
    public class KaartController : Controller
    {
        private readonly WdprPretparkDenhaagIdentityDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private IHubContext<NotifyHub> _hubContext { get; }

        public KaartController(
            WdprPretparkDenhaagIdentityDbContext context,
            IHubContext<NotifyHub> hubContext,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string attractieNaam)
        {
            var user = await _userManager.GetUserAsync(User);

            // haal de attracties op
            var attracties = from a in _context.Attracties select a;

            // Haal planningitems 
            var planningItems = _context.PlanningItems.Where(u => u.UserId == user.Id);

            // haal de attracties op die voldoen aan de filter van de searchbar
            if (!string.IsNullOrEmpty(attractieNaam))
            {
                attracties = attracties.
                                Where(attractie => attractie.Naam.Contains(attractieNaam));

                planningItems = planningItems.
                                    Where(u => u.Attractie.Naam.Contains(attractieNaam));
            }

            // IEnumerable<PlanningItem> planningItem = _context.PlanningItems;
            // maak een kaartview model aan die mee wordt gestuurd naar de kaartview
            KaartViewModel kaartViewModel = new KaartViewModel();
            kaartViewModel.Attracties = await attracties.ToListAsync();
            kaartViewModel.Tijdsloten = await _context.Tijdsloten.ToListAsync();
            kaartViewModel.PlanningItems = planningItems;

            // stuur de kaartviewmodel door naar de view
            return View(kaartViewModel);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractie =
                await _context.Attracties.SingleOrDefaultAsync(m => m.Id == id);
            if (attractie == null)
            {
                return NotFound();
            }

            var AttractieViewModel =
                new AttractieViewModel()
                {
                    Attractie = attractie,
                    Tijdsloten = await _context.Tijdsloten.ToListAsync()
                };

            ViewData["Attractie"] = AttractieViewModel;

            return View();
        }

        // Maak booking
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
        maakBooking(
            [Bind("AttractieId,Tijdslot,AantalPlekken")]
            BookingViewmodel booking
        )
        {
            Guid attractieId = Guid.Parse(booking.AttractieId);
            var user = await _userManager.GetUserAsync(User);
            var attractie = _context.Attracties.SingleOrDefault(a => a.Id == attractieId);

            if (ModelState.IsValid)
            {
                if (user.Leeftijd < attractie.MinimaleLeeftijd)
                {
                    return BadRequest("Bezoeker is te jong");
                }

                if ((attractie.Reserveercapaciteit - attractie.Reservaties) == 0)
                {
                    return BadRequest("Reservatie is Vol");
                }

                // Update reservaties
                attractie.Reservaties =
                    attractie.Reservaties + booking.AantalPlekken;
                var result = _context.Update(attractie).Entity;

                // Maak een planning item aan -> moet aangepast worden
                PlanningItem planningItem = new PlanningItem();
                planningItem.AttractieId = attractieId;
                planningItem.AantalPlekken = booking.AantalPlekken;
                planningItem.Prijs = booking.AantalPlekken * attractie.Prijs;
                planningItem.TijdSlotId = Guid.Parse(booking.Tijdslot);
                user.PlanningItems.Add(planningItem);

                await _context.SaveChangesAsync();

                // realtime update request
                int beschikbaarPlekken = attractie.Reserveercapaciteit - attractie.Reservaties;
                await _hubContext.Clients.All.SendAsync("ReceiveReservatie", beschikbaarPlekken);

                return Redirect("/kaart/index");
            }
            return Redirect("/kaart/details/" + attractieId);
        }
    }
}
