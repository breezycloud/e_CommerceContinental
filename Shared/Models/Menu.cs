using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class Menu
{
    public Menu()
    {
        SubMenus = new HashSet<SubMenu>();
    }

    [Key]
    public Guid Id { get; set; }
    public int Order { get; set; }
    [Required(ErrorMessage ="Menu name is required")]
    public string? MenuName { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    [NotMapped]
    public bool ShowSubMenus { get; set; } = false;
    public virtual ICollection<SubMenu>? SubMenus { get; set; }
}