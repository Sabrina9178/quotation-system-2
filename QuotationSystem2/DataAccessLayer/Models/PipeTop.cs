using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class PipeTop
{
    public int PipeTopID { get; set; }

    public string PipeTopName { get; set; } = null!;

    public int PricingMethod { get; set; }

    public int TranslationID { get; set; }

    public virtual ICollection<PipeTopPrice> PipeTopPrices { get; set; } = new List<PipeTopPrice>();

    public virtual ICollection<PipeTopTranslation> PipeTopTranslations { get; set; } = new List<PipeTopTranslation>();

    public virtual PipeTopPricingMethod PricingMethodNavigation { get; set; } = null!;

    public virtual ICollection<ProductPipeTopMapping> ProductPipeTopMappings { get; set; } = new List<ProductPipeTopMapping>();

    public virtual ICollection<SealingPlateRecord> SealingPlateRecords { get; set; } = new List<SealingPlateRecord>();

    public virtual TranslationGroup Translation { get; set; } = null!;

    public virtual ICollection<WaterMeterRecord> WaterMeterRecords { get; set; } = new List<WaterMeterRecord>();
}
