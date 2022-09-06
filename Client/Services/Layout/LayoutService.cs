using MudBlazor;

namespace e_CommerceContinental.Client.Services;
public class LayoutService
{
    private readonly IUserPreferencesService _userPreferencesService = default!;
    private UserPreferences _userPreferences = default!;

    public bool IsRTL { get; private  set; } = false;
    public bool IsDarkMode { get; private set; } = false;
    public MudTheme CurrentTheme { get; private set; } = default!;

    
    public LayoutService(IUserPreferencesService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }
    
    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _userPreferences = await _userPreferencesService.LoadUserPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.DarkTheme;
            IsRTL = _userPreferences.RightToLeft;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences { DarkTheme = IsDarkMode} ;
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
    }
    
    public event EventHandler? MajorUpdateOccured;

    public void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this,EventArgs.Empty);
    
    public async Task ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        _userPreferences.DarkTheme = IsDarkMode;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccured();
    }

    public async Task ToggleRightToLeft()
    {
        IsRTL = !IsRTL;
        _userPreferences.RightToLeft = IsRTL;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccured();

    }

    public void SetBaseTheme(MudTheme theme)
    {
        CurrentTheme = theme;
        OnMajorUpdateOccured();
    }    
}