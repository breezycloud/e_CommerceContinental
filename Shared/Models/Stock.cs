using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class Stock
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Date field is required")]
    public DateTime Date { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Quantity is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }
    public Guid ProductID { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    [ForeignKey("ProductID")]
    public virtual Product? Product { get; set; }
}
