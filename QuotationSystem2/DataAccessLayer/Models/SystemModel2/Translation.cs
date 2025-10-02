using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.SystemModel2;

public partial class Translation
{
    public int TranslationID { get; set; }

    public int LanguageID { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual TranslationGroup TranslationNavigation { get; set; } = null!;
}
