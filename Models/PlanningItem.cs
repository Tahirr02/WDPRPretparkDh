using System;
using System.Collections.Generic;

namespace WdprPretparkDenhaag.Models
{
    public class PlanningItem
    {
        public Guid Id { get; set; }
        public Guid PlanningId { get; set; }
        public Guid TijdSlotId { get; set; }
        public DateTime Dag {get; set;}

        public double Prijs { get; set; }

        public int AantalPlekken { get; set; }

        public Guid AttractieId {get; set; }
        public Attractie Attractie { get; set; }
        public Tijdslot  Tijdslot {get; set;}

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
