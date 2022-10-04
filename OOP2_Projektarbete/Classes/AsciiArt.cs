using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes
{
    internal struct AsciiArt
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
    }
}
