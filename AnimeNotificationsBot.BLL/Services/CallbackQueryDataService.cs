using AnimeNotificationsBot.BLL.Interfaces;
using System.Text.Json;
using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Services
{
    public class CallbackQueryDataService: ICallbackQueryDataService
    {
        private readonly DataContext _context;

        public CallbackQueryDataService(DataContext context)
        {
            _context = context;
        }

        public async Task<long> AddAsync<T>(T data)
        {
            var stringData = JsonSerializer.Serialize(data);
            var result = (await _context.CallbackQueryData.AddAsync(new CallbackQueryData()
            {
                Data = stringData
            })).Entity;

            await _context.SaveChangesAsync();

            return result.Id;
        }

        public async Task<T> GetAsync<T>(long dataId)
        {
            var data = await _context.CallbackQueryData.FindAsync(dataId);

            if (data == null)
            {
                throw new NotFoundEntityException()
                {
                    EntityName = nameof(CallbackQueryData),
                    PropertyName = nameof(data.Id),
                    PropertyValue = dataId
                };
            }

            var result = JsonSerializer.Deserialize<T>(data.Data);

            if (result == null)
            {
                throw new ArgumentException(typeof(T).Name);
            }

            return result;
        }
    }
}
