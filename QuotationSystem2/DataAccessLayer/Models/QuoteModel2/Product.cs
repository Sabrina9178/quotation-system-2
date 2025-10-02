using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class Product
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal PipeDiscount { get; set; }

    public int TranslationID { get; set; }

    public virtual ICollection<ProductPipeTopMapping> ProductPipeTopMappings { get; set; } = new List<ProductPipeTopMapping>();

    public virtual TranslationGroup Translation { get; set; } = null!;
}
