using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class SheerStudRecord
{
    public int SheerStudID { get; set; }

    public DateTime Date { get; set; }

    public int PipeDiameterID { get; set; }

    public int ProductID { get; set; }

    public decimal PipeUnitPrice { get; set; }

    public decimal PipeDiscount { get; set; }

    public decimal PipeLength { get; set; }

    public decimal PipePrice { get; set; }

    public int StudSpecID { get; set; }

    public decimal StudUnitPrice { get; set; }

    public int StudQuantity { get; set; }

    public decimal StudPrice { get; set; }

    public int CustomerID { get; set; }

    public decimal CustomerDiscount { get; set; }

    public decimal TotalPriceBeforeDiscount { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Note { get; set; }

    public int? Operator { get; set; }

    public decimal? Operand { get; set; }

    public decimal FinalPrice { get; set; }

    public int EmployeeID { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Operator? OperatorNavigation { get; set; }

    public virtual PipeDiameter PipeDiameter { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Stud StudSpec { get; set; } = null!;
}
