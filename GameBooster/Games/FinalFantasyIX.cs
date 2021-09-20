using System.Media;
using System.Threading;
using System;
using System.Drawing;

namespace GameBoosterNS.Games
{
    class FinalFantasyIX : PCGame
    {
        //private SoundPlayer soundPlayer;
        private Timer timer;
        private int jumpCount;
        private bool bubbleRead;

        protected override string GameWindowName => "FINAL FANTASY IX";

        public FinalFantasyIX(CancellationToken cancellationToken) : base(cancellationToken)
        {
            xboxController.AButtonKey = WindowsInput.Native.VirtualKeyCode.VK_X; // The game uses fixed buttons

            // Plays a sound each time there is a jump so it is easier to detect the timing
            //soundPlayer = new SoundPlayer(@"C:\Users\michaelr\Downloads\E-Mu-Proteus-FX-Wacky-Ride-Cymbal.wav");
            //soundPlayer.Load();
        }

        public override void Start()
        {
            Jump(); // 1
            Jump(700); // 2
            Jump(700); // 3
            Jump(550); // 4
            Jump(550); // 5
            Jump(500); // 6
            Jump(500); // 7
            Jump(500); // 8
            Jump(500); // 9
            Jump(500); // 10
            Jump(500); // 11
            Jump(500); // 12
            Jump(500); // 13
            Jump(550); // 14
            Jump(550); // 15
            Jump(550); // 16
            Jump(550); // 17
            Jump(550); // 18
            Jump(550); // 19
            Jump(550); // 20
            Jump(450); // 21
            Jump(450); // 22
            Jump(400); // 23
            Jump(400); // 24
            Jump(400); // 25
            Jump(400); // 26
            Jump(400); // 27
            Jump(400); // 28
            Jump(400); // 29
            Jump(400); // 30
            Jump(400); // 31
            Jump(400); // 32
            Jump(400); // 33
            Jump(400); // 34
            Jump(400); // 35
            Jump(400); // 36
            Jump(400); // 37
            Jump(350); // 38
            Jump(350); // 39
            Jump(350); // 40
            Jump(350); // 41
            Jump(350); // 42
            Jump(350); // 43
            Jump(350); // 44
            Jump(350); // 45
            Jump(400); // 46
            Jump(400); // 47
            Jump(400); // 48
            Jump(400); // 49
            Jump(400); // 50
            Jump(300); // 51
            Jump(300); // 52
            Jump(300); // 53
            Jump(300); // 54
            Jump(300); // 55
            Jump(350); // 56
            Jump(350); // 57
            Jump(350); // 58
            Jump(350); // 59
            Jump(350); // 60
        }

        private void StartWithScreenReading()
        {
            // Can't make it work...
            timer = new Timer((x) => JumpIfReady(), null, 0, 50);
            while (jumpCount <= 1000) { }
        }

        private void Jump()
        {
            xboxController.PressButton(XboxController.Button.A);
            //soundPlayer.Play(); Commented in case it causes a lag and desyncs the jumping
            jumpCount++;
            RaiseLog(this.jumpCount.ToString());
        }

        private void Jump(int delay)
        {      
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (delay > 0)
                    Thread.Sleep(delay);

                Jump();
            }
            catch (OperationCanceledException)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private void JumpIfReady()
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (DetectDialogBubble())
                    Jump();
            }
            catch (OperationCanceledException)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }            
        }

        private bool DetectDialogBubble()
        {
            Color color = WindowHelper.GetColorAt(IntPtr.Zero, 1319, 496); // Specific placemen of the game windows
            if (IsColorEqual(color, 243, 227, 219) || IsColorEqual(color, 154, 139, 132)) // Bubble could be 2 colors?
            {
                RaiseLog("Right color, bubble read = " + this.bubbleRead);
                if (!this.bubbleRead)
                {
                    this.bubbleRead = true;
                    return true;
                }
            }
            else
            {
                RaiseLog(string.Format("Wrong color ({0},{1},{2})", color.R, color.G, color.B));
                this.bubbleRead = false;
            }

            return false;
        }

        private bool IsColorEqual(Color color, int r, int g, int b)
        {
            return color.R == r && color.G == g && color.B == b;
        }
    }
}
