using System;
using System.ComponentModel.DataAnnotations;

public class TijdslotViewModel
{

    [Required]
    [DataType(DataType.Date)]
    public DateTime Datum { get; set; }
}
