
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Services;
using e_CommerceContinental.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace e_CommerceContinental.Client.Pages.Inventory;
public partial class StockReportDialog
{
    [Parameter] public Order[]? Orders { get; set; }
    [CascadingParameter]
    public MudDialogInstance? DialogInstance { get; set; }
    public string ReportType { get; set; } = "Date";
    bool isProcessing { get; set; } = false;
    private DateTime? startDate = DateTime.Today;
    private DateTime? endDate = DateTime.Today;

    void PrintReport()    
    {
        isProcessing = true;
        if (ReportType == "Date")
            AppState!.FilteredOrders = AppState!.Orders!.Where(x => x.OrderDate.Date == startDate!.Value.Date).ToArray();
        else
            AppState!.FilteredOrders = AppState!.Orders!.Where(x => x.OrderDate.Date >= startDate!.Value.Date && x.OrderDate.Date <= endDate!.Value.Date)
                                      .ToArray();    
        
        if (!AppState!.FilteredOrders.Any())
        {
            snackBar.Add("No records to display!", Severity.Warning);
            isProcessing = false;
        }            
        nav.NavigateTo("previewstockreport");   
        isProcessing = false;
    }

    private void Cancel() => DialogInstance!.Cancel();
}