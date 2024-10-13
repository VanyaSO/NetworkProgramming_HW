using System.Net;
using System.Net.Sockets;
using System.Text;

namespace hw_1_server;

class Program
{
    static void Main(string[] args)
    {
        StartServer();
    }
    
    public static void StartServer()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8888;
        TcpListener listener = new TcpListener(ipAddress, port);
    
        try
        {
            listener.Start();
            Console.WriteLine("Сервер запущен...");
    
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                
                byte[] getData = new byte[1024];
                StringBuilder responseData = new StringBuilder();
                int bytes;
                do
                {
                    bytes = stream.Read(getData, 0, getData.Length);
                    responseData.Append(Encoding.ASCII.GetString(getData, 0, bytes));
                }
                while (stream.DataAvailable);

                double squareRoot = Math.Pow(Int32.Parse(responseData.ToString()), 2);
                
                byte[] sendData = Encoding.ASCII.GetBytes(squareRoot.ToString());
    
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine($"Квадратный корень числа {responseData} - {squareRoot} отправен клиенту");
                client.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        finally
        {
            listener.Stop();
        }
    }
}