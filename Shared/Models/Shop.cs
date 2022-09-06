using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class Shop
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Shop name")]
    public string? ShopName { get; set; }
    public Guid BranchID { get; set; }
    [ForeignKey(nameof(BranchID))]
    public Branch? Branch { get; set; }
    public DateTime? ModifiedDate { get; set; }
}