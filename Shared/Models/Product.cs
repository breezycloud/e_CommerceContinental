using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Model No is required")]
    public string? Code { get; set; }
    [Required(ErrorMessage = "Product Name is required")]
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public string? ImageBase64 { get; set; }
    public string? Barcode { get; set; }
    [Required(ErrorMessage = "Price is required")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    [Required(ErrorMessage = "Quantity is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StocksOnHand { get; set; }
    public int ReorderLevel { get; set; }
    public Guid CategoryID { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    [ForeignKey("CategoryID")]
    public virtual Category? Category { get; set; }
    public Guid ShopID { get; set; }
    [ForeignKey(nameof(ShopID))]
    public virtual Shop? Shop { get; set; }
    public virtual List<Stock> Stocks { get; set; } = new();    
    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
}
