using e_CommerceContinental.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace e_CommerceContinental.Client.Shared.UserAuthorizedMenus;

public partial class DataEntry
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    private MudMenu? InventoryMenu = default!;    
    private MudMenu? SettingsMenu = default!;    
    private bool _searchDialogOpen;    

    private void Nav(string page, string menu)
    {        
        NavigationManager.NavigateTo(page);
        if (menu == "Inventory")
            InventoryMenu?.CloseMenu();
        if (menu == "settings")
            SettingsMenu?.CloseMenu();
    }
}