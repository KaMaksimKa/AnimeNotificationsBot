using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IImageService
    {
        Task<FileInfoModel> LoadImage(Stream stream);

        Task<FileModel> GetImage(params string[] pathParts);
    }
}
