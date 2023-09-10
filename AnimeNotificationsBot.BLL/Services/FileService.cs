using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Services
{
    public class FileService : IFileService
    {
        public async Task LoadFile(FileModel file, params string[] pathFolderParts)
        {
            var path = GetPath(file.FileName,pathFolderParts);
            await LoadFile(file.Content, path);
        }

        public async Task LoadFile(Stream stream, params string[] pathParts)
        {
            var fileInfo = new FileInfo(Path.Combine(pathParts));

            if (fileInfo.Directory is { Exists: false })
            {
                fileInfo.Directory.Create();
            }

            await using var fileStream = fileInfo.Create();

            await stream.CopyToAsync(fileStream);
        }

        public Task<FileModel> GetFile(string fileName, params string[] pathFolderParts)
        {
            return Task.FromResult(new FileModel()
            {
                FileName = fileName,
                Content = new FileStream(GetPath(fileName, pathFolderParts), FileMode.Open)
            });
        }

        public Task<FileModel> GetFile(params string[] pathParts)
        {
            var fileInfo = new FileInfo(Path.Combine(pathParts));
            return Task.FromResult(new FileModel()
            {
                FileName = fileInfo.Name,
                Content = fileInfo.OpenRead()
            });
        }

        private string GetPath(string fileName, params string[] pathFolderParts)
        {
            var folderPath = Path.Combine(pathFolderParts);
            return Path.Combine(folderPath, fileName);
        }

    }
}
