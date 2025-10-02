using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class Customer
{
    public int CustomerID { get; set; }

    public decimal Discount { get; set; }

    public int TranslationID { get; set; }

    public virtual ICollection<CustomerTranslation> CustomerTranslations { get; set; } = new List<CustomerTranslation>();

    public virtual ICollection<SealingPlateRecord> SealingPlateRecords { get; set; } = new List<SealingPlateRecord>();

    public virtual ICollection<SheerStudRecord> SheerStudRecords { get; set; } = new List<SheerStudRecord>();

    public virtual TranslationGroup Translation { get; set; } = null!;

    public virtual ICollection<WaterMeterRecord> WaterMeterRecords { get; set; } = new List<WaterMeterRecord>();
}
