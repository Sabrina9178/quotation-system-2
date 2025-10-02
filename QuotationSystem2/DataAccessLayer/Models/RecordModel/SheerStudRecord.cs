using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.RecordModel;

public partial class SheerStudRecord
{
    public int SheerStudID { get; set; }

    public DateTime Date { get; set; }

    public int PipeDiameterID { get; set; }

    public int ProductID { get; set; }

    public decimal PipeLength { get; set; }

    public decimal PipePrice { get; set; }

    public int StudSpecID { get; set; }

    public int StudQuantity { get; set; }

    public decimal StudPrice { get; set; }

    public int CustomerID { get; set; }

    public decimal TotalPriceBeforeDiscount { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Note { get; set; }

    public string? Operator { get; set; }

    public decimal? Operand { get; set; }

    public decimal FinalPrice { get; set; }

    public int EmployeeID { get; set; }
}
