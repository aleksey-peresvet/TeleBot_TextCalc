using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Newtonsoft.Json;

namespace TelegramBot_Arithmetic_Calculator
{
    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("Здесь должен быть токен моего бота :)");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                Calculator_OPZ opz = new Calculator_OPZ();
                string answer = 
                   opz.Calculate_poliz(ref opz.Parser_poliz(message.Text));
                if (answer == "Error01")
                {
                    answer = "Мой функционал ограничен решением арифметических выражений.";
                }
                await botClient.SendTextMessageAsync(message.Chat, answer);
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(exception));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {  }
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}