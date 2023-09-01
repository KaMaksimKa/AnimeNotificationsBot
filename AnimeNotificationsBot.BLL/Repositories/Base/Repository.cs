using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.Common.Interfaces;
using AnimeNotificationsBot.DAL;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DataContext Context;
        protected DbSet<T> Entity => Context.Set<T>();

        public Repository(DataContext context)
        {
            Context = context;
        }

        public async Task<List<T>> GetRangeWhereAsync(Func<IQueryable<T>, Task<List<T>>> selector)
        {
            return await selector(Entity);
        }

        public async Task<T?> GetFirstOrDefaultAsync(Func<IQueryable<T>, Task<T?>> selector)
        {
            return await selector(Entity);
        }

        public async Task<T> GetById(int id)
        {
            var entity = await Entity.FindAsync(id);

            if (entity == null)
                throw CreateNotFoundEntityException(id);

            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            return await Entity.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            return (await Entity.AddAsync(entity)).Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await Entity.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            Entity.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            Entity.UpdateRange(entities);
        }

        public void Remove(T entity)
        {
            Entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Entity.RemoveRange(entities);
        }

        public async Task RemoveById(int id)
        {
            var entity = await GetById(id);
            Entity.Remove(entity);
        }

        protected Exception CreateNotFoundEntityException(long id)
        {
            return new NotFoundEntityException()
            {
                EntityName = typeof(T).Name,
                EntityId = id
            };
        }

    }
}
