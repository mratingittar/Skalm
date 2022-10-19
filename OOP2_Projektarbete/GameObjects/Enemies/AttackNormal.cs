﻿using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Enemies
{
    internal class AttackNormal : IAttackComponent
    {
        public DoDamage Attack()
        {
            return new DoDamage();
        }
    }
}