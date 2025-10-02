using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class Stud
{
    public int StudID { get; set; }

    public string DisplayName { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<SheerStudRecord> SheerStudRecords { get; set; } = new List<SheerStudRecord>();
}
