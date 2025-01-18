using System;
using System.Windows.Forms;

namespace XboxStreamingIdleBoosting
{
	public partial class MainForm : Form
	{
		private XboxAchiever.Core.XboxAchiever xboxAchiever;

		public MainForm()
		{
			InitializeComponent();
			this.TopMost = true;
			this.xboxAchiever = new XboxAchiever.Core.XboxAchiever();
			this.xboxAchiever.Log += AddLog;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			this.xboxAchiever.KeyLogger?.Terminate();
			base.OnFormClosing(e);
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			xboxAchiever.Start(controllerLogsCheckBox.Checked);
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
			this.xboxAchiever.Stop();
		}

		private void resetKeyLoggerButton_Click(object sender, EventArgs e)
		{
			this.xboxAchiever.KeyLogger?.Reset();
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
				this.xboxAchiever.KeyLogger.Initialize();
			}
			else
			{
				this.xboxAchiever.KeyLogger.Reset();
				this.xboxAchiever.KeyLogger.Terminate();
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
