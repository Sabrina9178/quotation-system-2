using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.RecordModel;

public partial class WaterMeterRecordSocket
{
    public int SocketID { get; set; }

    public int WaterMeterID { get; set; }

    public int SocketDiameterID { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual WaterMeterRecord WaterMeter { get; set; } = null!;
}
