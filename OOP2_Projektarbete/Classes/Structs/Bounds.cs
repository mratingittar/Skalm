namespace OOP2_Projektarbete.Classes.Structs
{
    internal struct Bounds
    {
        public Vector2Int StartXY { get; private set; }
        public Vector2Int EndXY { get; private set; }
        public Rectangle Size { get; private set; }


        public Bounds(Vector2Int startXY, Vector2Int endXY)
        {
            if (endXY.X < startXY.X || endXY.Y < startXY.Y)
                throw new ArgumentException("End point coordinates must be bigger than start point coordinates.");

            StartXY = startXY;
            EndXY = endXY;
            Size = new Rectangle(endXY.X - startXY.X, endXY.Y - startXY.Y);
        }
        public Bounds(Vector2Int startXY, Rectangle size)
        {
            StartXY = startXY;
            EndXY = new Vector2Int(startXY.X + size.Width, startXY.Y + size.Height);
            Size = size;
        }
    }
}
