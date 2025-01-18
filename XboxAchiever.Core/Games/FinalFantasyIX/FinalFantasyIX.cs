using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace XboxAchiever.Core.Games
{
	class FinalFantasyIX : PCGame
	{
		protected override string GameWindowName => "FINAL FANTASY IX";

		public FinalFantasyIX(CancellationToken cancellationToken) : base(cancellationToken) { }

		public override void Start()
		{
			xboxController.AButtonKey = VirtualKeyCode.VK_X;

			Task.Run(() =>
			{
				while (!cancellationToken.IsCancellationRequested)
					xboxController.PressButton(XboxController.Button.A);
			}, cancellationToken);

			while (!cancellationToken.IsCancellationRequested)
			{
				xboxController.Move(Direction.Left, 1000);
			}
		}
	}
}
