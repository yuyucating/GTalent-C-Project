using InventorySystem.Models;
using InventorySystem.Utils;

namespace InventorySystem.Services;

public class NotificationService
{
    private readonly INotifier _notifier;

    // Constructor
    public NotificationService(INotifier notifier)
    {
        _notifier = notifier;
    }

    public void NotifyUser(string recipient, string message)
    {
        Console.WriteLine($"Ready to notify user {recipient}");
        _notifier.SendNotification(recipient, message);
        Console.WriteLine($"Notify completed.");
    }
}