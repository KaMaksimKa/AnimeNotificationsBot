using System.Security.Cryptography;

namespace AnimeNotificationsBot.BLL.Helpers
{
    public static class HashHelper
    {
        public static string CalculateMD5(Stream stream)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
