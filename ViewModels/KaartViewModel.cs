using System;
using System.Collections.Generic;
using WdprPretparkDenhaag.Models;

public class KaartViewModel
{
    public IEnumerable<PlanningItem> PlanningItems { get; set; }
    public IEnumerable<Tijdslot> Tijdsloten { get; set; }
    public IEnumerable<Attractie> Attracties { get; set; }
    
}