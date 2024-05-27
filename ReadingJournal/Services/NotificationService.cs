using BusinessLayer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer;
using System;
using System.Security.Claims;

public class Notification
{
    public string User { get; set; }

    public string Sender { get; set; }

    public string Message { get; set; }        

    public Notification(string user, string sender, string message)
    {
        User = user;
        Sender = sender;
        Message = message;
    }
}
public class NotificationService
{
   
    private HubConnection _hubConnection;
    private ILogger<NotificationService> _logger;
    public Dictionary<string, List<Notification>> Notifications;
    private bool _isHandlerAttached = false;

    public event Action<string, string, string> OnNotificationReceived;

    public NotificationService(ILogger<NotificationService> logger)
    {
        Notifications = new();
        _logger = logger;
    }

    public async Task StartConnectionAsync()
    {     
        if (_hubConnection == null)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7257/notificationHub")
                .Build();
        }

        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting SignalR connection: {Message}", ex.Message);
            }
        }

        if (!_isHandlerAttached)
        {
            _hubConnection.On<List<string>, string, string>("ReceiveNotification", (users, sender, message) =>
            {
                foreach (string user in users)
                {
                    AddNotification(user, sender, message);
                    OnNotificationReceived?.Invoke(user, sender, message);
                }
            });

            _isHandlerAttached = true;
        }
    }
    void AddNotification(string user, string sender, string message)
    {
        if (!Notifications.ContainsKey(user))
        {
                Notifications.Add(user, new List<Notification>());
        }
        Notifications[user].Add(new Notification(user, sender, message));
    }

    public List<Notification> GetNotifications(string user)
    {
        if (Notifications.ContainsKey(user))
        {
            List<Notification> result = Notifications[user];
            Notifications[user] = new List<Notification>();
            return result;
        }
        else
        {
            return new List<Notification>();
        }
    }

    public async Task SendNotificationAsync(List<string> users, string sender, string message)
    {
        await StartConnectionAsync();

        if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await _hubConnection.SendAsync("SendNotification", users, sender, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification: {Message}", ex.Message);
            }
        }
        else
        {
            _logger.LogWarning("Cannot send notification because SignalR connection is not started.");
        }
    }
}
