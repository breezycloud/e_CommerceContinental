using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class UserSubMenus
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserID { get; set; }
    public Guid SubMenuID { get; set; }
    [ForeignKey(nameof(SubMenuID))]
    public virtual SubMenu? SubMenu { get; set; }
    [ForeignKey(nameof(UserID))]
    public virtual EmployeeAccount? Employee { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}