using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete
{
    internal static class Globals
    {
        public static string G_GAME_TITLE = "Skälm";
        public static bool G_DISPLAY_CURSOR = false;
        public static int G_UPDATE_FREQUENCY = 10;

        public static int G_WINDOW_PADDING = 1;
        public static int G_BORDER_THICKNESS = 1;
        public static int G_CELL_WIDTH = 2;
        public static int G_CELL_HEIGHT = 1;
        public static int G_MAP_WIDTH = 42;
        public static int G_MAP_HEIGHT = 32;

        public static int G_HUD_MSGBOX_W = 96;
        public static int G_HUD_MSGBOX_H = 4;
        public static int G_HUD_MAINSTATS_W = 24;
        public static int G_HUD_MAINSTATS_H = 8;
        public static int G_HUD_SUBSTATS_W = 24;
        public static int G_HUD_SUBSTATS_H = 28;

        public static int G_HUD_PADDING = 1;
        public static int G_HUD_PADDING_ALL = 1;

        public static char G_BORDER_CHAR = '░'; //'█' || '\u2588'
        public static char G_WALL = '#';
        public static char G_FLOOR = '.';
        public static char G_DOOR = '+';

        public static bool G_HALFWIDTHDRAW = true;
    }
}
