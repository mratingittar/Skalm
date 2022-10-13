﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal interface IGameState
    {
        GameManager gameManager { get; }

        void Enter();
        void Exit();

        void UpdateLogic();
        void UpdateDisplay();
    }
}
