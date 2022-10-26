using Skalm.Display;
using Skalm.Utilities;

namespace Skalm.Animation
{
    internal class Animator
    {
        public Animation fireAnim1;
        public Animation fireAnim2;
        public Animation skullAnim;

        private ISettings _settings;
        private DisplayManager _displayManager;
        private List<char> _animationTest;
        private int _animationFrameRate;

        public Animator(DisplayManager displayManager, ISettings settings)
        {
            _settings = settings;
            _displayManager = displayManager;
            _animationTest = new List<char> { ' ', '░', '▒', '▓', '█', '▓', '▒', '░' };

            fireAnim1 = CreateFireAnimation(0);
            fireAnim2 = CreateFireAnimation(1);
            skullAnim = CreateSkullAnimation();

            _animationFrameRate = 0;
        }
        public void AnimatedBraziers()
        {
            if (_animationFrameRate >= 4)
            {
                _animationFrameRate = 0;

                _displayManager.Printer.PrintFromPosition(fireAnim1.NextFrame().Lines, 4, Console.WindowWidth / 2 - Console.WindowWidth / 4 - 4, _settings.TextColor);
                _displayManager.Printer.PrintFromPosition(fireAnim2.NextFrame().Lines, 4, Console.WindowWidth / 2 + Console.WindowWidth / 4, _settings.TextColor);

            }
            _animationFrameRate++;
        }

        public void LaughingSkull()
        {
            if (_animationFrameRate >= 4)
            {
                _animationFrameRate = 0;

                _displayManager.Printer.PrintCenteredInWindow(skullAnim.NextFrame().Lines, Console.WindowHeight / 2 - 7, _settings.TextColor);
            }
            _animationFrameRate += 2;
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
        private Animation CreateSkullAnimation()
        {
            Frame f1 = new Frame(
                @"                ",
                @"                ",
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ");

            Frame f2 = new Frame(
                @"                ",
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | |      | |  ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ");

            Frame f3 = new Frame(
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | |      | |  ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ",
                @"                ");

            Frame f4 = new Frame(
                @"                ",
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | |      | |  ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ");

            Frame f5 = new Frame(
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | |      | |  ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ",
                @"                ");

            Frame f6 = new Frame(
                @"                ",
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | |      | |  ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ");

            Frame f7 = new Frame(
                @"                ",
                @"                ",
                @"  .----------.  ",
                @" /            \ ",
                @"|              |",
                @"|,  .-.  .-.  ,|",
                @"| )(__/  \__)( |",
                @"|/     /\     \|",
                @"(_    ^ ^     _)",
                @" \__|IIIIII|__/ ",
                @"  | \IIIIII/ |  ",
                @"  \          /  ",
                @"   `--------`   ",
                @"                ");

            return new Animation(f1, f2, f3, f4, f5, f6, f7);
        }
    }
}
