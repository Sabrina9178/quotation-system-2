using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class PipeTopTranslation
{
    public int PipeTopID { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public virtual PipeTop PipeTop { get; set; } = null!;
}
