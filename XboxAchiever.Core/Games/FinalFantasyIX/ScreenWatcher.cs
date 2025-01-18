using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace XboxAchiever.Core
{
	internal class ScreenWatcher
	{
		//private Image<Bgr, byte> imageToSearch;
		private Rectangle regionOfInterest = new Rectangle(750, 500, 75, 50);

		public event EventHandler ImageDetected;

		public ScreenWatcher(string imagePath)
		{
			InitImage(imagePath);
		}

		public void Start()
		{
			using (VideoCapture capture = new VideoCapture())
			{
				while (true)
				{
					// Capture a frame from the screen
					Mat frame = new Mat();
					capture.Read(frame);

					// Check if the frame is valid
					if (!frame.IsEmpty)
					{
						// Convert the frame to an Emgu.CV image
						Image<Bgr, byte> screen = frame.ToImage<Bgr, byte>();

						screen.ROI = regionOfInterest;

						if (IsBubbleVisible(screen, null))
						{
							ImageDetected?.Invoke(null, null);
						}

						frame.Dispose();

						System.Threading.Thread.Sleep(1000);
					}
				}
			}
		}

		private static bool IsBubbleVisible(Image<Bgr, byte> screen, Image<Bgr, byte> imageToSearch)
		{
			// Perform template matching using OpenCV
			using (Image<Gray, float> result = screen.MatchTemplate(imageToSearch, TemplateMatchingType.CcoeffNormed))
			{
				double[] minValues, maxValues;
				Point[] minLocations, maxLocations;
				result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

				// Set a threshold for match detection (adjust as needed)
				double threshold = 0.8;

				if (maxValues[0] >= threshold)
				{
					return true;
				}
			}

			return false;
		}

		private void InitImage(string imagePath)
		{
			Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(imagePath);

			using (var memoryStream = new MemoryStream())
			{
				resourceStream.CopyTo(memoryStream);
				byte[] imageBytes = memoryStream.ToArray();
				Bitmap bitmap = new Bitmap(resourceStream);

				Image<Bgr, byte> emguImage = new Image<Bgr, byte>(bitmap.Width, bitmap.Height);
				emguImage.Bytes = imageBytes;
			}
		}
	}
}