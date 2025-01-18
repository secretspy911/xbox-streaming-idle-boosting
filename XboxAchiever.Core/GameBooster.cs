using System.Threading;
using System.Threading.Tasks;
using XboxAchiever.Core.Games;

namespace XboxAchiever.Core
{
	public class GameBooster
	{
		enum ScriptType
		{
			SuperBomberManRDestroyBlocks = 0,
			FinalFantasyIXLevel99 = 1,
			PhantasyStarOnline2 = 2,
			SpeedRunners = 3
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler Log;

		private Task task;
		private CancellationTokenSource cancellationTokenSource;
		private CancellationToken cancellationToken;

		// TODO Make settings configurable by user
		private const ScriptType scriptType = ScriptType.FinalFantasyIXLevel99;

		public bool UseKeyLogger { get; set; }
		public KeyLogger KeyLogger;

		public void Start(bool logInputs)
		{
			if (UseKeyLogger)
			{
				KeyLogger = new KeyLogger();
				KeyLogger.Initialize();
			}

			cancellationTokenSource = new CancellationTokenSource();
			cancellationToken = cancellationTokenSource.Token;
			Game game = null;

			switch (scriptType)
			{
				case ScriptType.SuperBomberManRDestroyBlocks:
					game = new SuperBomberman(true, cancellationToken);
					break;
				case ScriptType.FinalFantasyIXLevel99:
					game = new FinalFantasyIX(cancellationToken);
					break;
				case ScriptType.PhantasyStarOnline2:
					game = new PhantasyStarOnline2(cancellationToken);
					break;
				case ScriptType.SpeedRunners:
					game = new SpeedRunners(cancellationToken);
					break;
			}

			if (game != null)
			{
				if (logInputs)
					game.ActivateInputLogging();

				game.Log += (x) => { RaiseLog(x); };

				string errorMessage = null;
				if (game.FocusWindow(ref errorMessage))
				{
					Thread.Sleep(1000); // Let time for the application to focus and be ready to receive inputs

					task = Task.Factory.StartNew(() => game.Start(), cancellationToken);
					task.ContinueWith((x) =>
					{
						if (task.IsCanceled)
							RaiseLog("Script canceled.");
						else
							RaiseLog("Script complete.");
					});
				}
				else
				{
					RaiseLog(errorMessage);
				}
			}
		}

		public void Stop()
		{
			cancellationTokenSource.Cancel();
		}

		private void RaiseLog(string message)
		{
			Log?.Invoke(message);
		}
	}
}
