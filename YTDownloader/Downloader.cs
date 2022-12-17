using System.Diagnostics;
using System.IO;
using System.Threading;

namespace YTDownloader
{
	// The class has an ambiguous name because I could be using
	// youtube-dl or yt-dlp and it's not like I'm going to change
	// the whole class name just to reflect the tool I'm using.
	internal static class Downloader
	{
		internal static void Check()
		{
			string[] files = Directory.GetFiles(Globals.PYTHON + "Scripts\\");
			foreach (string file in files)
			{
				string fileName = file.Substring((Globals.PYTHON + "Scripts\\").Length);
				if (fileName.ToLower() == "yt-dlp.exe") { Globals.DOWNLOADER = true; }
			}
		}
		internal static void Install()
		{
			Process pythonPip = new Process();
			pythonPip.StartInfo.UseShellExecute = false;
			pythonPip.StartInfo.CreateNoWindow = true;
			pythonPip.StartInfo.FileName = "pip";
			pythonPip.StartInfo.Arguments = "install yt-dlp";
			pythonPip.Start();

			{
				bool exit = false;
				byte timer = 0;
				while (exit == false)
				{
					Thread.Sleep(500);
					if (timer >= 60 || pythonPip.HasExited) { exit = true; }
					timer++;
				}
			}

			pythonPip.Dispose();
		}
		internal static void Download()
		{
			Process ytDownloder = new Process();
			ytDownloder.StartInfo.UseShellExecute = false;
			ytDownloder.StartInfo.CreateNoWindow = true;
			ytDownloder.StartInfo.FileName = "yt-dlp";
			ytDownloder.StartInfo.Arguments = "-f bestvideo+";
			ytDownloder.Start();
		}
	}
}
