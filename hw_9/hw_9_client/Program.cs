using System.Collections.Specialized;
using Microsoft.AspNetCore.SignalR.Client;

namespace hw_9_client;

class Program
{
    private static HubConnection _connection;
    static async Task Main(string[] args)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7038/rates")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Dictionary<string, double>, DateTime>("Receive", (exchangeRates, dateTimeLastUpdate) =>
        {
            Console.Clear();
            Console.WriteLine($"Last update: {dateTimeLastUpdate}");
            foreach (var pair in exchangeRates)
                Console.WriteLine($"{pair.Key}: {pair.Value}");
        });

        try
        {
            await _connection.StartAsync();
            Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            await _connection.StopAsync();
        }
    }
}