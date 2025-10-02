using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class PipeDiameter
{
    public int DiameterID { get; set; }

    public decimal Diameter { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<ComponentPrice> ComponentPrices { get; set; } = new List<ComponentPrice>();

    public virtual ICollection<PipePrice> PipePrices { get; set; } = new List<PipePrice>();

    public virtual ICollection<PipeTopPrice> PipeTopPrices { get; set; } = new List<PipeTopPrice>();

    public virtual ICollection<SealingPlateRecord> SealingPlateRecordPipeDiameters { get; set; } = new List<SealingPlateRecord>();

    public virtual ICollection<SealingPlateRecord> SealingPlateRecordSocketDiameters { get; set; } = new List<SealingPlateRecord>();

    public virtual ICollection<SheerStudRecord> SheerStudRecords { get; set; } = new List<SheerStudRecord>();

    public virtual ICollection<WaterMeterRecordNipple> WaterMeterRecordNipples { get; set; } = new List<WaterMeterRecordNipple>();

    public virtual ICollection<WaterMeterRecordSocket> WaterMeterRecordSockets { get; set; } = new List<WaterMeterRecordSocket>();

    public virtual ICollection<WaterMeterRecord> WaterMeterRecords { get; set; } = new List<WaterMeterRecord>();
}
