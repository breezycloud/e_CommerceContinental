using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class SubMenu
{
    [Key]
    public Guid Id { get; set; }
    public Guid MenuID { get; set; }
    public int Order { get; set; }
    [Required(ErrorMessage ="Sub Menu name is required")]
    public string? SubMenuName { get; set; }
    [ForeignKey(nameof(MenuID))]
    public virtual Menu? Menu { get; set; }
}