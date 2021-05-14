using System;

namespace Tetris
{
    class GameField
    {
        protected static int width;                         //Ширина поля
        protected static int height;                        //Довжина поля
        protected static int[,] grid;                       //Сітка ігрового поля
        protected static int[,] droppedShapeLocationGrid;   //Сітка викинутої фігури
        protected char board;                               //Символ для виводу меж

        public GameField()
        {
            width = 10;
            height = 23;
            grid = new int[height, width];
            droppedShapeLocationGrid = new int[height, width];
            this.board = '*';
        }

        public static int GetWidth() { return width; }
        public static int GetHeight() { return height; }
        public static void SetGridValue(int i, int j, int value) { grid[i, j] = value; } 
        public char GetCharBoard() { return this.board; }
        public static int[,] GetDroppedShapeLocationGrid() { return droppedShapeLocationGrid; }

        public void DrawMarginBoard()
        {
            for (int widthCount = 0; widthCount <= GameField.GetWidth(); widthCount++)
                Console.Write("*-");
            Console.WriteLine();
            int skipLine = Console.CursorTop;
            for (int lengthCount = 0; lengthCount <= height-1; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount + skipLine);
                Console.Write("*");
                Console.SetCursorPosition(width*2+1, lengthCount + skipLine);
                Console.Write("*");
            }
            Console.SetCursorPosition(0, height + skipLine);
            for (int widthCount = 0; widthCount <= width; widthCount++)
                Console.Write("*-");
        } 

        public static void DrawShape(Shape shape)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i + 10);
                    if (grid[i, j] == 1 || droppedShapeLocationGrid[i, j] == 1)
                        Console.Write(shape.GetSymbolShape());
                    else
                        Console.Write("  ");
                }
            }
        }
    }
}