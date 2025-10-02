using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class PipeThickness
{
    public int ThicknessID { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<PipePrice> PipePrices { get; set; } = new List<PipePrice>();

    public virtual ICollection<SealingPlateRecord> SealingPlateRecords { get; set; } = new List<SealingPlateRecord>();

    public virtual ICollection<WaterMeterRecord> WaterMeterRecords { get; set; } = new List<WaterMeterRecord>();
}
