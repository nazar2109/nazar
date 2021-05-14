namespace Tetris
{
    class JShape : Shape
    {
        public JShape()
        {
            this.form = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        }
    }
}