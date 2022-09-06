using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;
public class OrderPayment
{
    [Key]
    public Guid Id { get; set; }
    public DateTime PaymentDate { get; set; }    
    public Guid OrderID { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
    [ForeignKey(nameof(OrderID))]
    public virtual Order? Order { get; set; }
}