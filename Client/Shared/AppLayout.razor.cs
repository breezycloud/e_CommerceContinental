using e_CommerceContinental.Client.AppTheme;
using e_CommerceContinental.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace e_CommerceContinental.Client.Shared;

public partial class AppLayout : LayoutComponentBase
{
    [Inject] private LayoutService LayoutService { get; set; } = default!;    
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    
    private bool _drawerOpen = false;
    private bool _topMenuOpen = false;
    protected override void OnInitialized()
    {
        LayoutService.SetBaseTheme(Theme.LandingPageTheme());        
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        //refresh nav menu because no parameters change in nav menu but internal data does
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OpenTopMenu()
    {
        _topMenuOpen = true;
    }

    private void OnDrawerOpenChanged(bool value)
    {
        _topMenuOpen = false;
        _drawerOpen = value;
        StateHasChanged();
    }    
}