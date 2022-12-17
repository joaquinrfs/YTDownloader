using System;
using System.IO;

namespace YTDownloader
{
	internal class FFmpeg
	{
		internal static void Check()
		{
			string fullPath = Environment.GetEnvironmentVariable("PATH");
			string[] envPath = fullPath.Split(';');
			foreach (string path in envPath)
			{
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					foreach (string file in files)
					{
						string fileName = file.Substring(path.Length);
						if (fileName.ToLower() == "ffmpeg.exe") { Globals.FFMPEG = true; }
					}
				}
			}
		}
		internal static void Install()
		{
			// TODO: Implement this!
			string downloadLink = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
		}
	}
}
