﻿using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class GenreRepository: RemoveRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DataContext context) : base(context)
        {
        }
    }
}
