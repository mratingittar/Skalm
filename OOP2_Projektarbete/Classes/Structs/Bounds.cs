namespace OOP2_Projektarbete.Classes.Structs
{
    internal struct Bounds
    {
        public Vector2Int StartXY { get; private set; }
        public Vector2Int EndXY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Bounds(Vector2Int startXY, Vector2Int endXY)
        {
            if (endXY.X < startXY.X || endXY.Y < startXY.Y)
                throw new ArgumentException("End point coordinates must be bigger than start point coordinates.");

            this.StartXY = startXY;
            this.EndXY = endXY;
            Width = endXY.X - startXY.X;
            Height = endXY.Y - startXY.Y;
        }
    }
}
