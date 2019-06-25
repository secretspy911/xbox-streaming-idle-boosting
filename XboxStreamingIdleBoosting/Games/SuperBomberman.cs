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
        private const int SetStartDelay = 10000;
        private const int BombDetonationDelay = 3000;
        private const int PlasmaBombDetonationDelay = 1500;
        private const int MenuNavigationDelay = 1000;
        private const int BattleEndToMenuDelay = 8000;
        private const int NumberOfSets = 5;

        private XboxController xboxController;
        private Timer bombTimer;
        private bool bombPlaced;
        private List<Movement> lastMovements = new List<Movement>();
        private bool usePlasmaBombs;

        public delegate void LogEventHandler(string message);
        public event LogEventHandler Log;

        public SuperBomberman(XboxController xboxController, bool usePlasmaBombs)
        {
            this.xboxController = xboxController;
            this.usePlasmaBombs = usePlasmaBombs;
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
        /// Map: The Great Wall (map with the most immuable blocks)
        /// Players: 1 Player, 1 COM
        /// </summary>
        public void StartIdleBoosting()
        {
            DateTime startTime = DateTime.Now;
            int blockDestroyedCount = 0;
            while (true)
            {
                for (int i = 1; i <= NumberOfSets; i++)
                {
                    PlaySet();
                    blockDestroyedCount += 2;
                    if (i < NumberOfSets)
                    {
                        Thread.Sleep(SetStartDelay);
                    }
                }
                Log("Boosting started at: " + startTime.ToShortTimeString() + "." + Environment.NewLine + "Blocks destroyed: " + blockDestroyedCount + "." + (blockDestroyedCount / ((DateTime.Now - startTime).TotalSeconds / 60)) + " bombs/min.");
                NavigateMenuForNextBattle();
            }
        }

        private void Move(Direction direction, int nbSquares)
        {
            xboxController.Move(direction, nbSquares - 1 * MoveOneSquareDelay);
            lastMovements.Add(new Movement(direction, nbSquares));
        }

        private void RevertLastMovements()
        {
            for (int i = lastMovements.Count; i == 1; i--)
            {
                Movement movement = lastMovements[i];
                lastMovements.Remove(movement);
                Move(GetOppositeDirection(movement.Direction), movement.NbSquares);
            }
        }

        private Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                default:
                    throw new NotImplementedException();
            }
        }

        private void PlaceBomb()
        {
            PlaceBomb(false);
        }

        private void PlaceBomb(bool clearLastMovements)
        {
            if (usePlasmaBombs)
                xboxController.PressButton(XboxController.Button.A);
            else
                xboxController.PressButton(XboxController.Button.B);

            if (clearLastMovements) lastMovements.Clear();
            bombPlaced = true;

            int bombDelay;
            if (usePlasmaBombs)
                bombDelay = PlasmaBombDetonationDelay;
            else
                bombDelay = BombDetonationDelay;
            bombTimer.Change(bombDelay, Timeout.Infinite);
        }

        private void WaitForBombToExplode()
        {
            while (bombPlaced) { }
        }

        private void NavigateMenuForNextBattle()
        {
            Thread.Sleep(BattleEndToMenuDelay);
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(MenuNavigationDelay);
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(MenuNavigationDelay);
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(SetStartDelay);
        }

        private void PlaySet()
        {
            PlaceBomb();
            WaitForBombToExplode();

            // Moving the character is too unreliable. It can pick up speed powerups which makes impossible to accurately calculate moving delays.
            // So, to be safe and minimize to the minimum the risk of losing sync with the game, we go for a self-destruct bomb right at the start.
            // It's far from being optimal, but at least it's stable.
            // Bomb 1
            //Move(Direction.Right, 1);
            //PlaceBomb(true); //1,2
            //Move(Direction.Left, 1);
            //Move(Direction.Down, 1);
            //WaitForBombToExplode(); //2,1 
            //RevertLastMovements();
            //PlaceBomb(); //1,2
            //RevertLastMovements();
            //WaitForBombToExplode(); // 2,1

            //    // Bomb 2
            //    Move(Direction.Up, 1);
            //    Move(Direction.Right, 2);
            //    PlaceBomb(); // 1,3
            //    Move(Direction.Left, 2);
            //    Move(Direction.Down, 1);
            //    WaitForBombToExplode(); //2,1

            //    // Bomb 3
            //    Move(Direction.Up, 1);
            //    Move(Direction.Right, 3);
            //    PlaceBomb(); //1,4
            //    Move(Direction.Left, 1);
            //    Move(Direction.Down, 1);
            //    WaitForBombToExplode(); //2,3

            //    // Bomb 4
            //    PlaceBomb(); //2,3
            //    Move(Direction.Up, 1);
            //    Move(Direction.Right, 2);
            //    WaitForBombToExplode(); //1,5

            //    // Bomb 5
            //    PlaceBomb(); //1,5
            //    Move(Direction.Left, 2);
            //    Move(Direction.Down, 2);
            //    WaitForBombToExplode(); //3,3

            //    // Bomb 6
            //    PlaceBomb(); //3,3
            //    Move(Direction.Up, 2);
            //    Move(Direction.Left, 2);
            //    Move(Direction.Down, 1);
            //    WaitForBombToExplode(); //2,1

            //    // Bomb 7
            //    PlaceBomb(); //2,1
            //    Move(Direction.Up, 1);
            //    Move(Direction.Right, 2);
            //    Move(Direction.Down, 3);
            //    WaitForBombToExplode(); // 4,3

            //    // Bomb 8
            //    PlaceBomb(); // 4,3
            //    Move(Direction.Up, 1);
            //    Move(Direction.Left, 2);
            //    WaitForBombToExplode(); // 3,1

            //    // Bomb 9
            //    PlaceBomb(); // 3,1
            //    Move(Direction.Right, 2);
            //    Move(Direction.Down, 2);
            //    WaitForBombToExplode(); // 5,3

            //    // Bomb 10
            //    PlaceBomb(); // 5,3
            //    Move(Direction.Right, 2);
            //    Move(Direction.Up, 1);
            //    WaitForBombToExplode(); // 4,5

            //    // Bomb 11
            //    Move(Direction.Down, 1);
            //    Move(Direction.Left, 2);
            //    Move(Direction.Down, 2);
            //    PlaceBomb(); // 7,3
            //    Move(Direction.Right, 2);
            //    Move(Direction.Down, 2);
            //    WaitForBombToExplode(); // 9,5

            //    // Bomb 12
            //    PlaceBomb(); // 9,5
            //    Move(Direction.Up, 2);
            //    Move(Direction.Left, 2);
            //    Move(Direction.Down, 1);
            //    WaitForBombToExplode(); // 8,3

            //    // Bomb 13
            //    PlaceBomb(); // 8,3
            //    Move(Direction.Up, 1);
            //    Move(Direction.Right, 2);
            //    Move(Direction.Down, 3);
            //    WaitForBombToExplode(); // 10,5

            //    // Bomb 14
            //    PlaceBomb(); // 10,5
            //    Move(Direction.Up, 1);
            //    Move(Direction.Left, 2);
            //    WaitForBombToExplode(); // 9,3

            //    // Bomb 15
            //    PlaceBomb(); // 9,3
            //    Move(Direction.Right, 2);
            //    Move(Direction.Down, 2);
            //    WaitForBombToExplode(); // 11,5

            //    // Bomb 16
            //    PlaceBomb(); // 11,5
            //    Move(Direction.Up, 2);
            //    Move(Direction.Left, 2);
            //    Move(Direction.Down, 1);
            //    WaitForBombToExplode(); // 10,3

            //    // Bomb 17
            //    PlaceBomb(); // 10,3
            //    Move(Direction.Up, 1);
            //    Move(Direction.Left, 1);
            //    WaitForBombToExplode(); // 9,2

            //    // Bomb 18
            //    PlaceBomb(); // 9,2
            //    Move(Direction.Right, 1);
            //    Move(Direction.Up, 2);
            //    Move(Direction.Left, 1);
            //    WaitForBombToExplode(); // 7,2

            //    // Bomb 19
            //    PlaceBomb(); // 7,2
            //    Move(Direction.Right, 1);
            //    Move(Direction.Down, 2);
            //    Move(Direction.Left, 2);
            //    WaitForBombToExplode(); // 9,1

            //    // Bomb 20
            //    PlaceBomb(); // 9,1
            //    Move(Direction.Right, 2);
            //    Move(Direction.Up, 2);
            //    Move(Direction.Left, 2);
            //    WaitForBombToExplode(); // 7,1

            //    // Bomb 21
            //    PlaceBomb(); // 7,1
            //    // Let self-destruct/
        }

        private class Movement
        {
            private Direction direction;
            private int nbSquares;

            public Direction Direction { get { return direction; } }
            public int NbSquares { get { return nbSquares; } }

            public Movement(Direction direction, int nbSquares)
            {
                this.direction = direction;
                this.nbSquares = nbSquares;
            }
        }
    }
}
