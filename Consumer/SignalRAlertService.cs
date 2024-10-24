using Microsoft.AspNetCore.SignalR.Client;

namespace Consumer
{
    public class SignalRAlertService
    {
        private readonly HubConnection _connection;

        public SignalRAlertService(string signalrUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(signalrUrl)
                .Build();
            _connection.StartAsync().Wait();
        }

        public async void SendAlert(string message)
        {
            await _connection.InvokeAsync("SendAlert", message);

            Console.WriteLine("Sent alert via SignalR: " + message);
        }
    }

}
