using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class Stud
{
    public int StudID { get; set; }

    public string DisplayName { get; set; } = null!;

    public decimal Price { get; set; }
}
