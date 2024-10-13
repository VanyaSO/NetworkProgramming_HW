using System.ComponentModel.Design.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace hw_2_server;

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server();
        server.StartServer();
    }
}

class Server
{
    private TcpListener _listener;
    private bool _isWork;
    private static readonly Dictionary<string, double> _exchangeCurrencies = new Dictionary<string, double>()
    {
        { "USD/CAD", 1.34 },
        { "CAD/USD", 0.75 },
        { "USD/AUD", 1.56 },
        { "AUD/USD", 0.64 },
        { "USD/JPY", 149.80 },
        { "JPY/USD", 0.0067 },
        { "USD/CNY", 7.30 },
        { "CNY/USD", 0.14 },
        { "USD/CHF", 0.92 },
        { "CHF/USD", 1.09 },
        { "USD/INR", 83.50 },
        { "INR/USD", 0.012 },
        { "USD/BRL", 5.05 },
        { "BRL/USD", 0.20 },
        { "EUR/GBP", 0.87 },
        { "GBP/EUR", 1.15 }
    };

    public Server()
    {
        _listener = new TcpListener(IPAddress.Any, 8080);
        _isWork = true;
    }

    public void StartServer()
    {
        _listener.Start();

        while (_isWork)
        {
            TcpClient client = _listener.AcceptTcpClient();
            Logger.Write($"Клиент {client.Client.RemoteEndPoint} покдлючился");

            Thread thread = new Thread(ListenClient);
            thread.Start(client);
        }
    }

    private void ListenClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        
        byte[] buffer = new byte[512];
        int bytes;

        while ((bytes = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string request = Encoding.ASCII.GetString(buffer, 0, bytes);

            string[] currencies = request.Split('/');
            if (currencies.Length == 2)
            {
                string currentPair = $"{currencies[0]}/{currencies[1]}";
                if (_exchangeCurrencies.ContainsKey(currentPair))
                {
                    double rate = _exchangeCurrencies[currentPair];
                    string response = $"{currentPair}: {rate.ToString()}";
                    SendResponse(stream, response);
                    Logger.Write($"Ответ сервера на запрос пары {response}");
                }
                else
                {
                    SendResponse(stream, "Такой курс не найден");
                    Logger.Write($"Пара {currentPair} не найдена");
                }
            }
            else
            {
                SendResponse(stream,"Сформулируйте корректно запрос");
                Logger.Write($"Клиент {client.Client.AddressFamily} некорректно сформулировал запрос");
            }
        }
        
        client.Close();
        Logger.Write($"Клиент {client.Client.AddressFamily} отключился");
    }

    private void SendResponse(NetworkStream stream, string response)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(response);
        stream.Write(bytes, 0, bytes.Length);
    }

}