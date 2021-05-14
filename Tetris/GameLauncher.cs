using System;
using System.Media;

namespace Tetris
{
    class GameLauncher
    {
        static private Player player = new Player();                           

        static void Main()
        {
            SoundPlayer soundPlayer = new SoundPlayer("../../music/zvuk-tetrisa-na-konsoli.wav"); //Створення та ініціалізація програвача для музики 
            soundPlayer.PlayLooping();                                                            //Запуск на повторне програвання
            Game game = new Game(player);
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
            game.StartGame();
            Console.SetCursorPosition(10, 15);
            Console.WriteLine("\nGame over!");
            Console.WriteLine("Play again?(y/n)");
            string input = Console.ReadLine();
            if (input == "y" || input == "Y")
            {
                game = null;                                                                       //Вказівник стає неактивним, тим самим, попадаючи в збирач сміття
                GC.Collect();                                                                      //Запуск збирача сміття
                player.SetScore(0);
                Main();
            }
        }
    }
}