using System;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace XboxAchiever.Core
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
			Y = 4,
			Start = 5
		}

		public VirtualKeyCode AButtonKey = VirtualKeyCode.VK_F;
		public VirtualKeyCode BButtonKey = VirtualKeyCode.VK_G;
		public VirtualKeyCode XButtonKey = VirtualKeyCode.VK_R;
		public VirtualKeyCode YButtonKey = VirtualKeyCode.VK_T;
		public VirtualKeyCode StartButtonKey = VirtualKeyCode.VK_B;
		public VirtualKeyCode UpButtonKey = VirtualKeyCode.VK_W;
		public VirtualKeyCode DownButtonKey = VirtualKeyCode.VK_S;
		public VirtualKeyCode LeftButtonKey = VirtualKeyCode.VK_A;
		public VirtualKeyCode RightButtonKey = VirtualKeyCode.VK_D;
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
				case Button.Start:
					keyCode = StartButtonKey;
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

		public void Move(Direction direction)
		{
			Move(direction, 0);
		}

		public void Move(Direction direction, int time)
		{
			VirtualKeyCode keyCode = GetKeyCode(direction);

			Thread.Sleep(InputDelay);
			inputSimulator.Keyboard.KeyDown(keyCode);

			InputSent?.Invoke(string.Format("Move {0} for {1} ms", direction, time));
			if (time > 0)
				Thread.Sleep(time);

			inputSimulator.Keyboard.KeyUp(keyCode);
		}

		public void MoveForever(Direction direction)
		{
			VirtualKeyCode keyCode = GetKeyCode(direction);
			inputSimulator.Keyboard.KeyDown(keyCode);
		}

		private VirtualKeyCode GetKeyCode(Direction direction)
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

			return keyCode;
		}
	}
}
