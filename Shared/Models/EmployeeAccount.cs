using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class EmployeeAccount
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmpID { get; set; }
    [ForeignKey(nameof(EmpID))]
    public virtual Employee? Employee { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }       
    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, ErrorMessage = "Must be atleast 8 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? HashedPassword { get; set; } = "5fd16e765cd6ff4f2803eb1c84c155cdaea60f520de4eaafb2b858f80233deec4f93dc1e9f74bfc1675f189f8c198ff61e90f84218745ff11bce05bbf47bc93e";
    public bool IsActive { get; set; } = true;
    public Guid? ShopID { get; set; }
    public Guid RoleId { get; set; } 
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
    [ForeignKey(nameof(ShopID))]
    public virtual Shop? Shop { get; set; }
}