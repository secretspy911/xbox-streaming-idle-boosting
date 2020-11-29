using System.Media;
using System.Threading;
using System;
using System.Drawing;

namespace XboxStreamingIdleBoosting.Games
{
    class FinalFantasyIX : PCGame
    {
        private SoundPlayer soundPlayer;
        private Timer timer;
        private int jumpCount;
        private bool bubbleRead;

        protected override string GameWindowName => "FINAL FANTASY IX";

        public FinalFantasyIX(bool logInputs, CancellationToken cancellationToken) : base(logInputs, cancellationToken)
        {
            xboxController.AButtonKey = WindowsInput.Native.VirtualKeyCode.VK_X;
            soundPlayer = new SoundPlayer(@"C:\Users\michaelr\Downloads\E-Mu-Proteus-FX-Wacky-Ride-Cymbal.wav");
            soundPlayer.Load();
        }

        public override void Start()
        {
            // Can't make it work...
            timer = new Timer((x) => JumpIfReady(), null, 0, 50);
            while (jumpCount <= 1000) { }
        }

        private void Jump()
        {
            xboxController.PressButton(XboxController.Button.A);
            soundPlayer.Play();
            jumpCount++;
        }

        private void Jump(int delay)
        {
            delay -= 90;
            if (delay > 0)
                Thread.Sleep(delay);

            Jump();
        }

        private void JumpIfReady()
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                Color color = WindowHelper.GetColorAt(IntPtr.Zero, 1319, 496);
                if (IsColorEqual(color, 243, 227, 219) || IsColorEqual(color, 154, 139, 132))
                {
                    RaiseLog("Right color, bubble read = " + bubbleRead);
                    if (!bubbleRead)
                    {
                        bubbleRead = true;
                        Jump();
                    }
                }
                else
                {
                    RaiseLog(string.Format("Wrong color ({0},{1},{2})", color.R ,color.G ,color.B));
                    bubbleRead = false;
                }
            }
            catch (OperationCanceledException)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }            
        }

        private bool IsColorEqual(Color color, int r, int g, int b)
        {
            return color.R == r && color.G == g && color.B == b;
        }
    }
}
