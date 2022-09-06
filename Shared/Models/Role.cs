using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace e_CommerceContinental.Shared.Models;

public partial class Role
{
    public Role()
    {
        EmployeeAccounts = new HashSet<EmployeeAccount>();
    }

    [Key]
    public Guid RoleId { get; set; }
    public string RoleType { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public virtual ICollection<EmployeeAccount> EmployeeAccounts { get; set; }
}
