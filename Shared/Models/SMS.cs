#nullable disable

namespace e_CommerceContinental.Shared.Models;

public partial class SMS
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string SenderName { get; set; }
}