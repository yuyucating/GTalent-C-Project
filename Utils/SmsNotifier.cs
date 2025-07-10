using InventorySystem.Models;

namespace InventorySystem.Utils;

public class SmsNotifier : INotifier
{
    // public void INotifier.SendNotification(string recipient, string message)
    public void SendNotification(string recipient, string message)
    {
        Console.WriteLine($"Send SMS to {recipient}: {message}");
    }

    public void SendAlarm(string recipient)
    {
        Console.WriteLine($"Send SMS to {recipient}");
    }

}