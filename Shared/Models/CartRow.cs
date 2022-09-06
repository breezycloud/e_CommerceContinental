using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;
public class CartRow
{
    public decimal GrandTotal { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total => (decimal)(Quantity) * Cost;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; } = 0;   
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0;   
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost => Product is null || UnitPrice > 0 ? UnitPrice : Product!.UnitPrice;
    public Product? Product { get; set; } = new();
}
