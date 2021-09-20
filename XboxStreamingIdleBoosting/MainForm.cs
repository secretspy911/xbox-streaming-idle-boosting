using System;
using System.Windows.Forms;
using System.Diagnostics;
using GameBoosterNS;

namespace XboxStreamingIdleBoosting
{
    public partial class MainForm : Form
    {
        private GameBooster gamebooster;

        public MainForm()
        {
            InitializeComponent();
            this.TopMost = true;
            this.gamebooster = new GameBooster();
            this.gamebooster.Log += AddLog;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.gamebooster.KeyLogger?.Terminate();
            base.OnFormClosing(e);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            gamebooster.Start(controllerLogsCheckBox.Checked);           
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
            this.gamebooster.Stop();
        }

        private void resetKeyLoggerButton_Click(object sender, EventArgs e)
        {
            this.gamebooster.KeyLogger?.Reset();
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
                this.gamebooster.KeyLogger.Initialize();
            }
            else
            {
                this.gamebooster.KeyLogger.Reset();
                this.gamebooster.KeyLogger.Terminate();
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

        private void ClearLogsbutton_Click(object sender, EventArgs e)
        {
            logTextBox.Text = string.Empty;
        }
    }
}
