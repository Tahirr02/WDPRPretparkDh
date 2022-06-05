

using System;
using System.ComponentModel.DataAnnotations;

public class BookingViewmodel {
    [Required]
    public string AttractieId { get; set; }
    [Required]
    public string Tijdslot { get; set; }
    [Required]
    public int AantalPlekken { get; set; }
}