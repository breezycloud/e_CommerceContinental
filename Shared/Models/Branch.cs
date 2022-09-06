using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace e_CommerceContinental.Shared.Models;

public class Branch
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage ="Branch Name is required")]
    public string? BranchName { get; set; } = "";
    public string? BranchAddress { get; set; }
    [Required(ErrorMessage ="Phone No is required")]
    public string? PhoneNo1 { get; set; }
    public string? PhoneNo2 { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public virtual List<Employee> Employees { get; set; } =new();
    public virtual List<Shop> Shops { get; set; } =new();
    [NotMapped]
    [JsonIgnore]
    public bool ShowShops { get; set; }


}