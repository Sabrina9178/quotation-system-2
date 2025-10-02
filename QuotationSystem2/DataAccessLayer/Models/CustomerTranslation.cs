using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class CustomerTranslation
{
    public int CustomerID { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
