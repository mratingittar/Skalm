using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Animation
{
    internal class Animation
    {
        public int FramesCount { get; private set; }
        public Rectangle Size { get; private set; }
        private Frame[] frames;
        private int frameIndex;

        public Animation(params Frame[] frames)
        {
            this.frames = frames;
            FramesCount = frames.Length;
            frameIndex = 0;
            Size = new Rectangle(frames.Select(x => x.Width).Max(), frames.Select(f => f.Lines.Length).Max());
        }

        public Frame NextFrame()
        {
            Frame frame = frames[frameIndex];
            frameIndex++;

            if (frameIndex >= FramesCount)
                frameIndex = 0;

            return frames[frameIndex];
        }
    }
}
