using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public partial class Customer
{

    [Key]
    public Guid CustomerId { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Customer Name field is required!")]
    public string? CustomerName { get; set; }

    [Required(ErrorMessage ="Phone Number field is required!")]
    [StringLength(13, MinimumLength= 11, ErrorMessage="Phone No must be atleast 11 digits")]        
    public string? PhoneNo { get; set; }
    [RegularExpression("^[A-Za-z0-9_\\+-]+(\\.[A-Za-z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    public string? OfficeAddress { get; set; }
    public string? ContactAddress { get; set; }
    public int Bday { get; set; }
    public int Bmonth { get; set; }
    public string? Religion { get; set; }
    public DateTime? CustomerSince { get; set; } = DateTime.Now;
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    [Range(0, 100, ErrorMessage = "Value for {0} must be between {0} and {2}.")]
    public double? Discount { get; set; } = 0;    
    public virtual List<Order> Orders { get; set; } = new();    
}