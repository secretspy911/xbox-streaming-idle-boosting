using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace XboxStreamingIdleBoosting.Games
{
    class SuperBomberman : XboxGame
    {
        private const int MoveOneSquareDelay = 225;
        private const int SetStartDelay = 10000;
        private const int BombDetonationDelay = 3000;
        private const int PlasmaBombDetonationDelay = 1500;
        private const int MenuNavigationDelay = 500;
        private const int BattleEndToMenuDelay = 8000;
        private const int MenuToBattleDelay = 10000;
        private const int NumberOfSets = 5;

        private Timer bombTimer;
        private bool bombPlaced;
        private List<Movement> lastMovements = new List<Movement>();
        private bool usePlasmaBombs;
        private int blocksDestroyedCount;
        private int bombPlacedCount;

        public SuperBomberman(bool usePlasmaBombs, CancellationToken cancellationToken) : base(cancellationToken)
        {
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
        /// Skulls : Off
        /// Special abilities : On
        /// Map: The Great Wall (map with the most immuable blocks)
        /// Players: 2 Player
        /// </summary>
        public override void Start()
        {
            DateTime startTime = DateTime.Now;
            while (true)
            {
                for (int i = 1; i <= NumberOfSets; i++)
                {
                    RaiseLog("Start set.");
                    PlaySet();
                    blocksDestroyedCount += 26;
                    if (i < NumberOfSets)
                    {
                        Thread.Sleep(SetStartDelay);
                    }
                }
                RaiseLog("Boosting started at: " + startTime.ToShortTimeString() + "." + Environment.NewLine + "Blocks destroyed: " + blocksDestroyedCount + ". " + Math.Round(blocksDestroyedCount / ((DateTime.Now - startTime).TotalSeconds / 60), 2) + " blocks/min.");
                NavigateMenuForNextBattle();
            }
        }

        private void Move(Direction direction, int nbSquares)
        {
            xboxController.Move(direction, nbSquares * MoveOneSquareDelay);
            lastMovements.Add(new Movement(direction, nbSquares));
        }

        private void RevertLastMovements()
        {
            for (int i = lastMovements.Count - 1; i >= 0; i--)
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
            RaiseLog("Battle end");
            Thread.Sleep(BattleEndToMenuDelay);

            RaiseLog("Dialog");
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(MenuNavigationDelay);

            RaiseLog("Stars summary");
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(MenuNavigationDelay);

            RaiseLog("Gem win 1");
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(MenuNavigationDelay);

            RaiseLog("Gem win 2");
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(MenuNavigationDelay);

            RaiseLog("Battle again");
            xboxController.PressButton(XboxController.Button.A);

            Thread.Sleep(MenuToBattleDelay);
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
            bombPlacedCount += 1;
            RaiseLog("Bomb " + bombPlacedCount);
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
            bombPlacedCount = 0;

            // Bomb 1
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1) }, //1,2
                new[] { new Movement(Direction.Left, 1), new Movement(Direction.Down, 1) }); //2,1

            // Bomb 2
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 2) }, //1,3
                new[] { new Movement(Direction.Left, 2), new Movement(Direction.Down, 1) }); //2,1

            // Bomb 3
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 3) }, //1,4
                new[] { new Movement(Direction.Left, 3), new Movement(Direction.Down, 1) }); //2,1

            // Bomb 4
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 2), new Movement(Direction.Down, 1) }, //2,3
                new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 1) }); //1,4

            // Bomb 5
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1) }, //1,5
                new[] { new Movement(Direction.Left, 2), new Movement(Direction.Down, 1) }); //2,3

            // Bomb 6
            PlaceNextBomb(new[] { new Movement(Direction.Down, 1) }, //3,3
                new[] { new Movement(Direction.Up, 2), new Movement(Direction.Left, 1) }); //1,2

            // Bomb 7
            PlaceNextBomb(new[] { new Movement(Direction.Left, 1), new Movement(Direction.Down, 1) }, //2,1
                new[] { new Movement(Direction.Up, 1), new Movement(Direction.Right, 1) }); //1,2

            // Bomb 8
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1), new Movement(Direction.Down, 3) }, //4,3
                new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 1) }); //3,2

            // Bomb 9
            PlaceNextBomb(new[] { new Movement(Direction.Left, 1) }, //3,1
                new[] { new Movement(Direction.Right, 2), new Movement(Direction.Down, 1) }); //4,3

            // Bomb 10
            PlaceNextBomb(new[] { new Movement(Direction.Down, 1) }, //5,3
                new[] { new Movement(Direction.Down, 1), new Movement(Direction.Right, 1) }); //6,4

            // Bomb 11
            PlaceNextBomb(new[] { new Movement(Direction.Left, 1), new Movement(Direction.Down, 1) }, //7,3
                new[] { new Movement(Direction.Right, 2), new Movement(Direction.Down, 1) }); //8,5

            // Bomb 12
            PlaceNextBomb(new[] { new Movement(Direction.Down, 1) }, //9,5
                new[] { new Movement(Direction.Up, 2), new Movement(Direction.Left, 1) }); //7,4

            // Bomb 13
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1), new Movement(Direction.Down, 2), new Movement(Direction.Left, 1) }, //9,5
                new[] { new Movement(Direction.Right, 1), new Movement(Direction.Down, 1) }); //10,5

            // Bomb 14
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 1) }); //9,4

            // Bomb 15
            PlaceNextBomb(new[] { new Movement(Direction.Left, 1) }, //9,3
                new[] { new Movement(Direction.Right, 2), new Movement(Direction.Down, 1) }); //10,5

            // Bomb 16
            PlaceNextBomb(new[] { new Movement(Direction.Down, 1) }, //11,5
                new[] { new Movement(Direction.Up, 2), new Movement(Direction.Left, 1) }); //9,4

            // Bomb 17
            PlaceNextBomb(new[] { new Movement(Direction.Left, 1), new Movement(Direction.Down, 1) }, //10,3
                new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 1) }); //9,2

            // Bomb 18
            PlaceNextBomb(new[] { new Movement(Direction.Right, 1), new Movement(Direction.Up, 1) }); //8,3

            // Bomb 19
            PlaceNextBomb(new[] { new Movement(Direction.Up, 1), new Movement(Direction.Left, 1) }, //7,2
                new[] { new Movement(Direction.Right, 1), new Movement(Direction.Up, 1) }); //6,3

            // Bomb 20
            PlaceNextBomb(new[] { new Movement(Direction.Down, 1), new Movement(Direction.Left, 2) }, //7,1
                new[] { new Movement(Direction.Right, 2), new Movement(Direction.Up, 1) }); //6,3

            // Bomb 21
            PlaceBomb();
            WaitForBombToExplode();
            // Self-descruct
        }

        private class Movement
        {
            public Direction Direction { get; }
            public int NbSquares { get; }

            public Movement(Direction direction, int nbSquares)
            {
                this.Direction = direction;
                this.NbSquares = nbSquares;
            }
        }
    }
}
