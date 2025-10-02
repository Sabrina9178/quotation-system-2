using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.RecordModel;

public partial class SealingPlateRecord
{
    public int SealingPlateID { get; set; }

    public DateTime Date { get; set; }

    public int PipeDiameterID { get; set; }

    public int ProductID { get; set; }

    public int PipeThicknessID { get; set; }

    public decimal PipeLength { get; set; }

    public decimal PipePrice { get; set; }

    public int PipeTopID { get; set; }

    public int? PipeTopQuantity { get; set; }

    public decimal? PipeTopPrice { get; set; }

    public int SocketDiameterID { get; set; }

    public int? SocketQuantity { get; set; }

    public int WeldingQuantity { get; set; }

    public decimal SocketPrice { get; set; }

    public decimal SealingPlatePrice { get; set; }

    public int CustomerID { get; set; }

    public decimal TotalPriceBeforeDiscount { get; set; }

    public double TotalPrice { get; set; }

    public string? Note { get; set; }

    public string? Operator { get; set; }

    public decimal? Operand { get; set; }

    public decimal FinalPrice { get; set; }

    public int EmployeeID { get; set; }
}
