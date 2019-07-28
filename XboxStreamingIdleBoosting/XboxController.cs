using System;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace XboxStreamingIdleBoosting
{
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public class XboxController
    {
        public enum Button
        {
            A = 1,
            B = 2,
            X = 3,
            Y = 4
        }

        private const VirtualKeyCode AButtonKey = VirtualKeyCode.VK_J;
        private const VirtualKeyCode BButtonKey = VirtualKeyCode.VK_K;
        private const VirtualKeyCode XButtonKey = VirtualKeyCode.VK_U;
        private const VirtualKeyCode YButtonKey = VirtualKeyCode.VK_I;
        private const VirtualKeyCode UpButtonKey = VirtualKeyCode.VK_W;
        private const VirtualKeyCode DownButtonKey = VirtualKeyCode.VK_S;
        private const VirtualKeyCode LeftButtonKey = VirtualKeyCode.VK_A;
        private const VirtualKeyCode RightButtonKey = VirtualKeyCode.VK_D;
        private const int InputDelay = 50; // The xbox games cannot take inputs too quickly.

        private InputSimulator inputSimulator;

        public delegate void InputSentEventHandler(string input);
        public event InputSentEventHandler InputSent;

        public XboxController()
        {
            inputSimulator = new InputSimulator();
        }

        public void PressButton(Button button)
        {
            VirtualKeyCode keyCode;
            switch (button)
            {
                case Button.A:
                    keyCode = AButtonKey;
                    break;
                case Button.B:
                    keyCode = BButtonKey;
                    break;
                case Button.X:
                    keyCode = XButtonKey;
                    break;
                case Button.Y:
                    keyCode = YButtonKey;
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Sending a simple KeyPress seems to be too quick for the xbox, it is not registered.
            // A KeyDown/KeyUp is sent instead.
            Thread.Sleep(InputDelay);
            inputSimulator.Keyboard.KeyDown(keyCode);
            Thread.Sleep(InputDelay);
            inputSimulator.Keyboard.KeyUp(keyCode);

            InputSent?.Invoke(button.ToString());
        }

        public void Move(Direction direction, int time)
        {
            VirtualKeyCode keyCode;
            switch (direction)
            {
                case Direction.Up:
                    keyCode = UpButtonKey;
                    break;
                case Direction.Down:
                    keyCode = DownButtonKey;
                    break;
                case Direction.Left:
                    keyCode = LeftButtonKey;
                    break;
                case Direction.Right:
                    keyCode = RightButtonKey;
                    break;
                default:
                    throw new NotImplementedException();
            }

            Thread.Sleep(InputDelay);
            inputSimulator.Keyboard.KeyDown(keyCode);
            InputSent?.Invoke(string.Format("Move {0} for {1} ms", direction, time));
            Thread.Sleep(time);
            inputSimulator.Keyboard.KeyUp(keyCode);
        }
    }
}
