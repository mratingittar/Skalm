﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Interfaces
{
    internal interface IOccupiable
    {
        Stack<GameObject> ObjectsOnTile { get; }
        bool ActorPresent { get; set; }
    }
}
