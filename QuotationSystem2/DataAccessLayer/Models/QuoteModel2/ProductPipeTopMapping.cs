using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class ProductPipeTopMapping
{
    public int ProductID { get; set; }

    public int PipeTopID { get; set; }

    public bool? IsExist { get; set; }

    public virtual PipeTop PipeTop { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
