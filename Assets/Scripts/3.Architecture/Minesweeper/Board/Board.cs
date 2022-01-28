namespace ProductMadness.Architecture
{
    public class Board
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Board(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
