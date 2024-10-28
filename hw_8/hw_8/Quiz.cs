namespace hw_8;

public static class Quiz
{
    public static List<Question> Questions = new List<Question>
    {
        new Question
        {
            Title = "Что такое `int` в C#?",
            ListAnswers = new List<string> { "Тип данных", "Метод", "Класс", "Событие" },
            CorrectAnswerIndex = 0,
            Explanation = "`int` — это встроенный тип данных, представляющий целое число."
        },
        new Question
        {
            Title = "Какой метод используется для вывода текста в консоль?",
            ListAnswers = new List<string> { "Console.WriteLine()", "Console.ReadLine()", "Console.Output()", "Console.Print()" },
            CorrectAnswerIndex = 0,
            Explanation = "Метод `Console.WriteLine()` используется для вывода текста в консоль."
        },
        new Question
        {
            Title = "Что возвращает метод `Console.ReadLine()`?",
            ListAnswers = new List<string> { "int", "string", "void", "bool" },
            CorrectAnswerIndex = 1,
            Explanation = "`Console.ReadLine()` возвращает значение типа `string`."
        },
        new Question
        {
            Title = "Какое ключевое слово используется для создания объекта в C#?",
            ListAnswers = new List<string> { "this", "new", "static", "object" },
            CorrectAnswerIndex = 1,
            Explanation = "Ключевое слово `new` используется для создания экземпляра объекта в C#."
        },
        new Question
        {
            Title = "Что произойдет, если попытаться делить на ноль в C#?",
            ListAnswers = new List<string> { "Возникнет ошибка времени компиляции", "Возникнет ошибка времени выполнения", "Вернется значение 0", "Программа завершится успешно" },
            CorrectAnswerIndex = 1,
            Explanation = "При попытке деления на ноль возникнет ошибка времени выполнения `DivideByZeroException`."
        }
    };
}
