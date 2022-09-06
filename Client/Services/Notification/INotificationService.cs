using e_CommerceContinental.Shared.Models;

public interface INotificationService
{
    Task<bool> AreNewNotificationsAvailable();
    Task MarkNotificationsAsRead();
    Task MarkNotificationsAsRead(Guid id);
    Task<NotificationMessage> GetMessageById(Guid id);
    Task<NotificationMessage[]> GetNotifications();
    Task<NotificationMessage[]> GetUnReadNotifications();
    Task AddNotification(NotificationMessage message);
    void NotifySuccess(string message);
    void NotifyError(string message);
}