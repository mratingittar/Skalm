using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
                file = File.ReadAllLines(rootFolder+fileName, UTF8Encoding.UTF8);
                success = true;
            }
            catch (Exception)
            {
                file = null;
                success = false;
            }
            return success;
        }

        public static void WriteFile(string fileName, string[] file)
        {
            string rootFolder = Directory.GetCurrentDirectory() + "/";
            File.WriteAllLines(rootFolder+fileName, file);
        }
    }
}
