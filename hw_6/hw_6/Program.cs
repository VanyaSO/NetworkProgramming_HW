using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace hw_6;

class Program
{
    // Используя возможности https://api.random.org/, создайте оконное приложение для игры в кости. Реализуйте 2 режима игры: человек-человек, человек-компьютер. Значения для броска костей нужно генерировать с помощью API.
    // Добавьте к заданию визуальное отображение игровых костей. Также реализуйте анимацию броска кости.
    // Ключ получаем по этой ссылке: https://api.random.org/dashboard/details.
    private static HttpClient _httpClient = new HttpClient();
    private static string _apiKey = "ae4e11d0-5a74-4aa1-9f20-cc6fa79de8dc";
    private static string _url = "https://api.random.org/json-rpc/2/invoke";
    private static int _idRequest = 1;
    static async Task Main(string[] args)
    {
        // await PvP();
        await PvE();
    }

    static async Task<int> GetRandomNumber()
    {
        var jsonRequest = new
        {
            jsonrpc = "2.0",
            method = "generateIntegers",
            @params = new
            {
                apiKey = _apiKey,
                n = 1,
                min = 1,
                max = 6
            },
            id = _idRequest++
        };

        var response = await _httpClient.PostAsJsonAsync(_url, jsonRequest);
        var resultString = await response.Content.ReadAsStringAsync();
        dynamic resultObject = JsonConvert.DeserializeObject(resultString);

        return resultObject.result.random.data[0];
    }

    static void GetResultThrows(ref int countWinPl1, ref int countWinPl2, int numberPl1, int numberPl2)
    {
        if (numberPl1 > numberPl2)
        {
            countWinPl1 += 1;
        }
        else if (numberPl1 < numberPl2)
        {
            countWinPl2 += 1;
        }
        else
        {
            countWinPl1 += 1;
            countWinPl2 += 1;
        }
    }
    
    static async Task<int> Throw(string namePlayer)
    {
        Console.WriteLine($"Throws {namePlayer}, press Enter to throw.");
        Console.ReadKey();
        Console.WriteLine("The cube is spinning...");
        int result = await GetRandomNumber();
        Console.WriteLine($"{result} \n");

        return result;
    }
    
    static async Task PvP()
    {
        int countWinPlayer1 = 0, countWinPlayer2 = 0;
        int numberPlayer1 = 0, numberPlayer2 = 0;
        while (countWinPlayer1 != 3 && countWinPlayer2 != 3)
        {
            numberPlayer1 = await Throw("Alex");
            numberPlayer2 = await Throw("Sam");

            GetResultThrows(ref countWinPlayer1, ref countWinPlayer2, numberPlayer1, numberPlayer2);
            Console.WriteLine($"Count win player1: {countWinPlayer1}, Count win player2 {countWinPlayer2} \n");
        }
        string winner = countWinPlayer1 > countWinPlayer2 ? "Alex" : "Sam";

        Console.WriteLine($"Winner: {winner}");
    }

    
    static async Task PvE()
    {
        int countWinPlayer1 = 0, countWinPlayer2 = 0;
        int numberPlayer1 = 0, numberPlayer2 = 0;
        while (countWinPlayer1 != 3 && countWinPlayer2 != 3)
        {
            numberPlayer1 = await Throw("Alex");

            Console.WriteLine("The cube is spinning...");
            numberPlayer2 = await GetRandomNumber();
            Console.WriteLine($"{numberPlayer2} \n");

            GetResultThrows(ref countWinPlayer1, ref countWinPlayer2, numberPlayer1, numberPlayer2);
            Console.WriteLine($"Count win player1: {countWinPlayer1}, Count win player2 {countWinPlayer2} \n");
        }
        string winner = countWinPlayer1 > countWinPlayer2 ? "Alex" : "PC";

        Console.WriteLine($"Winner: {winner}");
    }

    
}


