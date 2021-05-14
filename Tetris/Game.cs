using System;
using System.Diagnostics;

namespace Tetris
{
    class Game
    {
        private GameField gameField;        //ігрове поле
        private Player player;              //гравець
        private Stopwatch dropTimer;        //секундомір
        private int dropRate;               //частота викидання фігури
        private Shape shape;                //теперішня фігура
        private Shape nextShape;            //наступна фігура
        private int linesPicked;            //кількість очищених ліній
        private int level;                  //рівень складності гри
        private int width;                  //ширина поля   

        public Game(Player player)
        {
            this.level = 1;
            this.dropRate = 300;
            this.gameField = new GameField();
            this.dropTimer = new Stopwatch();
            this.width = GameField.GetWidth() * 2 + 2;
            this.player = player;
        }

        private void ShowHorizontalBoardLine(int from, int to)
        {
            for (int i = from; i < to; i++)
                Console.Write(gameField.GetCharBoard());
        }

        private void ShowVerticalBoardLine(int height, int left, int right)
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, Console.CursorTop);
                Console.Write(gameField.GetCharBoard());
                Console.SetCursorPosition(right, Console.CursorTop);
                Console.WriteLine(gameField.GetCharBoard());
            }
        }

        private void ShowScoreBlock()
        {
            Console.SetCursorPosition(this.width / 6, 0);
            ShowHorizontalBoardLine(this.width / 6, this.width - this.width / 6);
            Console.SetCursorPosition(this.width / 6, 1);
            ShowVerticalBoardLine(2, this.width / 6, this.width - this.width / 6 - 1);
            Console.SetCursorPosition(this.width / 6, 3);
            ShowHorizontalBoardLine(this.width / 6, this.width - this.width / 6);
            Console.SetCursorPosition(this.width / 2 - 2, 1);
            Console.Write("Score");
            Console.SetCursorPosition(this.width / 2 - 2, 2);
            Console.WriteLine(player.GetScore().ToString("000000") + "\n");
        }

        private void ShowLineBlock()
        {
            Console.SetCursorPosition(0, 4);
            ShowHorizontalBoardLine(0, this.width / 2 - 1);
            Console.WriteLine();
            ShowVerticalBoardLine(3, 0, this.width / 2 - 2);
            ShowHorizontalBoardLine(0, this.width / 2 - 1);
            Console.SetCursorPosition(this.width / 6, 5);
            Console.Write("Line");
            Console.SetCursorPosition(this.width / 6, 6);
            Console.Write(this.linesPicked.ToString("000"));
            Console.SetCursorPosition(0, 9);
        }

        private void ShowNextBlock()
        {
            
            Console.SetCursorPosition(this.width / 2 + 1, 4);
            ShowHorizontalBoardLine(this.width / 2, this.width - 1);
            Console.WriteLine();
            ShowVerticalBoardLine(3, this.width / 2 + 1, this.width - 1);
            Console.SetCursorPosition(this.width / 2 + 1, 8);
            ShowHorizontalBoardLine(this.width / 2, this.width - 1);
            Console.SetCursorPosition(this.width - this.width/ 3, 5);
            Console.Write("Block");
            Console.SetCursorPosition(this.width - this.width / 3, 6);
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(this.width / 2 + 2, 6 + i);
                for (int j = this.width / 2 + 2; j < this.width - 1; j++)
                    Console.Write(" ");
            }
                Console.SetCursorPosition(this.width - this.width / 3, 6);
            if(this.nextShape != null)
            {
                for (int i = 0; i < this.nextShape.GetForm().GetLength(0); i++)
                {
                    for (int j = 0; j < this.nextShape.GetForm().GetLength(1); j++)
                    {
                        Console.SetCursorPosition(this.width - this.width / 3 - 1 + j * 2, 6 + i);
                        if (this.nextShape.GetForm()[i, j] == 1)
                            Console.Write(this.nextShape.GetSymbolShape());
                        else
                            Console.Write(" ");
                    }
                }
            }
            Console.SetCursorPosition(0, 9);
        }

        private void ShowPlayerStat()
        {
            Console.SetCursorPosition(0, GameField.GetHeight() + 11);
            ShowHorizontalBoardLine(0, this.width);
            Console.WriteLine();
            ShowVerticalBoardLine(2, 0, this.width - 1);
            ShowHorizontalBoardLine(0, this.width);
            Console.SetCursorPosition(this.width / 3, Console.CursorTop - 2);
            Console.Write(player.GetName());
            Console.SetCursorPosition(this.width / 3, Console.CursorTop + 1);
            Console.Write("Level: " + level.ToString("000"));
        }

        public void StartGame()
        {
            Console.Clear();
            this.dropTimer.Start();
            this.shape = SetNewFigure();
            this.nextShape = SetNewFigure();
            ShowScoreBlock();
            ShowLineBlock();
            ShowNextBlock();
            this.gameField.DrawMarginBoard();
            ShowPlayerStat();
            this.shape.Spawn();
            Update();
        }
        
        private Shape SetNewFigure()
        {
            Random numOfShape = new Random();
            Shape figure = null;
            switch (numOfShape.Next(0, 7))
            {
                case 0: figure = new StraightShape(); break;
                case 1: figure = new SquareShape(); break;
                case 2: figure = new TShape(); break;
                case 3: figure = new S_Shape(); break;
                case 4: figure = new ZShape(); break;
                case 5: figure = new JShape(); break;
                case 6: figure = new LShape(); break;
            }
            return figure;
        }

        public void Update()
        {
            while (true)
            {
                int dropTime = (int)this.dropTimer.ElapsedMilliseconds;
                if (dropTime > this.dropRate)
                {
                    this.dropTimer.Restart();
                    this.shape.Drop();
                }

                if (this.shape.GetIsDropped() == true)
                {
                    this.shape = this.nextShape;
                    this.nextShape = SetNewFigure();
                    ShowNextBlock();
                    this.shape.Spawn();
                }

                for (int j = 0; j < GameField.GetWidth(); j++)
                    if (GameField.GetDroppedShapeLocationGrid()[0, j] == 1)
                        return;

                Move();
                ClearLine();
            }
        }

        private void Move()
        {
            if (!Console.KeyAvailable) return;
            ConsoleKeyInfo consoleKeyInfo;
            consoleKeyInfo = Console.ReadKey();

            if (consoleKeyInfo.Key == ConsoleKey.LeftArrow & !this.shape.IsShapeLeft())
            {
                for (int i = 0; i < this.shape.GetLocation().Count; i++)
                    this.shape.GetLocation()[i][1] -= 1;
                this.shape.Update();
            }
            else if (consoleKeyInfo.Key == ConsoleKey.RightArrow & !this.shape.IsShapeRight())
            {
                for (int i = 0; i < this.shape.GetLocation().Count; i++)
                    this.shape.GetLocation()[i][1] += 1;
                this.shape.Update();
            }

            if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                this.shape.Drop();

            if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                while(!this.shape.IsShapeBelow())
                    this.shape.Drop();

            if (consoleKeyInfo.Key == ConsoleKey.Spacebar)
            {
                 this.shape.Turn();
                 this.shape.Update();
            }
        }

        private void ClearLine()
        {
            int combo = 0;
            for (int i = 0; i < GameField.GetHeight(); i++)
            {
                int j;
                for (j = 0; j < GameField.GetWidth(); j++)
                    if (GameField.GetDroppedShapeLocationGrid()[i, j] == 0)
                        break;

                if (j == GameField.GetWidth())
                {
                    linesPicked++;
                    combo++;
                    ShowLineBlock();

                    for (j = 0; j < GameField.GetWidth(); j++)
                        GameField.GetDroppedShapeLocationGrid()[i, j] = 0;

                    int[,] newDroppedShapeLocationGrid = new int[GameField.GetHeight(), GameField.GetWidth()];
                    for (int k = 1; k < i; k++)
                        for (int l = 0; l < GameField.GetWidth(); l++)
                            newDroppedShapeLocationGrid[k + 1, l] = GameField.GetDroppedShapeLocationGrid()[k, l];

                    for (int k = 1; k < i; k++)
                        for (int l = 0; l < GameField.GetWidth(); l++)
                            GameField.GetDroppedShapeLocationGrid()[k, l] = 0;

                    for (int k = 0; k < GameField.GetHeight(); k++)
                        for (int l = 0; l < GameField.GetWidth(); l++)
                            if (newDroppedShapeLocationGrid[k, l] == 1)
                                GameField.GetDroppedShapeLocationGrid()[k, l] = 1;
                    GameField.DrawShape(this.shape);

                    if (combo == 1)
                        this.player.SetScore(this.player.GetScore() + 100);
                    else if (combo == 2)
                        this.player.SetScore(this.player.GetScore() + 300);
                    else if (combo == 3)
                        this.player.SetScore(this.player.GetScore() + 700);
                    else if (combo > 3)
                        this.player.SetScore(this.player.GetScore() + 1500);
                    ShowScoreBlock();

                    if (linesPicked < 5) level = 1;
                    else if (linesPicked < 10) level = 2;
                    else if (linesPicked < 15) level = 3;
                    else if (linesPicked < 25) level = 4;
                    else if (linesPicked < 35) level = 5;
                    else if (linesPicked < 50) level = 6;
                    else if (linesPicked < 70) level = 7;
                    else if (linesPicked < 90) level = 8;
                    else if (linesPicked < 110) level = 9;
                    else if (linesPicked < 150) level = 10;
                    ShowPlayerStat();
                }
            }
            dropRate = 300 - 22 * level;
        }
    }
}
