using AnimeNotificationsBot.Api.Configs;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AnimeNotificationsBot.Api.Services
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BotConfiguration _botConfig;

        public ConfigureWebhook(
            IServiceProvider serviceProvider,
            IOptions<BotConfiguration> botOptions)
        {
            _serviceProvider = serviceProvider;
            _botConfig = botOptions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAddress = $"{_botConfig.HostAddress}/{_botConfig.Route}/{_botConfig.BotToken}";
         
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
