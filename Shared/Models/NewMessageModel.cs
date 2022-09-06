namespace e_CommerceContinental.Shared.Models;
public class NewMessageModel
{
    public Guid OrderID { get; set; }
    public string? Recipient { get; set; }
    public string? Message { get; set; }
    public HashSet<string> Recipients { get; set; } = new HashSet<string>();
    public bool IsSent { get; set; } = false;
}