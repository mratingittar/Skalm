using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Utilities
{
    internal static class TextTools
    {
        public static string AddPointersToString(string str, int padding)
        {
            return "►" + RepeatChar(' ', padding) + str + RepeatChar(' ', padding) + "◄";
        }
        public static string[] AddDoubleBordersToText(string str)
        {
            string[] result = new string[3];

            result[0] = '╔' + RepeatChar('═', str.Length) + '╗';
            result[1] = '║' + str + '║';
            result[2] = '╚' + RepeatChar('═', str.Length) + '╝';

            return result;
        }

        public static string[] AddHeavyBordersToText(string str)
        {
            string[] result = new string[3];

            result[0] = '┏' + RepeatChar('━', str.Length) + '┓';
            result[1] = '┃' + str + '┃';
            result[2] = '┗' + RepeatChar('━', str.Length) + '┛';

            return result;
        }

        public static string[] AddLightBordersToText(string str)
        {
            string[] result = new string[3];

            result[0] = '┌' + RepeatChar('─', str.Length) + '┐';
            result[1] = '│' + str + '│';
            result[2] = '└' + RepeatChar('─', str.Length) + '┘';

            return result;
        }

        public static string RepeatChar(char ch, int count)
        {
            string result = "";
            for (int i = 0; i < count; i++)
            {
                result += ch;
            }
            return result;
        }
    }
}
