using System.Threading;

namespace XboxStreamingIdleBoosting.Games
{
    class PhantasyStarOnline2 : XboxGame
    {
        public PhantasyStarOnline2(CancellationToken cancellationToken) : base(cancellationToken) { }

        public override void Start()
        {
            StartStoryDialogs();
        }

        private void StartDartGame()
        {
            while (true)
            {
                xboxController.PressButton(XboxController.Button.B);
                Thread.Sleep(1000);
                xboxController.PressButton(XboxController.Button.A);
                Thread.Sleep(7000);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        private void StartStoryDialogs()
        {
            while (true)
            {
                xboxController.PressButton(XboxController.Button.A);
                Thread.Sleep(10000);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
