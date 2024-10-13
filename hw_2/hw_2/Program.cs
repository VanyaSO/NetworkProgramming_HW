using System.Net.Sockets;
using System.Text;

namespace hw_2;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 8080);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Введите пару валют (EUR/USD) или 0 для выхода:");
            string pair;
            while (true)
            {
                pair = Console.ReadLine();
                
                if (pair == "0") break;

                byte[] request = Encoding.UTF8.GetBytes(pair);
                stream.Write(request, 0, request.Length);
                
                byte[] buffer = new byte[512];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytes);

                Console.WriteLine(response);
                Console.WriteLine();
            }
            
            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}