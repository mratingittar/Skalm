using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Sounds
{
    internal struct Sound
    {
        public string soundName;
        public string fileName;

        public Sound(string soundName, string fileName)
        {
            this.soundName = soundName;
            this.fileName = fileName;
        }
    }
}
