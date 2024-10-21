using System.Net;
using System.Text;
using System.Text.Json;

public class Program
{
    private static int id = 1;
    public static List<User> _users = new List<User>();

    static async Task Main()
    {
        HttpListener server = new HttpListener();
        server.Prefixes.Add("http://localhost:8888/register/");
        server.Start();
        
        Console.WriteLine("Сервер запущен...");

        var context = await server.GetContextAsync();
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod == "POST")
        {
            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                string contentUser = await reader.ReadToEndAsync();

                try
                {
                    var user = JsonSerializer.Deserialize<User>(contentUser);
                    if (user is not null)
                    {
                        user.Id = id++;
                        _users.Add(user);
                        await SendResponse(response, $"Пользователь {user.Email} успешно зарегистрирован");
                    }
                }
                catch (Exception e)
                {
                    await SendResponse(response, "Произошла ошибка, попробуйте еще раз");
                    Console.WriteLine(e);
                }
            }
        }
        else
        {
            await SendResponse(response, "Действие не доступно");
        }
        
        response.Close();
    }


    static async Task SendResponse(HttpListenerResponse response, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;

        using Stream output = response.OutputStream;
        await output.WriteAsync(buffer);
        await output.FlushAsync();
    }
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
    