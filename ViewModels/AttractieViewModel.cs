using System;
using System.Collections.Generic;
using WdprPretparkDenhaag.Models;

public class AttractieViewModel
{
    public Attractie Attractie { get; set; }
    public IEnumerable<Tijdslot> Tijdsloten { get; set; }
    
}