namespace OOP2_Projektarbete.Classes
{
    internal class AsciiArt
    {
        public readonly string SkalmTitle;

        public AsciiArt()
        {
            SkalmTitle = @"
   _______  _        __   __  _        _______ 
  (  ____ \| \    /\(__) (__)( \      (       )
  | (    \/|  \  / / _______ | (      | () () |
  | (_____ |  (_/ / (  ___  )| |      | || || |
  (_____  )|   _ (  | (___) || |      | |(_)| |
        ) ||  ( \ \ |  ___  || |      | |   | |
  /\____) ||  /  \ \| )   ( || (____/\| )   ( |
  \_______)|_/    \/|/     \|(_______/|/     \|
                                               ";
        }

        public void PrintFromPlace(int col, int row, string str)
        {
            string[] lines = str.Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(col, i+row);
                Console.WriteLine(lines[i]);

            }

            foreach (string line in lines)
            {
            }
        }
    }
}
