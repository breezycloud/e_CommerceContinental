using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace e_CommerceContinental.Shared.Models;

public class Order
{    
    [Key]
    public Guid Id { get; set; }
    public int InvoiceNo { get; set; }
    public DateTime TransactionDate { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public Guid CustomerID { get; set; }
    public Guid EmpID { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }
    [NotMapped]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal => TotalAmount - Discount;
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
    [NotMapped]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance => SubTotal - AmountPaid;
    [Column(TypeName = "decimal(18,2)")]
    public decimal VAT { get; set; }
    public string? PaymentMode { get; set; }
    public bool IsMessageSent { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    [ForeignKey(nameof(EmpID))]    
    public virtual Employee? Employee { get; set; }    

    [ForeignKey(nameof(CustomerID))]
    public virtual Customer? Customer { get; set; }    
    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
    public virtual ICollection<OrderPayment> Payments { get; set; } = new HashSet<OrderPayment>();           
}
