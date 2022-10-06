using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Grid
{
    internal interface IGridObject
    {
        Vector2Int GridPosition { get; set; }
        List<Vector2Int> ConsolePositions { get; set; }
        char CharacterRepresentation { get; set; }
    }
}
