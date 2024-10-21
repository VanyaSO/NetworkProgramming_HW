using System.Net.Sockets;
using System.Text;

namespace hw_3;

class Program
{
    // Вам необходимо разработать распределенную систему, которая позволит ресторанам принимать заказы через сеть, а затем обрабатывать их на центральном сервере.
    // Создайте 2 приложения.
    // Сервер:
    // 1) Используйте TcpListener для прослушивания подключений от ресторанов.
    // 2) Сервер должен принимать данные о заказах от подключенных ресторанов.
    // 3) Реализуйте логику обработки заказов (например, добавление заказов в очередь и управление временем выполнения).
    // Клиент:
    // 1) Используйте TcpClient для подключения к серверу.
    // 2) Ресторан должен иметь возможность отправлять информацию о заказах на сервер.
    // 3) Предоставьте возможность для ресторана проверить состояние своих заказов (например, узнать текущее положение в очереди и оставшееся время до готовности).
    // Дополнительно:
    // 1) Система должна поддерживать одновременное обслуживание нескольких ресторанов.
    // 2) Реализуйте механизмы обработки заказов (например, уведомление ресторана о готовности заказа и его выдача).
    // 3) Добавьте возможность отмены заказа или его изменения.

    static async Task Main()
    {
        Client client = new Client();
        client.SendOrder(orderId: 1, quantity: 1);
        client.SendOrder(orderId: 2, quantity: 4);

        client.CheckOrderStatus();

        client.SendOrder(orderId: 3, quantity: 3);

        await Task.Delay(3000);

        while (true)
        {
            client.CheckOrderStatus();
            Console.ReadLine();
        }
    }

    
    public class Client
    {
        private string _serverIp = "127.0.0.1";
        private int _serverPort = 3000;
        public void SendOrder(int orderId, int quantity)
        {
            using (TcpClient client = new TcpClient(_serverIp, _serverPort))
            {
                NetworkStream stream = client.GetStream();

                try
                {
                    string orderData = $"{orderId},{quantity}";
                    byte[] data = Encoding.UTF8.GetBytes(orderData);
                    stream.Write(data, 0, data.Length);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine($"Ответ сервера: {response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    stream.Close();
                }
            }
        }
        public void CheckOrderStatus()
        {
            using (TcpClient client = new TcpClient(_serverIp, _serverPort))
            {
                NetworkStream stream = client.GetStream();

                try
                {
                    byte[] statusRequest = Encoding.UTF8.GetBytes("status");
                    stream.Write(statusRequest, 0, statusRequest.Length);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine($"Ответ сервера: {response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    stream.Close();
                }
            }
        }

    }

}

