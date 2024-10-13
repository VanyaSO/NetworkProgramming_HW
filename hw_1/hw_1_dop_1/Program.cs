using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;

namespace hw_1_dop1;

// Создайте сервер, который принимает запрос от клиента, вычисляет квадратный корень числа, отправленного клиентом, и возвращает результат.

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
            Console.WriteLine("Введите число:");
            int number;
            if (!Int32.TryParse(Console.ReadLine(), out number)) new ArgumentException("Это не число");
            
            TcpClient client = new TcpClient("127.0.0.1", 8888);
            NetworkStream stream = client.GetStream();
            
            byte[] sendData = Encoding.ASCII.GetBytes(number.ToString());
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