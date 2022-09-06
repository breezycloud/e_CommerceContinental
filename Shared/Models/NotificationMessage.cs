using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class NotificationMessage
{
    [Key]
    public Guid id { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Category { get; set; }
    public DateTime PublishDate { get; set; }
    public Guid EmpID { get; set; }
    [ForeignKey(nameof(EmpID))]
    public virtual Employee? Employee { get; set; }
    public bool IsRead { get; set; }
}