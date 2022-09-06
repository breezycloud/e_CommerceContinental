using e_CommerceContinental.Client.Services;
using e_CommerceContinental.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace e_CommerceContinental.Client.Shared;

public partial class AppBarButtons
{
    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private LayoutService LayoutService { get; set; } = default!;
    private NotificationMessage[] _messages = Array.Empty<NotificationMessage>();
    private bool _newNotificationsAvailable = false;

    private async Task GetNotifications()
    {
        _newNotificationsAvailable = await NotificationService.AreNewNotificationsAvailable();
        _messages = await NotificationService.GetUnReadNotifications();
    }

    private async Task MarkNotificationAsRead()
    {
        await NotificationService.MarkNotificationsAsRead();
        _newNotificationsAvailable = false;
    }
    private async Task LogOut()
    {
        await _localStorage.RemoveItemAsync("token");
        await _localStorage.RemoveItemAsync("uid");
        await _localStorage.RemoveItemAsync("branch");
        await _localStorage.RemoveItemAsync("shop");
        nav.NavigateTo("/", true);
    }
}