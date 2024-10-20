using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace hw_4_server;

class Program
{
    private static UdpClient _udpClient;
    private static Dictionary<string, string> _products = new Dictionary<string, string>()
    {
        { "CPU i5", "$150" },
        { "CPU Ryzen 5", "$170" },
        { "GPU 3060", "$350" },
        { "RAM 16GB", "$60" },
        { "SSD 1TB", "$80" },
        { "HDD 2TB", "$40" },
        { "Motherboard B550", "$90" },
        { "PSU 650W", "$50" },
        { "Case", "$30" },
        { "Cooler", "$15" }
    };

    private static Dictionary<IPEndPoint, (int requests, DateTime lastRequests)> _clientRequests = new();
    private static int _maxRequestPerHour = 10;
    private static TimeSpan _inactiveTimeout = TimeSpan.FromMinutes(10);
    private static Timer _cleanupTimer;

    static void Main(string[] args)
    {
        _udpClient = new UdpClient(3000);
        Console.WriteLine("start");

        _cleanupTimer = new Timer(60000);
        _cleanupTimer.Elapsed += CleanupInactiveClients;
        _cleanupTimer.Start();

        ServerStart();
    }

    private static void ServerStart()
    {
        while (true)
        {
            IPEndPoint client = null;
            byte[] data = _udpClient.Receive(ref client);
            string request = Encoding.UTF8.GetString(data);
            Console.WriteLine($"Новый запрос от {client}: {request}");

            string response;
            if (!IsClientAllowed(client))
                response = "Вы привысили лимит запросов, попробуйте позже";
            else
                response = GetPrice(request);

            SendResponse(client, response);
        }
    }

    private static bool IsClientAllowed(IPEndPoint client)
    {
        if (!_clientRequests.ContainsKey(client))
            _clientRequests[client] = (0, DateTime.Now);

        var (count, lastRequest) = _clientRequests[client];
        
        if ((DateTime.Now - lastRequest).TotalHours >= 1) count = 0;
        if (count >= _maxRequestPerHour) return false;

        _clientRequests[client] = (count += 1, DateTime.Now);
        return true;
    }

    private static void SendResponse(IPEndPoint client, string response)
    {
        byte[] data = Encoding.UTF8.GetBytes(response);
        _udpClient.Send(data, data.Length, client);
        Console.WriteLine($"Клиенту {client} отправлен овтет: {response}");
    }

    private static string GetPrice(string product)
    {
        if (_products.ContainsKey(product))
            return _products[product];
        return "Product not found";
    }

    private static void CleanupInactiveClients(object sender, ElapsedEventArgs eda)
    {
        var inactiveClients = new List<IPEndPoint>();

        foreach (var client in _clientRequests)
            if ((DateTime.Now - client.Value.lastRequests) > _inactiveTimeout)
                inactiveClients.Add(client.Key);

        foreach (var client in inactiveClients)
        {
            _clientRequests.Remove(client);
            Console.WriteLine($"Клиент {client} был отключен принудительно");
        }
    }
}