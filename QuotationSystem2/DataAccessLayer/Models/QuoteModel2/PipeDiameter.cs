using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class PipeDiameter
{
    public int DiameterID { get; set; }

    public decimal Diameter { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<ComponentPrice> ComponentPrices { get; set; } = new List<ComponentPrice>();

    public virtual ICollection<PipePrice> PipePrices { get; set; } = new List<PipePrice>();

    public virtual ICollection<PipeTopPrice> PipeTopPrices { get; set; } = new List<PipeTopPrice>();
}
