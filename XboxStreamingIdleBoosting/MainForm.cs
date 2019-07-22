using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using XboxStreamingIdleBoosting.Games;

namespace XboxStreamingIdleBoosting
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

        enum ShowWindowCmd
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimize = 2,
            ShowMaximize = 3, // Or ActivatedMaximized
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        private XboxController xboxController;
        private Task task;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;

        public MainForm()
        {
            InitializeComponent();
            xboxController = new XboxController();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (controllerLogsCheckBox.Checked)
                xboxController.InputSent += (x) => { AddLog(x, false); };

            if (FocusXboxApp())
            {
                System.Threading.Thread.Sleep(500); // Let time for the application to focus and be ready to receive inputs
                SuperBomberman game = new SuperBomberman(xboxController, true);
                game.Log += (x) => { AddLog(x, false); };

                task = Task.Factory.StartNew(() => game.StartBlockBoosting(), cancellationToken);
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
            //cancellationToken
        }

        private Boolean FocusXboxApp()
        {
            IntPtr handle = FindWindowA(null, "Compagnon de la console Xbox");
            if (handle.ToInt32() == 0)
            {
                MessageBox.Show("The Xbox app is not started.");
                return false;
            }
            else
            {
                ShowWindow(handle, (int)ShowWindowCmd.ShowNormal);
                SetForegroundWindow(handle);
                return true;
            }
        }

        private void AddLog(string text, bool overwrite)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { AddLog(text, overwrite); }));
            }
            else
            {
                if (overwrite)
                {
                    logTextBox.Text = text;
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
        }
    }
}
