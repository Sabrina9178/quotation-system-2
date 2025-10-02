using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class PipeThickness
{
    public int ThicknessID { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<PipePrice> PipePrices { get; set; } = new List<PipePrice>();
}
