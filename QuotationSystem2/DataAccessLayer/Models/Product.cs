using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal PipeDiscount { get; set; }

    public int TranslationID { get; set; }

    public virtual ICollection<ProductPipeTopMapping> ProductPipeTopMappings { get; set; } = new List<ProductPipeTopMapping>();

    public virtual ICollection<ProductTranslation> ProductTranslations { get; set; } = new List<ProductTranslation>();

    public virtual ICollection<SealingPlateRecord> SealingPlateRecords { get; set; } = new List<SealingPlateRecord>();

    public virtual ICollection<SheerStudRecord> SheerStudRecords { get; set; } = new List<SheerStudRecord>();

    public virtual TranslationGroup Translation { get; set; } = null!;
}
