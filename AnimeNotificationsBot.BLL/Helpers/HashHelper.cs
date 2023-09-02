using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
