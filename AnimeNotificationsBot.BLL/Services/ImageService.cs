using AnimeNotificationsBot.BLL.Configs;
using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Models;
using Microsoft.Extensions.Options;

namespace AnimeNotificationsBot.BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly IFileService _fileService;
        private readonly ImageConfig _config;

        public ImageService(IFileService fileService, IOptions<ImageConfig> options)
        {
            _fileService = fileService;
            _config = options.Value;
        }
        public async Task<FileInfoModel> LoadImage(Stream stream)
        {
            var hash = HashHelper.CalculateMD5(stream);
            var fileName = hash.Substring(2);
            var path = Path.Combine(_config.BasePath, hash.Substring(0, 2), fileName);
            await _fileService.LoadFile(stream, path);

            return new FileInfoModel()
            {
                Name = fileName,
                Path = path
            };
        }

        public async Task<FileModel> GetImage(params string[] pathParts)
        {
            return await _fileService.GetFile(pathParts);
        }
    }
}
