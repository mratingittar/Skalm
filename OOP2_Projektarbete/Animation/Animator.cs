using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Animation
{
    internal class Animator
    {
        private DisplayManager displayManager;
        private List<char> animationTest;

        private int animationFrameRate;
        public Animation fireAnim1;
        public Animation fireAnim2;

        public Animator(DisplayManager displayManager)
        {
            this.displayManager = displayManager;
            animationTest = new List<char> { ' ', '░', '▒', '▓', '█', '▓', '▒', '░' };

            fireAnim1 = CreateFireAnimation(0);
            fireAnim2 = CreateFireAnimation(1);

            animationFrameRate = 0;
        }
        public void AnimatedBraziers()
        {
            if (animationFrameRate == 2)
            {
                animationFrameRate = 0;

                displayManager.Printer.PrintFromPosition(fireAnim1.NextFrame().Lines, 4, Console.WindowWidth / 2 - Console.WindowWidth / 4 - 4);
                displayManager.Printer.PrintFromPosition(fireAnim2.NextFrame().Lines, 4, Console.WindowWidth / 2 + Console.WindowWidth / 4);

            }
            animationFrameRate++;
        }

        private Animation CreateFireAnimation(int offset)
        {
            Frame f1 = new Frame(
                @" (  ",
                @" )\ ",
                @"((_)",
                @" \/ ",
                @" || ",
                @" || ",
                @"/__\");

            Frame f2 = new Frame(
                @" \  ",
                @" )\ ",
                @"(__)",
                @" \/ ",
                @" || ",
                @" || ",
                @"/__\");

            Frame f3 = new Frame(
                @"  ) ",
                @" /( ",
                @"(_))",
                @" \/ ",
                @" || ",
                @" || ",
                @"/__\");

            Frame f4 = new Frame(
                @"  / ",
                @" /( ",
                @"(__)",
                @" \/ ",
                @" || ",
                @" || ",
                @"/__\");

            if (offset == 0)
                return new Animation(f1, f2, f3, f4);
            else if (offset == 1)
                return new Animation(f2, f3, f4, f1);
            else if (offset == 2)
                return new Animation(f3, f4, f1, f2);
            else
                return new Animation(f4, f1, f2, f3);
        }
    }
}
