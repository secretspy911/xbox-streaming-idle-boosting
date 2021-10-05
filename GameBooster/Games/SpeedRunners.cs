using System;
using System.Threading;


namespace GameBoosterNS.Games
{
    /// <summary>
    /// The goal of this script is to max out exp but hitting 3 other idle humans players.
    /// The only items that must be activated are the freeze beam and the fire ball. All the other items make the characters move out of place.
    /// The map must be Abyss, it's the only map which have an item box on the first screen, removing the need to move the other players.
    /// The script must be started when the level has been started.
    /// </summary>
    class SpeedRunners : XboxGame
    {
        public SpeedRunners(CancellationToken cancellationToken) : base(cancellationToken) { }

        public override void Start()
        {
            while (true)
            {
                MoveFromSecondPlace(); // Can work for second and the first bit of the third place (the character slides down the slope a little bit). So it has the best chance to be effective.
                Thread.Sleep(500); // Wait for character to completly stop
                xboxController.Move(Direction.Right, 50);
                Throw100Items();
                RestartGame();
            }
        }

        private void RestartGame()
        {
            xboxController.Move(Direction.Right, 30000); // Win the game
            Thread.Sleep(15000); // Animation time

            // Level select
            xboxController.PressButton(XboxController.Button.A);
            Thread.Sleep(55000); // Wait for the 1min timer
        }

        private void MoveInMenu(Direction direction)
        {
            xboxController.Move(direction, 200);
        }

        private void MoveFromFirstPlace()
        {
            xboxController.Move(Direction.Left, 650);
        }

        private void MoveFromSecondPlace()
        {
            xboxController.Move(Direction.Left, 525);
        }

        private void MoveFromThirdPlace()
        {
            xboxController.Move(Direction.Left, 450);
        }

        private void MoveFromFourthPlace()
        {
            xboxController.Move(Direction.Left, 400);
        }

        private void Throw100Items()
        {
            int count = 1;
            while (count <= 100) // 50 hits with each item. More does not game any exp.
            {
                xboxController.PressButton(XboxController.Button.B);
                Thread.Sleep(5000);

                count++;
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}