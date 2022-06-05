using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using WdprPretparkDenhaag.Areas.Identity.Data; 
using WdprPretparkDenhaag.Models;


namespace WdprPretparkDenhaag
{
    public class Program
    {

        public static void Main(string[] args)
        {
          var builder = CreateHostBuilder(args).Build();
           using (var scope = builder.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                using (var roleManager1 = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    var roles = new string[] { "Admin", "Bezoeker", "mohamed" };
                    foreach (var role in roles)
                        if (!roleManager1.RoleExistsAsync(role).Result)
                            roleManager1.CreateAsync(new IdentityRole(role)).Wait();
                }
                //  GenereerAttracties(scope);
                //   GenereerTijdssloten(scope);       
            }
            
          builder.Run();                        
        }

        private static void GenereerAttracties(IServiceScope scope)
        {
            Attractie attractie = new Attractie();
                attractie.Naam = "Cycloon Achtbaan";
                attractie.AngstFactor = 9;
                attractie.MinimaleLeeftijd = 12;
                attractie.MinimaleLengte = 150;
                attractie.Prijs = 4.50;
                attractie.Inschrijfplicht = true;
                attractie.Reserveercapaciteit = 15;
                var roleManager = scope.ServiceProvider.GetRequiredService<WdprPretparkDenhaagIdentityDbContext>();
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "Draaimolen";
                attractie.AngstFactor = 2;
                attractie.MinimaleLeeftijd = 5;
                attractie.MinimaleLengte = 100;
                attractie.Prijs = 4.25;
                attractie.Inschrijfplicht = false;
                attractie.Reserveercapaciteit = 25;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "Reuzenrad";
                attractie.AngstFactor = 2;
                attractie.MinimaleLeeftijd = 8;
                attractie.MinimaleLengte = 100;
                attractie.Prijs = 3.00;
                attractie.Inschrijfplicht = true;
                attractie.Reserveercapaciteit = 50;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "clowns";
                attractie.AngstFactor = 4;
                attractie.MinimaleLeeftijd = 5;
                attractie.MinimaleLengte = 80;
                attractie.Prijs = 5.00;
                attractie.Inschrijfplicht = false;
                attractie.Reserveercapaciteit = 10;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "acrobaten en zangers ";
                attractie.AngstFactor = 0;
                attractie.MinimaleLeeftijd = 5;
                attractie.MinimaleLengte = 100;
                attractie.Prijs = 5.00;
                attractie.Inschrijfplicht = true;
                attractie.Reserveercapaciteit = 250;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "Ijskraam";
                attractie.AngstFactor = 0;
                attractie.MinimaleLeeftijd = 2;
                attractie.MinimaleLengte = 30;
                attractie.Prijs = 3.50;
                attractie.Inschrijfplicht = false;
                attractie.Reserveercapaciteit = 100;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "Restaurant 'T haagse keukentje";
                attractie.AngstFactor = 0;
                attractie.MinimaleLeeftijd = 0;
                attractie.MinimaleLengte = 0;
                attractie.Prijs = 10;
                attractie.Inschrijfplicht = true;
                attractie.Reserveercapaciteit = 150;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

                attractie = new Attractie();
                attractie.Naam = "Speeltuin";
                attractie.AngstFactor = 0;
                attractie.MinimaleLeeftijd = 0;
                attractie.MinimaleLengte = 0;
                attractie.Prijs = 5.00;
                attractie.Inschrijfplicht = false;
                attractie.Reserveercapaciteit = 180;
                roleManager.Attracties.Add(attractie);
                roleManager.SaveChanges();

        }

       private static void GenereerTijdssloten(IServiceScope scope)
       {
             for (int i = 0; i < 36; i++)
               {
                   DateTime date = new DateTime(2000, 1, 1, 9, 0, 0);
                                      
                   int minutes = i * 15;
                    Tijdslot tijdslot = new Tijdslot();
                    tijdslot.BeginTijd = date.AddMinutes(minutes);
                    tijdslot.EindTijd = tijdslot.BeginTijd.AddMinutes(15);
                    var dbcontext = scope.ServiceProvider.GetRequiredService<WdprPretparkDenhaagIdentityDbContext>();
                                        
                    dbcontext.Tijdsloten.Add(tijdslot);
                    dbcontext.SaveChanges();
               }
       }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
