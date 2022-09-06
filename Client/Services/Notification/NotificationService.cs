using e_CommerceContinental.Shared.Models;
using MudBlazor;
using System.Collections.Generic;


namespace e_CommerceContinental.Client.Services;

class NotificationService : INotificationService
{    
    public Task AddNotification(NotificationMessage message)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AreNewNotificationsAvailable()
    {
        return Task.FromResult(true);
    }

    public Task<NotificationMessage> GetMessageById(Guid id)
    {
        return Task.FromResult(new NotificationMessage());
    }

    public Task<NotificationMessage[]> GetNotifications()
    {
        return Task.FromResult(Array.Empty<NotificationMessage>());
    }

    public Task<NotificationMessage[]> GetUnReadNotifications()
    {
        return Task.FromResult(Array.Empty<NotificationMessage>());
    }

    public Task MarkNotificationsAsRead()
    {
        throw new NotImplementedException();
    }

    public Task MarkNotificationsAsRead(Guid id)
    {
        throw new NotImplementedException();
    }

    public void NotifyError(string message)
    {
        throw new NotImplementedException();
    }

    public void NotifySuccess(string message)
    {
        throw new NotImplementedException();
    }
}