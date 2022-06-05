using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WdprPretparkDenhaag.Areas.Identity.Data;

// using SignalRChat.Hubs;
public class NotifyHub : Hub
{
    private readonly WdprPretparkDenhaagIdentityDbContext _context;

    public NotifyHub(WdprPretparkDenhaagIdentityDbContext context){
        _context = context;
    }


    // public async Task SendMessage(string attractieId)
    // {
    //     Guid Id = Guid.Parse(attractieId);
    //     var capacity = _context.Attracties.SingleOrDefault(u => u.Id == Id).Reserveercapaciteit;
    //     await Clients.All.SendAsync("ReceiveMessage", capacity);
    // }

    public async Task Reserveer(string attractieId, int aantalPlekken)
    {
        Guid Id = Guid.Parse(attractieId);
        var attractie = _context.Attracties.SingleOrDefault(u => u.Id == Id);
        
        await Clients.All.SendAsync("ReceiveReservatie", attractie.Reservaties);
    }

    public async Task SendLike(string attractieId){
        Guid Id = Guid.Parse(attractieId);
        var attractie = _context.Attracties.SingleOrDefault(u => u.Id == Id);
        attractie.AantalLikes += 1;
        var result = _context.Update(attractie).Entity;
        _context.SaveChanges();
        await Clients.All.SendAsync("ReceiveLikes", attractie.AantalLikes);
    }

    public async Task getAvailable(String attractieId){
        Guid Id = Guid.Parse(attractieId);
        int Reserveercapaciteit = _context.Attracties.SingleOrDefault(u => u.Id == Id).Reserveercapaciteit;
        int Reservaties = _context.Attracties.SingleOrDefault(u => u.Id == Id).Reservaties;
        int beschikbaarPlekken = Reserveercapaciteit - Reservaties;
        await Clients.All.SendAsync("LoadPlekken", beschikbaarPlekken);
    }
}
