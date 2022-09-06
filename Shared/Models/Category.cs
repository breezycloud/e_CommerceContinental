using System.ComponentModel.DataAnnotations;

namespace e_CommerceContinental.Shared.Models;

public class Category
{

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Name is required")]
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    public virtual List<Product> Products { get; set; } = new();
}
