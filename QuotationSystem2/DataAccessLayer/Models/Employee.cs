using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class Employee
{
    public int EmployeeID { get; set; }

    public string Name { get; set; } = null!;

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleID { get; set; }

    public DateTime LastLogin { get; set; }

    public bool IsRegistered { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SealingPlateRecord> SealingPlateRecords { get; set; } = new List<SealingPlateRecord>();

    public virtual ICollection<SheerStudRecord> SheerStudRecords { get; set; } = new List<SheerStudRecord>();

    public virtual ICollection<WaterMeterRecord> WaterMeterRecords { get; set; } = new List<WaterMeterRecord>();
}
