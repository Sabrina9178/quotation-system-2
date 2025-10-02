using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.SystemModel2;

public partial class TranslationGroup
{
    public int TranslationID { get; set; }

    public string? Dummy { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
