using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace XboxStreamingIdleBoosting.Games
{
    class SuperBomberman
    {
        private const int MoveOneSquareDelay = 225;
        private const int MinimumMoveOneSquareDelay = 100;
        private const int MatchEndDelay = 10000;
        private const int BombDetonationDelay = 3000;
        private XboxController xboxController;
        private Timer bombTimer;
        private bool bombPlaced;

        public SuperBomberman(XboxController xboxController)
        {
            this.xboxController = xboxController;
            bombTimer = new Timer(BombExploded);
        }

        private void BombExploded(object state)
        {
            bombPlaced = false;
            bombTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

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
        /// Players: 1 Player, 1 COM
        /// </summary>
        public void StartIdleBoosting()
        {
            //PlayMatch();
            PlaceBomb();
            WaitForBombToExplode();
            WaitForNextMatch();
            PlaceBomb();
            WaitForBombToExplode();
        }

        private void Move(Direction direction, int nbSquares)
        {
            xboxController.Move(direction, 1 * MinimumMoveOneSquareDelay);
            xboxController.Move(direction, (nbSquares - 1) * MoveOneSquareDelay);
        }

        private void PlaceBomb()
        {
            xboxController.PressButton(XboxController.Button.B);
            bombPlaced = true;
            bombTimer.Change(BombDetonationDelay, Timeout.Infinite);
        }

        private void WaitForBombToExplode()
        {
            while (bombPlaced) { }
        }

        private void WaitForNextMatch()
        {
            Thread.Sleep(MatchEndDelay);
        }

        private void PlayMatch()
        {
            // Bomb 1
            Move(Direction.Right, 1);
            PlaceBomb(); //1,2
            Move(Direction.Left, 1);
            Move(Direction.Down, 1);
            WaitForBombToExplode(); //2,1 

            // Bomb 2
            Move(Direction.Up, 1);
            Move(Direction.Right, 2);
            PlaceBomb(); // 1,3
            Move(Direction.Left, 2);
            Move(Direction.Down, 1);
            WaitForBombToExplode(); //2,1

            // Bomb 3
            Move(Direction.Up, 1);
            Move(Direction.Right, 3);
            PlaceBomb(); //1,4
            Move(Direction.Left, 1);
            Move(Direction.Down, 1);
            WaitForBombToExplode(); //2,3

            // Bomb 4
            PlaceBomb(); //2,3
            Move(Direction.Up, 1);
            Move(Direction.Right, 2);
            WaitForBombToExplode(); //1,5

            // Bomb 5
            PlaceBomb(); //1,5
            Move(Direction.Left, 2);
            Move(Direction.Down, 2);
            WaitForBombToExplode(); //3,3

            // Bomb 6
            PlaceBomb(); //3,3
            Move(Direction.Up, 2);
            Move(Direction.Left, 2);
            Move(Direction.Down, 1);
            WaitForBombToExplode(); //2,1

            // Bomb 7
            PlaceBomb(); //2,1
            Move(Direction.Up, 1);
            Move(Direction.Right, 2);
            Move(Direction.Down, 3);
            WaitForBombToExplode(); // 4,3

            // Bomb 8
            PlaceBomb(); // 4,3
            Move(Direction.Up, 1);
            Move(Direction.Left, 2);
            WaitForBombToExplode(); // 3,1

            // Bomb 9
            PlaceBomb(); // 3,1
            Move(Direction.Right, 2);
            Move(Direction.Down, 2);
            WaitForBombToExplode(); // 5,3

            // Bomb 10
            PlaceBomb(); // 5,3
            Move(Direction.Right, 2);
            Move(Direction.Up, 1);
            WaitForBombToExplode(); // 4,5

            // Bomb 11
            Move(Direction.Down, 1);
            Move(Direction.Left, 2);
            Move(Direction.Down, 2);
            PlaceBomb(); // 7,3
            Move(Direction.Right, 2);
            Move(Direction.Down, 2);
            WaitForBombToExplode(); // 9,5

            // Bomb 12
            PlaceBomb(); // 9,5
            Move(Direction.Up, 2);
            Move(Direction.Left, 2);
            Move(Direction.Down, 1);
            WaitForBombToExplode(); // 8,3

            // Bomb 13
            PlaceBomb(); // 8,3
            Move(Direction.Up, 1);
            Move(Direction.Right, 2);
            Move(Direction.Down, 3);
            WaitForBombToExplode(); // 10,5

            // Bomb 14
            PlaceBomb(); // 10,5
            Move(Direction.Up, 1);
            Move(Direction.Left, 2);
            WaitForBombToExplode(); // 9,3

            // Bomb 15
            PlaceBomb(); // 9,3
            Move(Direction.Right, 2);
            Move(Direction.Down, 2);
            WaitForBombToExplode(); // 11,5

            // Bomb 16
            PlaceBomb(); // 11,5
            Move(Direction.Up, 2);
            Move(Direction.Left, 2);
            Move(Direction.Down, 1);
            WaitForBombToExplode(); // 10,3

            // Bomb 17
            PlaceBomb(); // 10,3
            Move(Direction.Up, 1);
            Move(Direction.Left, 1);
            WaitForBombToExplode(); // 9,2

            // Bomb 18
            PlaceBomb(); // 9,2
            Move(Direction.Right, 1);
            Move(Direction.Up, 2);
            Move(Direction.Left, 1);
            WaitForBombToExplode(); // 7,2

            // Bomb 19
            PlaceBomb(); // 7,2
            Move(Direction.Right, 1);
            Move(Direction.Down, 2);
            Move(Direction.Left, 2);
            WaitForBombToExplode(); // 9,1

            // Bomb 20
            PlaceBomb(); // 9,1
            Move(Direction.Right, 2);
            Move(Direction.Up, 2);
            Move(Direction.Left, 2);
            WaitForBombToExplode(); // 7,1

            // Bomb 21
            PlaceBomb(); // 7,1
            // Let self-destruct
        }
    }
}
