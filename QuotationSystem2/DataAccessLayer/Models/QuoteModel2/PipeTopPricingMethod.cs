using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class PipeTopPricingMethod
{
    public int MethodID { get; set; }

    public string MethodName { get; set; } = null!;

    public int TranslationID { get; set; }

    public virtual ICollection<PipeTop> PipeTops { get; set; } = new List<PipeTop>();

    public virtual TranslationGroup Translation { get; set; } = null!;
}
