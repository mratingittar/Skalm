﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal interface IGameState : IState
    {
        GameManager GameManager { get; }

        void UpdateLogic();
        void UpdateDisplay();
    }
}
