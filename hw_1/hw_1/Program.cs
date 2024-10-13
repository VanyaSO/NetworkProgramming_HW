using System.Net.Sockets;
using System.Text;

namespace hw_1;

// Разработайте набор консольных приложений. 

// Первое приложение: серверное приложение, которое на запросы клиента возвращает текущее время или дату на сервере. 
// Второе приложение: клиентское приложение, запрашивающее дату или время. 

// Пользователь с клавиатуры определяет, что нужно запросить. После отсылки даты или времени сервер разрывает соединение.
// Клиентское приложение отображает полученные данные.

class Program
{
    static void Main(string[] args)
    {
        ConnectToServer();
    }

    static void ConnectToServer()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 8888);

            NetworkStream stream = client.GetStream();

            Console.WriteLine("Enter \"date\" or \"time\":");
            string clientAction = Console.ReadLine();
            byte[] sendData = Encoding.ASCII.GetBytes(clientAction);
            stream.Write(sendData, 0, sendData.Length);
            
            

            byte[] getData = new byte[256];
            StringBuilder responseData = new StringBuilder();
            int bytes;
            do
            {
                bytes = stream.Read(getData, 0, getData.Length);
                responseData.Append(Encoding.UTF8.GetString(getData, 0, bytes));
            } while (bytes > 0);

            Console.WriteLine(responseData);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}