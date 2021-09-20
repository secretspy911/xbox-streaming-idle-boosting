using GameBoosterNS;
using System;

namespace XboxStreamingIdleBoostingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowHelper.SetTopMost();
            Console.WindowWidth = 30;
            Console.WindowHeight = 5;

            Console.ReadLine();

            GameBooster gameBooster = new GameBooster();
            gameBooster.Log += Console.WriteLine;
            gameBooster.Start(false);

            Console.ReadLine();
        }
    }
}
