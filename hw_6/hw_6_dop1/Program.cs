using System.Net;
using System.Net.Http.Json;

namespace hw_6_dop1;

class Program
{
    // Определите сервер, который будет принимать данный пользователя для регистрации в виде JSON. 
    // Определите клиента, который с помощью класса HttpClient будет отправлять 
    // POST-запрос на сервер для регистрации нового пользователя.
    private static HttpClient _client = new HttpClient();
    private static string uri = "http://localhost:8888/register/";

    static async Task Main(string[] args)
    {
        User newUser = new User() { Email = "ushachovg324@gmail.com", Password = "ushachovg324@gmail.com" };
        using var response = await _client.PostAsJsonAsync(uri, newUser);
    
        string message = await response.Content.ReadAsStringAsync();
        Console.WriteLine(message);
    }
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
