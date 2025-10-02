using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class SealingPlateRecord
{
    public int SealingPlateID { get; set; }

    public DateTime Date { get; set; }

    public int PipeDiameterID { get; set; }

    public int ProductID { get; set; }

    public int PipeThicknessID { get; set; }

    public decimal PipeUnitPrice { get; set; }

    public decimal PipeDiscount { get; set; }

    public decimal PipeLength { get; set; }

    public decimal PipePrice { get; set; }

    public int PipeTopID { get; set; }

    public decimal? PipeTopUnitPrice { get; set; }

    public int? PipeTopQuantity { get; set; }

    public decimal? PipeTopPrice { get; set; }

    public int SocketDiameterID { get; set; }

    public decimal? SocketUnitPrice { get; set; }

    public decimal WeldingUnitPrice { get; set; }

    public int? SocketQuantity { get; set; }

    public int WeldingQuantity { get; set; }

    public decimal SocketPrice { get; set; }

    public decimal SealingPlatePrice { get; set; }

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

    public virtual PipeThickness PipeThickness { get; set; } = null!;

    public virtual PipeTop PipeTop { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual PipeDiameter SocketDiameter { get; set; } = null!;
}
