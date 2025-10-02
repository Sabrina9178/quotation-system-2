using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.SystemModel2;

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
}
