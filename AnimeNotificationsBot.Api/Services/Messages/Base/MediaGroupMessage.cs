﻿using AnimeNotificationsBot.Api.Models;

namespace AnimeNotificationsBot.Api.Services.Messages.Base
{
    public class MediaGroupMessage
    {
        public List<TelegramPhotoModel> Images { get; set; } = new List<TelegramPhotoModel>();
    }
}
