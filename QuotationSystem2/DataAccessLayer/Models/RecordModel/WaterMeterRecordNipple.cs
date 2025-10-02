using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.RecordModel;

public partial class WaterMeterRecordNipple
{
    public int NippleID { get; set; }

    public int WaterMeterID { get; set; }

    public int NippleDiameterID { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual WaterMeterRecord WaterMeter { get; set; } = null!;
}
