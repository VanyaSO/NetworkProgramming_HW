using System.Net;
using System.Net.Sockets;
using System.Text;

namespace hw_4;

class Program
{
    static void Main(string[] args)
    {
        UdpClient client = new UdpClient();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3000);

        Console.WriteLine("Введите имя товара чтобы узнать цену или (выйти) для выхода");

        while (true)
        {
            Console.WriteLine("Введите название товара:");
            string request = Console.ReadLine();
            if (request.ToLower() == "выйти") break;

            byte[] data = Encoding.UTF8.GetBytes(request);
            client.Send(data, data.Length, endPoint);


            IPEndPoint remoteEndPoint = null;
            byte[] responseData = client.Receive(ref remoteEndPoint);

            string response = Encoding.UTF8.GetString(responseData);
            Console.WriteLine(response);
        }
        
        client.Close();
    }
}