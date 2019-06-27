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
            if (usePlasmaBombs)
                xboxController.PressButton(XboxController.Button.A);
            else
                xboxController.PressButton(XboxController.Button.B);

            bombPlaced = true;

            int bombDelay;
            if (usePlasmaBombs)
                bombDelay = PlasmaBombDetonationDelay;
            else
                bombDelay = BombDetonationDelay;
            bombTimer.Change(bombDelay, Timeout.Infinite);
        }

        // Bombs need to be placed twice to make sure any powerup is destroyed.
        private void PlaceSecondBomb()
        {
            RevertLastMovements();
            PlaceBomb();
            RevertLastMovements();
            WaitForBombToExplode();
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

        private void PlaceNextBomb(Movement[] movesAwayFromBomb)
        {
            PlaceNextBomb(null, movesAwayFromBomb);
        }

        private void PlaceNextBomb(Movement[] movesToBomb, Movement[] movesAwayFromBomb)
        {
            if (movesToBomb != null)
            {
                foreach (Movement movement in movesToBomb)
                {
                    Move(movement.Direction, movement.NbSquares);
                }
            }

            PlaceBomb();
            lastMovements.Clear();

            if (movesAwayFromBomb != null)
            {
                foreach (Movement movement in movesAwayFromBomb)
                {
                    Move(movement.Direction, movement.NbSquares);
                }
            }

            WaitForBombToExplode();
            PlaceSecondBomb();
        }

        private void PlaySet()
        {
            // Moving the character is too unreliable. It can pick up speed powerups which makes impossible to accurately calculate moving delays.
            // So, to be safe and minimize to the minimum the risk of losing sync with the game, we go for a self-destruct bomb right at the start.
            // It's far from being optimal, but at least it's stable.
            //PlaceBomb();
            //WaitForBombToExplode();

            // Bomb 1
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1) },
                new[] { new Movement(Direction.Left, 1), new Movement(Direction.Down, 1) });

            // Bomb 2
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 2) },
                new[] { new Movement(Direction.Left, 2), new Movement(Direction.Down, 1) });

            // Bomb 3
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 3) },
                new[] { new Movement(Direction.Left, 1), new Movement(Direction.Down, 1) });

            // Bomb 4
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 2) });

            // Bomb 5
            PlaceNextBomb(new[] { new Movement(Direction.Left, 2), new Movement(Direction.Down, 2) });

            // Bomb 6
            PlaceNextBomb(new[] { new Movement(Direction.Up, 2), new Movement(Direction.Left, 2), new Movement(Direction.Down, 1) });

            // Bomb 7
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 2), new Movement(Direction.Down, 3) });

            // Bomb 8
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 2) });

            // Bomb 9
            PlaceNextBomb(new[] { new Movement(Direction.Right, 2), new Movement(Direction.Down, 2) });

            // Bomb 10
            PlaceNextBomb(new[] { new Movement(Direction.Right, 2), new Movement(Direction.Up, 1) });

            // Bomb 11
            PlaceNextBomb(new[] { new Movement(Direction.Down, 1), new Movement(Direction.Left, 2), new Movement(Direction.Down, 2) },
                new[] { new Movement(Direction.Right, 2), new Movement(Direction.Down, 2) });

            // Bomb 12
            PlaceNextBomb(new[] { new Movement(Direction.Up, 2), new Movement(Direction.Left, 2), new Movement(Direction.Down, 1) });

            // Bomb 13
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 2), new Movement(Direction.Down, 3) });

            // Bomb 14
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 2) });

            // Bomb 15
            PlaceNextBomb(new[] { new Movement(Direction.Right, 2), new Movement(Direction.Down, 2) });

            // Bomb 16
            PlaceNextBomb(new[] { new Movement(Direction.Up, 2), new Movement(Direction.Left, 2), new Movement(Direction.Down, 1) });

            // Bomb 17
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 1) });

            // Bomb 18
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1), new Movement(Direction.Up, 2), new Movement(Direction.Left, 1) });

            // Bomb 19
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1), new Movement(Direction.Down, 2), new Movement(Direction.Left, 2) });

            // Bomb 20
            PlaceNextBomb(new[] { new Movement(Direction.Right, 2), new Movement(Direction.Up, 2), new Movement(Direction.Left, 2) });

            // Bomb 21
            PlaceBomb(); // 7,1
            // Let self-destruct/
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
