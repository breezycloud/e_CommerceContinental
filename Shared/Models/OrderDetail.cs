using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace e_CommerceContinental.Shared.Models;

public class OrderDetails
{
    [Key]
    public Guid Id { get; set; }
    public Guid OrderID { get; set; }
    public Guid ProductID { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal ItemPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    [ForeignKey(nameof(OrderID))]
    public virtual Order? Order { get; set; }
    [ForeignKey(nameof(ProductID))]
    public virtual Product? Product { get; set; }
}

