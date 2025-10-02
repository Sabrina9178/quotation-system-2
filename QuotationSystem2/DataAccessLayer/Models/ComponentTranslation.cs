using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class ComponentTranslation
{
    public int ComponentID { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public virtual Component Component { get; set; } = null!;
}
