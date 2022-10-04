using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.HUD
{
    internal class HUDmainStats
    {
        // LISTEN FOR CHANGES TO DISPLAYED STATS
        public Action onMainStatsChanged;

        // CONSTRUCTOR I
        public HUDmainStats()
            => onMainStatsChanged += UpdateMainStatsHUD;

        // UPDATE STATS HUD
        public void UpdateMainStatsHUD()
        {

        }
    }
}
