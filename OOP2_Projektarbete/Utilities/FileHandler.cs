using System.Text;

namespace Skalm.Utilities
{
    internal static class FileHandler
    {
#if DEBUG
        public static readonly string rootFolder = Directory.GetCurrentDirectory() + "/..\\..\\..\\data/";
#else
        public static readonly string rootFolder = Directory.GetCurrentDirectory() + "/data/";
#endif
        public static bool TryReadFile(string fileName, out string[] file)
        {
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
