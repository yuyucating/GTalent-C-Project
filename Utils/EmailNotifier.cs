namespace InventorySystem.Utils;

public class EmailNotifier : INotifier
{
    // public void INotifier.SendNotification(string recipient, string message)
    public void SendNotification(string recipient, string message)
    {
        Console.WriteLine($"Send email to {recipient}: {message}");
    }
    public void SendAlarm(string recipient)
    {
        Console.WriteLine($"Send email to {recipient}");
    }
}