using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IFileService
    {
        Task LoadFile(FileModel file, params string[] pathFolderParts);
        Task LoadFile(Stream stream, params string[] pathParts);
        Task<FileModel> GetFile(string fileName,params string[] pathFolderParts);
        Task<FileModel> GetFile(params string[] pathParts);
    }
}
