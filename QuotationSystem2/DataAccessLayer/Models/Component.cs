using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class Component
{
    public int ComponentID { get; set; }

    public string ComponentName { get; set; } = null!;

    public int TranslationID { get; set; }

    public virtual ICollection<ComponentPrice> ComponentPrices { get; set; } = new List<ComponentPrice>();

    public virtual ICollection<ComponentTranslation> ComponentTranslations { get; set; } = new List<ComponentTranslation>();

    public virtual TranslationGroup Translation { get; set; } = null!;
}
