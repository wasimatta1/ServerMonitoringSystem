using Microsoft.AspNetCore.SignalR;

namespace SignalR
{
    public class AlertHub : Hub
    {
        public async Task SendAlert(string message)
        {
            await Clients.All.SendAsync("ReceiveAlert", message);
        }
    }
}
