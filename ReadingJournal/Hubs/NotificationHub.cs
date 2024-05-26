using Microsoft.AspNetCore.SignalR;

namespace ReadingJournal.Hubs
{
    public class NotificationHub : Hub
    {        
        public async Task SendNotification(List<string> users, string sender, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", users, sender, message);                     
        }   

    }
}
