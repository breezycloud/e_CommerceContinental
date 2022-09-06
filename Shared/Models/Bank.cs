using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_CommerceContinental.Shared.Models;
public class Bank
{
    [Key]
    public Guid Id { get; set; }
    public string? BankCode { get; set; }
    [Required(ErrorMessage="Bank name is required")]
    public string? BankName { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();    
}

