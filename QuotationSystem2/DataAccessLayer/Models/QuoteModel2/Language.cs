using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class Language
{
    public int LanguageID { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
