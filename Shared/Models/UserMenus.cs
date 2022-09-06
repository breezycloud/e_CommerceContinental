using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class UserMenus
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserID { get; set; }
    public Guid MenuID { get; set; }
    [ForeignKey(nameof(MenuID))]
    public virtual Menu? Menu { get; set; }
    [ForeignKey(nameof(UserID))]
    public virtual EmployeeAccount? Employee { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}