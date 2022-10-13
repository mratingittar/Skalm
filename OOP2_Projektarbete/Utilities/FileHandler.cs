using System.Text;

namespace Skalm.Utilities
{
    internal static class FileHandler
    {
        public static bool TryReadFile(string fileName, out string[] file)
        {
            string rootFolder = Directory.GetCurrentDirectory() + "/";
            bool success;
            try
            {
                file = File.ReadAllLines(rootFolder + fileName, Encoding.UTF8);
                success = true;
            }
            catch (Exception)
            {
                file = new string[0];
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
