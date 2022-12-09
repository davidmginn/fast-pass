﻿using FastPass.UI.ViewModels;

namespace FastPass.UI.Services;

public class NotificationService
{
    // todo: move to configuration along with period to update notification window and due time potentially
    private readonly int MINUTES_TO_LIVE = 15;

    public NotificationService()
    {
        _timer = new Timer(RemoveExpiredNotifications, null, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(3));
    }

    private readonly Timer _timer;
    public readonly List<Notification> Notifications = new();

    public void AddNotification(Notification notitication)
    {
        Console.WriteLine("Pushing notification");
        Notifications.Add(notitication);
        Console.WriteLine($"Now at {Notifications.Count} notifications");

        NotifyStateChanged();
    }

    public void RemoveNotification(Notification notitication)
    {
        Console.WriteLine("Popping notification");
        Notifications.Remove(notitication);
        Console.WriteLine($"Now at {Notifications.Count} notifications");

        NotifyStateChanged();
    }

    private void RemoveExpiredNotifications(object? stateInfo)
    {
        for (int i = Notifications.Count - 1; i >= 0; i--)
        {
            var difference = DateTime.Now - Notifications[i].CreatedAt;

            if (difference >= TimeSpan.FromMinutes(MINUTES_TO_LIVE))
                Notifications.RemoveAt(i);
        }

        NotifyStateChanged();
    }

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}

