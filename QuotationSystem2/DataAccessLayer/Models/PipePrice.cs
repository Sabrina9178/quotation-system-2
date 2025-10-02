using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class PipePrice
{
    public int DiameterID { get; set; }

    public int ThicknessID { get; set; }

    public decimal PipeUnitPrice { get; set; }

    public virtual PipeDiameter Diameter { get; set; } = null!;

    public virtual PipeThickness Thickness { get; set; } = null!;
}
