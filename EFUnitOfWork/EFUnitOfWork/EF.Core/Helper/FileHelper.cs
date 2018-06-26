using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EF.Core.Helper
{
    public static class FileHelper
    {
        public static bool Exist(string path)
        {
            return File.Exists(path);
        }

        public static void Delete(string path)
        {
            File.Delete(path);
        }

        public static bool ExistDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public static string GetExtensionName(string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static char GetDirectorySeparatorChar()
        {
            return Path.DirectorySeparatorChar;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        public static string GetMD5HashOfFile(string file)
        {
            var md5 = new MD5CryptoServiceProvider();
            var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
            md5.ComputeHash(stream);
            stream.Close();

            byte[] hash = md5.Hash;
            var sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(string.Format("{0:X2}", b));
            }

            return sb.ToString();
        }
    }
}
