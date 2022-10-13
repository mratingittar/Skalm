using System.Text;

namespace Skalm
{
    internal static class FileHandler
    {
        public static bool TryReadFile(string fileName, out string[]? file)
        {
            string rootFolder = Directory.GetCurrentDirectory() + "/";
            bool success;
            try
            {
                file = File.ReadAllLines(rootFolder + fileName, UTF8Encoding.UTF8);
                success = true;
            }
            catch (Exception)
            {
                file = null;
                success = false;
            }
            return success;
        }

        public static bool WriteFile(string fileName, string[] file)
        {
            string rootFolder = Directory.GetCurrentDirectory() + "/";
            bool success;
            try
            {
                File.WriteAllLines(rootFolder + fileName, file);
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }
    }
}
