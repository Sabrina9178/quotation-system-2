using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class Component
{
    public int ComponentID { get; set; }

    public string ComponentName { get; set; } = null!;

    public int TranslationID { get; set; }

    public virtual ICollection<ComponentPrice> ComponentPrices { get; set; } = new List<ComponentPrice>();

    public virtual TranslationGroup Translation { get; set; } = null!;
}
