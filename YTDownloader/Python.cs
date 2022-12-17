using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace YTDownloader
{
	internal static class Python
	{
		internal static void Check()
		{
			// Check for Python's responseProcess{
			Process pythonVersion = new Process();
			pythonVersion.StartInfo.UseShellExecute = false;
			pythonVersion.StartInfo.CreateNoWindow = true;
			pythonVersion.StartInfo.RedirectStandardOutput = true;
			pythonVersion.StartInfo.FileName = "python";
			pythonVersion.StartInfo.Arguments = "-c \"import sys; print(sys.executable)\"";
			pythonVersion.Start();
			
			{
				bool exit = false;
				byte timer = 0;
				while (exit == false)
				{
					Thread.Sleep(200);
					if (timer >= 60 || pythonVersion.HasExited) { exit = true; }
					timer++;
				}
			}
			
			using (StreamReader reader = pythonVersion.StandardOutput)
			{
				string path = reader.ReadLine();
				FileInfo file= new FileInfo(path);
				if (file.Name.ToLower() == "python.exe") { Globals.PYTHON = file.DirectoryName.ToLower() + "\\"; }
			}
		}
		internal static void CheckLegacy()
		{
			// Checks for "Python in the $PATH"
			string fullPath = Environment.GetEnvironmentVariable("PATH");
			string[] envPath = fullPath.Split(';');
			foreach (string path in envPath)
			{
				// Search for "python.exe" in any folder with "Python" in the $PATH
				if (path.ToLower().Contains("python") && Globals.PYTHON == null)
				{
					if (Directory.Exists(path))
					{
						string[] files = Directory.GetFiles(path);
						foreach (string file in files)
						{
							string fileName = file.Substring(path.Length);
							if (fileName.ToLower() == "python.exe")
							{
								// Append '\' if trailing (back)slash not present
								string lastDigit = path[path.Length - 1].ToString();
								string append = "";
								if (lastDigit != "\\" && lastDigit != "/") { append = "\\"; }

								Globals.PYTHON = path + append;
							}
						}
					}
				}
			}
		}
	}
}
