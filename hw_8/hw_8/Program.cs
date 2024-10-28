using hw_8;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

class Program
{
    static ITelegramBotClient botClient = new TelegramBotClient("7743941885:AAHP8Euek7A_Mpo3oDBOjQSXuyeOhln47v4");

    static async Task Main()
    {
        using var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        Console.ReadKey();
        cts.Cancel();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var message = update.Message;
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Начинаем квиз по С#");
                await SendQuestionAsync(message.Chat.Id, 0);
            }
        }
        else if (update.Type == UpdateType.CallbackQuery)
        {
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery?.Data != null)
            {
                var callbackData = callbackQuery.Data.Split('-');
                int questionIndex = int.Parse(callbackData[1]);
                int selectedAnswer = int.Parse(callbackData[2]);

                await CheckAnswerAsync(callbackQuery.Message.Chat.Id, questionIndex, selectedAnswer);
            }
        }
    }

    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
 
 
        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private static async Task SendQuestionAsync(long chatId, int questionIndex)
    {
        var question = Quiz.Questions[questionIndex];
        var answers = question.ListAnswers.Select((answer, index) => new[]
        {
            InlineKeyboardButton.WithCallbackData(answer, $"/answer-{questionIndex}-{index}")
        }).ToArray();

        var keyboard = new InlineKeyboardMarkup(answers);
        await botClient.SendTextMessageAsync(chatId, question.Title, replyMarkup: keyboard);
    }

    private static async Task CheckAnswerAsync(long chatId, int questionIndex, int selectedAnswerIndex)
    {
        var question = Quiz.Questions[questionIndex];
        if (selectedAnswerIndex == question.CorrectAnswerIndex)
        {
            await botClient.SendTextMessageAsync(chatId, $"Правильно");
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId, $"Неправильно {question.Explanation}");
        }

        if (questionIndex + 1 < Quiz.Questions.Count)
        {
            await SendQuestionAsync(chatId, questionIndex + 1);
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId, "Квиз завершен!");
        }
    }
}
