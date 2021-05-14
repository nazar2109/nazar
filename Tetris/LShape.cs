namespace Tetris
{
    class LShape : Shape
    {
        public LShape()
        {
            this.form = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        }
    }
}