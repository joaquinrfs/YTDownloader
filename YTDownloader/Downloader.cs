using Microsoft.WindowsAPICodePack.Shell;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

		/// <summary>
		/// Interface for the downloader program.
		/// </summary>
		/// <param name="url">URL to the video.</param>
		/// <param name="path">Where to download the file.</param>
		/// <param name="type">Type of file to download: 0 for video, 1 for audio.</param>
		internal static Task<bool> Download(string url, string path, byte type)
		{
			string argFile = "";

			if (type == 0)
			{
				if (Globals.FFMPEG == true) { argFile = "-f bestvideo+bestaudio"; }
				else { argFile = "-f best"; }
			}
			else if (type == 1) { argFile = "-f bestaudio"; }

			Process ytDownloder = new Process();
			ytDownloder.StartInfo.UseShellExecute = false;
			ytDownloder.StartInfo.CreateNoWindow = true;
			if (Directory.Exists(path)) { ytDownloder.StartInfo.WorkingDirectory = path; }
			ytDownloder.StartInfo.FileName = "yt-dlp";
			ytDownloder.StartInfo.Arguments = $"{argFile} {url}";
			ytDownloder.Start();

			{
				bool exit = false;
				while (exit == false)
				{
					Thread.Sleep(500);
					if (ytDownloder.HasExited) { exit = true; }
				}
			}

			return Task.FromResult(true);
			//ytDownloder.Dispose();
		}
	}
}
