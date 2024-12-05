using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add SignalR service to the DI container
builder.Services.AddSignalR();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var urlaleart = config.GetSection("SignalRConfig:SignalRUrl").Value!.ToString();

// Configure app URLs to listen on (HTTP and HTTPS)
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

var app = builder.Build();

// Map the SignalR hub to the /alertHub endpoint
app.MapHub<AlertHub>("/alertHub");

// Start the application
app.Run();

public class AlertHub : Hub
{
    // Method to send alerts from any client, broadcasting to all clients
    public async Task SendAlert(string message)
    {
        // Broadcast the message to all connected clients
        await Clients.All.SendAsync("ReceiveAlert", message);
    }
}
