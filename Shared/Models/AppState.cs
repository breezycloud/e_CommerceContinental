

using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Shared.Models;
public class AppState
{        
    public event EventHandler? PrintDialog;
    public event Action? OnUpdate;
    public List<CartRow>? Rows { get; set; } = new();
    public Product[]? Products { get; set; } = Array.Empty<Product>();
    public CustomerWithOrder[]? Orders { get; set; } = Array.Empty<CustomerWithOrder>();
    public CustomerWithOrder[]? FilteredOrders { get; set; } = Array.Empty<CustomerWithOrder>();
    public ReportDataModel? ReportDataModel { get; set; } = new();

    public Task OnNotify()
    {
        OnUpdate?.Invoke();
        return Task.CompletedTask;
    }    

    public void OpenPrintDialog()
    {
        PrintDialog!.Invoke(null, EventArgs.Empty);
        
    }
}