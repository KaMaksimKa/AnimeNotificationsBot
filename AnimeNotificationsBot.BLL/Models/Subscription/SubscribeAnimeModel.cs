using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeNotificationsBot.BLL.Models.Subscription
{
    public class SubscribeAnimeModel
    {
        /// <summary>
        /// Если null, то подразумеваются все аниме
        /// </summary>
        public long? AnimeId { get; set; }

        /// <summary>
        /// Если null, то подразумеваются все озвучки
        /// </summary>
        public long? DubbingId { get; set; }
    }
}
