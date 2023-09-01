using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IFileService
    {
        Task LoadFile(FileModel file, params string[] pathParts);
        Task<FileModel> GetFile(string fileName,params string[] pathParts);
    }
}
