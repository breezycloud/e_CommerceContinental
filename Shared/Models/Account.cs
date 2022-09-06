using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace e_CommerceContinental.Shared.Models;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage ="Code is required!")]
    public int? Code { get; set; }
    [Required(ErrorMessage ="Name is required!")]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string? AccountName { get; set; }
}