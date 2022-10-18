using Skalm.GameObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Structs
{
    internal struct DoDamage
    {
        public float damage;
        public Vector2Int originXY;
        public IDamageable sender;
    }
}
