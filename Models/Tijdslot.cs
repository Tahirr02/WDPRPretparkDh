using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WdprPretparkDenhaag.Models
{
    public class Tijdslot
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime BeginTijd { get; set; }
        public  DateTime EindTijd { get; set; }

        public List<PlanningItem> PlanningItem { get; set; }
    }
}