using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace e_CommerceContinental.Shared.Models;

public class Department
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Department is required")]
    public string? DeptName { get; set; }    
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
}