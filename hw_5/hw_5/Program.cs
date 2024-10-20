using System.Net;

namespace hw_5;

class Program
{
    private static HttpClient _httpClient = new HttpClient();
    // Скачайте любое изображение из интернета и сохраните в локальной папке на компьютере.
    static async Task Main(string[] args)
    {
        string urlImage = "https://images.prom.ua/1734007967_w600_h600_1734007967.jpg";
    
        WebClient webClient = new WebClient();
        using (Stream stream = webClient.OpenRead(urlImage))
        {
            using (FileStream fileStream = File.Create("/Users/ivanushachov/Desktop/image.jpg"))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
    
    
    // dop 1
    // Считайте «Заголовки» и «Статус код», отправив запрос на любой сайт.
    static async Task Dop1()
    {
        using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://od.itstep.org");
        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        Console.WriteLine($"Code response - {responseMessage.StatusCode}\n");

        Console.WriteLine("---Headers---");
        foreach (var header in responseMessage.Headers)
        {
            Console.WriteLine(header.Key);
            foreach (var val in header.Value)
            {
                Console.WriteLine(val);
            }
        }
    }

    // dop 2
    // Создайте приложение, читающее Html страницу и отображающую ее в текстовом редакторе в виде текста.
    // Пользователь ввод Url адрес запрашиваемой страницы.
    static async Task Dop2()
    {
        string userUrl = "https://od.itstep.org";
        using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, userUrl);
        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        if (responseMessage.StatusCode != HttpStatusCode.OK)
            Console.WriteLine("Не удалось получить ответ от указаного url");

        string contentPage = await responseMessage.Content.ReadAsStringAsync();

        string fileName = userUrl.Replace("https://", "") + ".txt";
        if (File.Exists(fileName))
            File.Delete(fileName);
        
        using (StreamWriter streamWriter = new StreamWriter(fileName))
        {
            streamWriter.WriteLine(contentPage);
        }
    }
}
