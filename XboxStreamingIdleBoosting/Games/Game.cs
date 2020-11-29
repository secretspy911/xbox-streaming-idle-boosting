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
        protected bool logInputs;
        protected XboxController xboxController;
        protected CancellationToken cancellationToken;
        public IntPtr hwnd;

        protected Game(bool logInputs, CancellationToken cancellationToken)
        {
            this.logInputs = logInputs;
            this.cancellationToken = cancellationToken;

            // All games will use a controller to wrap keystrokes
            xboxController = new XboxController();
            if (logInputs)
                xboxController.InputSent += (x) => { RaiseLog(x); };
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
    }
}
