using InventorySystem.Models;

namespace InventorySystem.Utils;

public interface INotifier
{
    public void SendNotification(string recipient, string message);
    public void SendAlarm(string recipient);
}