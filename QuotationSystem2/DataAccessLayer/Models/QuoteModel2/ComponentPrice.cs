using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class ComponentPrice
{
    public int DiameterID { get; set; }

    public int ComponentID { get; set; }

    public decimal Price { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual PipeDiameter Diameter { get; set; } = null!;
}
