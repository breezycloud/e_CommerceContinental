using e_CommerceContinental.Client.AppTheme;
using e_CommerceContinental.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Blazored.LocalStorage;

namespace e_CommerceContinental.Client.Shared;


public partial class MainLayout : LayoutComponentBase, IDisposable
{ 
    [Inject] private  LayoutService LayoutService { get; set; } = default!;
    [Inject] private BackgroundTask _BackgroundTask { get; set; } = default!;
    private MudThemeProvider? _mudThemeProvider ;
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    protected override void OnInitialized()
    {
        // _BackgroundTask.StartExecuting();
        _localStorage.SetItemAsync("mode", true);
        LayoutService.SetBaseTheme(Theme.DocsTheme());
        LayoutService.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccured!;
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (firstRender)
        {
            await ApplyUserPreferences();
            StateHasChanged();
        }
    }

    private async Task ApplyUserPreferences()
    {
        var defaultDarkMode = await _mudThemeProvider!.GetSystemPreference();
        await LayoutService.ApplyUserPreferences(defaultDarkMode);
    }

    public void Dispose()
    {
        LayoutService.MajorUpdateOccured -= LayoutServiceOnMajorUpdateOccured!;
    }

    private void LayoutServiceOnMajorUpdateOccured(object sender, EventArgs e) => StateHasChanged();
}