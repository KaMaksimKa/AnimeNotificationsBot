﻿using AnimeNotificationsBot.BLL.Models.Subscription;

namespace AnimeNotificationsBot.Api.Models
{
    public class SubcribeAnimeExtModel
    {
        public SubscribeAnimeModel SubscribeAnimeModel { get; set; } = null!;
        public bool Unsubscribe { get; set; }
    }
}
