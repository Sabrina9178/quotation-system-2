using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class Customer
{
    public int CustomerID { get; set; }

    public decimal Discount { get; set; }

    public int TranslationID { get; set; }

    public virtual TranslationGroup Translation { get; set; } = null!;
}
