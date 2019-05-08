using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XboxStreamingIdleBoosting.Games
{
    class SuperBomberman
    {
        private const int MoveOneSquareDelay = 225;
        private const int BombDetonationDelay = 2500;

        /// <summary>
        /// The Xbox App must be at the beginning of a new match.
        /// Must use these match settings:
        /// Rules: Battle Royale
        /// Player: 8P
        /// COM Level: Normal (so it's less likely to kill itself)
        /// Sets: 5
        /// Time: 3
        /// Start Position: Fixed
        /// Others: Off
        /// Map: The Great Wall (map with the most immuable blocks)
        /// </summary>
        public void StartIdleBoosting()
        {
            PlayMatch();
        }

        private void Move(Direction direction, int nbSquares)
        {
            MainForm.XboxController.Move(direction, nbSquares * MoveOneSquareDelay);
        }

        private void PlaceBomb()
        {
            MainForm.XboxController.PressButton(XboxController.Button.B);
        }

        private void WaitForBomb()
        {
            Thread.Sleep(BombDetonationDelay);
        }

        private void PlayMatch()
        {
            // Bomb 1
            Move(Direction.Right, 1);
            PlaceBomb(); //1,2
            Move(Direction.Left, 1);
            Move(Direction.Down, 1);
            WaitForBomb(); //2,1 

            // Bomb 2
            Move(Direction.Up, 1);
            Move(Direction.Right, 2);
            PlaceBomb(); // 1,3
            Move(Direction.Left, 2);
            Move(Direction.Down, 1);
            WaitForBomb(); //2,1

            // Bomb 3
            Move(Direction.Up, 1);
            Move(Direction.Right, 3);
            PlaceBomb(); //1,4
            Move(Direction.Left, 1);
            Move(Direction.Down, 1);
            WaitForBomb(); //2,3

            // Bomb 4
            PlaceBomb(); //2,3
            Move(Direction.Up, 1);
            Move(Direction.Right, 2);
            WaitForBomb(); //1,5

            // Bomb 5
            PlaceBomb(); //1,5
            Move(Direction.Left, 2);
            Move(Direction.Down, 2);
            WaitForBomb(); //3,3

            // Bomb 6
            PlaceBomb(); //3,3
            Move(Direction.Up, 2);
            Move(Direction.Left, 2);
            Move(Direction.Down, 1);
            WaitForBomb(); //2,1

            // Bomb 7
            //PlaceBomb(); //2,1
            //Move(Direction.Down, 1);
            //Move(Direction.Right, 4);
            //PlaceBomb();
            //Move(Direction.Left, 2);
            //Move(Direction.Down, 2);
            //WaitForBomb();

            // Bomb 8
            //PlaceBomb();
            //Move(Direction.Up, 2);
            //Move(Direction.Left, 1);
            //WaitForBomb();
        }
    }
}
