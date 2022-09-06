using e_CommerceContinental.Client.Data;
using e_CommerceContinental.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace e_CommerceContinental.Client.Shared;

public partial class AppBar
{
    [Parameter] public EventCallback<MouseEventArgs> DrawerToggleCallback { get; set; }
    [Parameter] public bool DisplaySearchBar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private LayoutService LayoutService { get; set; } = default!;    
    private bool _searchDialogOpen;    

    private void CloseAppMenu(string menu)
    {        
    }

    private void OpenSearchDialog() => _searchDialogOpen = true;
    private DialogOptions _dialogOptions = new() {Position = DialogPosition.TopCenter, NoHeader = true};
    
}