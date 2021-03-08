using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using XboxStreamingIdleBoosting.Games;

namespace XboxStreamingIdleBoosting
{
    public partial class MainForm : Form
    {
        enum ScriptType
        {
            SuperBomberManRDestroyBlocks = 0,
            FinalFantasyIXRopeJumping = 1,
            PhantasyStarOnline2 = 2
        }

        private Task task;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private Stopwatch stopWatch;

        // TODO Make settings configurable by user
        private const ScriptType scriptType = ScriptType.PhantasyStarOnline2;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (keyLoggerCheckBox.Checked)
                TerminateKeyLogger();

            base.OnFormClosing(e);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            Game game = null;
            bool logInputs = controllerLogsCheckBox.Checked;
            switch (scriptType)
            {
                case ScriptType.SuperBomberManRDestroyBlocks:
                    game = new SuperBomberman(true, cancellationToken);
                    break;
                case ScriptType.FinalFantasyIXRopeJumping:
                    game = new FinalFantasyIX(cancellationToken);                    
                    break;
                case ScriptType.PhantasyStarOnline2:
                    game = new PhantasyStarOnline2(cancellationToken);
                    break;
            }

            if (game != null)
            {
                if (logInputs)
                    game.ActivateInputLogging();

                game.Log += (x) => { AddLog(x); };

                string errorMessage = null;
                if (game.FocusWindow(ref errorMessage))
                {
                    Thread.Sleep(1000); // Let time for the application to focus and be ready to receive inputs

                    task = Task.Factory.StartNew(() => game.Start(), cancellationToken);
                    task.ContinueWith((x) =>
                    {
                        if (task.IsCanceled)
                            AddLog("Script canceled.");
                        else
                            AddLog("Script complete.");
                    });
                }
                else
                {
                    AddLog(errorMessage);
                }
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void resetKeyLoggerButton_Click(object sender, EventArgs e)
        {
            ResetKeyLogger();
        }

        private void copyToClipboardLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(logTextBox.Text);
        }

        private void keyLoggerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            resetKeyLoggerButton.Visible = keyLoggerCheckBox.Checked;

            if (keyLoggerCheckBox.Checked)
            {
                InitializeKeyLogger();
            }
            else
            {
                ResetKeyLogger();
                TerminateKeyLogger();
            }
        }

        private void AddLog(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { AddLog(text); }));
            }
            else
            {
                if (!string.IsNullOrEmpty(logTextBox.Text))
                {
                    logTextBox.AppendText(Environment.NewLine);
                }
                logTextBox.AppendText(text);
            }
        }

        private void InitializeKeyLogger()
        {
            WindowHelper.SetHook((x) =>
            {
                //if (!string.IsNullOrEmpty(logTextBox.Text))
                //{
                //    logTextBox.Text += Environment.NewLine;
                //}

                if (stopWatch == null)
                {
                    stopWatch = Stopwatch.StartNew();
                }
                else
                {
                    long elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
                    //AddLog("Jump(" + elapsedMilliseconds + "); // " + totalKeyCount);

                    stopWatch.Restart();
                }

                //AddLog(totalKeyCount + " - " + x.ToString());
            });
        }

        private void TerminateKeyLogger()
        {
            WindowHelper.UnhookWindowsHookEx(); // Does nothing
        }

        private void ResetKeyLogger()
        {
            if (stopWatch != null)
                stopWatch.Stop();
            stopWatch = null;
            //averageKeyCount = 0;
            //averageTotalTime = 0;
            //totalKeyCount = 0;
            logTextBox.Clear();
        }

        private void ClearLogsbutton_Click(object sender, EventArgs e)
        {
            logTextBox.Text = string.Empty;
        }
    }
}
