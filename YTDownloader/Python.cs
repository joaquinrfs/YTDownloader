using System;
using System.IO;

namespace YTDownloader
{
	internal static class Python
	{
		internal static void Check()
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
