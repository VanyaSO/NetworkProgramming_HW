using System.Net;
using System.Net.Sockets;
using System.Text;

namespace hw_3_server;

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server();
        Task.Run(() => server.Start());
        Console.ReadLine();
    }
}

public class Server
{
    private TcpListener _listener;
    private Queue<Order> _queueOrders;
    private List<Dish> _dishes;

    public Server()
    {
        _listener = new TcpListener(IPAddress.Any, 3000);
        _queueOrders = new Queue<Order>();
        _dishes = new List<Dish>
        {
            new Dish { Name = "Spaghetti Bolognese", TimeToCook = TimeSpan.FromSeconds(30) },
            new Dish { Name = "Caesar Salad", TimeToCook = TimeSpan.FromSeconds(15) },
            new Dish { Name = "Grilled Chicken", TimeToCook = TimeSpan.FromSeconds(40) },
            new Dish { Name = "Vegetable Stir Fry", TimeToCook = TimeSpan.FromSeconds(20) },
            new Dish { Name = "Beef Stroganoff", TimeToCook = TimeSpan.FromSeconds(45) }
        };
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Сервер запущен...");

        while (true)
        {
            TcpClient client = _listener.AcceptTcpClient();
            Task.Run(() =>
            {
                NetworkStream networkStream = client.GetStream();

                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesReader = networkStream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesReader);
                    Console.WriteLine($"Получен запрос {data} от {client.Client.RemoteEndPoint}");

                    if (data.Trim().ToLower() == "status")
                    {
                        SendStatusOrder(networkStream);
                        Console.WriteLine("Запрос на получения статуса заказа");
                    }
                    else
                    {
                        string[] orderData = data.Split(',');
                        var order = new Order
                        {
                            OrderId = int.Parse(orderData[0]),
                            Dish = _dishes.FirstOrDefault(e => e.Id == int.Parse(orderData[0])),
                            Quantity = int.Parse(orderData[1]),
                            OrderTime = DateTime.Now
                        };

                        _queueOrders.Enqueue(order);
                        Console.WriteLine($"Заказ - {_queueOrders.Peek()}");

                        byte[] response = Encoding.UTF8.GetBytes("Заказ успешно принят");
                        networkStream.Write(response, 0, response.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    networkStream.Close();
                    client.Close();
                }
            });
        }
    }

    public void SendStatusOrder(NetworkStream networkStream)
    {
        string response;
        if (_queueOrders.Count > 0)
        {
            Order currentOrder = _queueOrders.Peek();
            if (CalcRemainingTime(currentOrder).TotalSeconds <= 0)
            {
                response = $"Заказ {currentOrder.OrderId} готов";
                _queueOrders.Dequeue();
            }
            else
            {
                response = $"Заказ {currentOrder.OrderId} будет готов через {CalcRemainingTime(currentOrder)}";
            }
        }
        else
        {
            response = "В очереди нету заказов";
        }

        byte[] data = Encoding.UTF8.GetBytes(response);
        networkStream.Write(data, 0, data.Length);
    }
    
    private TimeSpan CalcRemainingTime(Order restaurantOrder)
    {
        DateTime now = DateTime.Now;
        TimeSpan elapsedTime = now - restaurantOrder.OrderTime;
        TimeSpan totalCookingTime = TimeSpan.FromTicks(restaurantOrder.Quantity * restaurantOrder.Dish.TimeToCook.Ticks);
        TimeSpan remainingTime = totalCookingTime - elapsedTime;
        return remainingTime;
    }

}

public class Dish
{
    private static int _id { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan TimeToCook { get; set; }

    public Dish()
    {
        Id = ++_id;
    }
}

public class Order
{
    public int OrderId { get; set; }
    public Dish Dish { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderTime { get; set; }
}