using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeWithImageModel: AnimeModel
    {
        public FileModel? Image { get; set; }
    }
}
