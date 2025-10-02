using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class PipeTopPrice
{
    public int DiameterID { get; set; }

    public int PipeTopID { get; set; }

    public decimal Price { get; set; }

    public virtual PipeDiameter Diameter { get; set; } = null!;

    public virtual PipeTop PipeTop { get; set; } = null!;
}
