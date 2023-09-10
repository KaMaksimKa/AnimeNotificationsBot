using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Notifications;
using AnimeNotificationsBot.BLL.Interfaces;
using Quartz;

namespace AnimeNotificationsBot.Api.Quartz.Jobs
{
    public class NotifyAboutAnimeJob : IJob
    {
        private readonly IAnimeNotificationService _notificationService;
        private readonly IBotSender _botSender;

        public NotifyAboutAnimeJob(IAnimeNotificationService notificationService, IBotSender botSender)
        {
            _notificationService = notificationService;
            _botSender = botSender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var notifications = await _notificationService.GetNewNotificationAsync();

            var tasks = new List<Task>();

            foreach (var notification in notifications)
            {
                foreach (var chatId in notification.ChatIds)
                {
                    tasks.Add(_botSender.SendMessageAsync(new NotifyAboutAnimeMessage(notification.AnimeNotificationModel), chatId));
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}
