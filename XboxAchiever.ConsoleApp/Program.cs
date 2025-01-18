using System;
using XboxAchiever.Core;

namespace XboxStreamingIdleBoostingConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			WindowHelper.SetTopMost();
			Console.WindowWidth = 30;
			Console.WindowHeight = 5;

			Console.ReadLine();

			var xboxAchiever = new XboxAchiever.Core.XboxAchiever();
			xboxAchiever.Log += Console.WriteLine;
			xboxAchiever.Start(false);

			Console.ReadLine();
		}
	}
}
