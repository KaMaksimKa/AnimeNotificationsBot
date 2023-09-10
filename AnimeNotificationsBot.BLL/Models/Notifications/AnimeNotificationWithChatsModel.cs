using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeNotificationsBot.BLL.Models.Notifications
{
    public class AnimeNotificationWithChatsModel
    {
        public required AnimeNotificationModel AnimeNotificationModel { get; set; }
        public List<long> ChatIds { get; set; } = new List<long>();
    }
}
