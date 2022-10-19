using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Interfaces
{
    internal interface IGridObject
    {
        Vector2Int GridPosition { get; }
        char Sprite { get; }
        ConsoleColor Color { get; }
        string Label { get; }
    }
}
