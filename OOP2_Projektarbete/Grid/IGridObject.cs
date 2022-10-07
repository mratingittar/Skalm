using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Grid
{
    internal interface IGridObject
    {
        Vector2Int GridPosition { get; set; }
        List<Vector2Int> ConsolePositions { get; set; }
        IContentType Content { get; set; }
    }
}
