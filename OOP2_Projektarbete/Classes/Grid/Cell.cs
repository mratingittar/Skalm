﻿using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Grid
{
    internal class Cell : IGridObject
    {
        public Vector2Int GridPosition { get; set; }
        public List<Vector2Int> ConsolePositions { get; set; }
        public char CharacterRepresentation { get; set; }

        public Cell(Vector2Int gridPosition, List<Vector2Int> consolePositions)
        {
            GridPosition = gridPosition;
            ConsolePositions = consolePositions;
            CharacterRepresentation = '*';
        }

    }
}
