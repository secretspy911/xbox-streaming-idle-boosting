using System.Diagnostics;

namespace GameBoosterNS
{
    public class KeyLogger
    {
        private Stopwatch stopWatch;

        public void Initialize()
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

        public void Terminate()
        {
            WindowHelper.UnhookWindowsHookEx(); // Does nothing
        }

        public void Reset()
        {
            if (stopWatch != null)
                stopWatch.Stop();
            stopWatch = null;
            //averageKeyCount = 0;
            //averageTotalTime = 0;
            //totalKeyCount = 0;
        }
    }
}
