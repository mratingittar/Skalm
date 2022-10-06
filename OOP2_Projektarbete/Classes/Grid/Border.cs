using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Grid
{
    internal class Border : IGridObject
    {
        public Vector2Int GridPosition { get; set; }
        public List<Vector2Int> ConsolePositions { get; set; }
        public char CharacterRepresentation { get; set; }
        public bool Empty;

        public Border(Vector2Int gridPosition, List<Vector2Int> consolePositions)
        {
            GridPosition = gridPosition;
            ConsolePositions = consolePositions;
            CharacterRepresentation = '█';
        }
    }
}
