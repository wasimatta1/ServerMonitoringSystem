using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace ConumerSignalIR
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            var urlaleart = config.GetSection("SignalRConfig:SignalRUrl").Value!.ToString();

            var connection = new HubConnectionBuilder()
            .WithUrl(urlaleart)
            .Build();

            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub");

            connection.On<string>("ReceiveAlert", (message) =>
            {
                Console.WriteLine($"Received Alert: {message}");
            });
            Console.WriteLine("Listening for alerts. Press any key to exit...");
            Console.ReadKey();

            // Stop the connection when done
            await connection.StopAsync();
        }
    }
}
