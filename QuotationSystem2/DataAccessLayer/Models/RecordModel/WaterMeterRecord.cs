using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.RecordModel;

public partial class WaterMeterRecord
{
    public int WaterMeterID { get; set; }

    public DateTime Date { get; set; }

    public int PipeDiameterID { get; set; }

    public int PipeThicknessID { get; set; }

    public decimal PipeLength { get; set; }

    public decimal PipePrice { get; set; }

    public int PipeTopID { get; set; }

    public int? PipeTopQuantity { get; set; }

    public decimal? PipeTopPrice { get; set; }

    public bool EndPlate { get; set; }

    public decimal EndPlatePrice { get; set; }

    public int CustomerID { get; set; }

    public decimal TotalPriceBeforeDiscount { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Note { get; set; }

    public string? Operator { get; set; }

    public decimal? Operand { get; set; }

    public decimal FinalPrice { get; set; }

    public int EmployeeID { get; set; }

    public virtual ICollection<WaterMeterRecordNipple> WaterMeterRecordNipples { get; set; } = new List<WaterMeterRecordNipple>();

    public virtual ICollection<WaterMeterRecordSocket> WaterMeterRecordSockets { get; set; } = new List<WaterMeterRecordSocket>();
}
