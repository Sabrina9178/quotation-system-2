using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class Role
{
    public int RoleID { get; set; }

    public string RoleName { get; set; } = null!;

    public int TranslationID { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual TranslationGroup Translation { get; set; } = null!;
}
