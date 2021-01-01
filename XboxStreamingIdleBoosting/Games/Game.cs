using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XboxStreamingIdleBoosting.Games
{
    abstract class Game
    {
        public delegate void LogEventHandler(string message);
        public event LogEventHandler Log;

        public abstract void Start();

        protected abstract string GameWindowName { get; }
        protected XboxController xboxController;
        public IntPtr hwnd;

        protected CancellationToken cancellationToken;

        protected Game(CancellationToken cancellationToken)
        {
            // All games will use a controller to wrap keystrokes
            xboxController = new XboxController();
            this.cancellationToken = cancellationToken;
        }

        protected void RaiseLog(string message)
        {
            Log?.Invoke(message);
        }

        public bool FocusWindow(ref string errorMessage)
        {
            hwnd = WindowHelper.FocusWindow(GameWindowName, ref errorMessage);
            return hwnd != IntPtr.Zero;
        }

        public void ActivateInputLogging()
        {
            xboxController.InputSent += (x) => { RaiseLog(x); };
        }
    }
}
