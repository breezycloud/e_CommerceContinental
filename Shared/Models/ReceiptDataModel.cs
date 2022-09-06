using e_CommerceContinental.Shared.Models;

public class ReportDataModel
{
    public Employee? Employee { get; set; }
    public Customer? Customer { get; set; }
    public Order? Order {get; set; }    
    public string? PrevPage { get; set; }
}