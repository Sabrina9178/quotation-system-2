using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class WaterMeterRecordSocket
{
    public int SocketID { get; set; }

    public int WaterMeterID { get; set; }

    public int SocketDiameterID { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual PipeDiameter SocketDiameter { get; set; } = null!;

    public virtual WaterMeterRecord WaterMeter { get; set; } = null!;
}
