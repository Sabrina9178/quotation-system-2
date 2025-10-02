using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class WaterMeterRecordNipple
{
    public int NippleID { get; set; }

    public int WaterMeterID { get; set; }

    public int NippleDiameterID { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual PipeDiameter NippleDiameter { get; set; } = null!;

    public virtual WaterMeterRecord WaterMeter { get; set; } = null!;
}
