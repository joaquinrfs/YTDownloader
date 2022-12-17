using System.Threading.Tasks;
using System.Windows;
using YTDownloader.Resources;

namespace YTDownloader
{
	public partial class App : Application
	{
		protected override async void OnStartup(StartupEventArgs e)
		{
			// Language debugging
			//Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");
			//Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");


			MainWindow = new MainWindow();
			ProgressWindow installWindow = new ProgressWindow();
			installWindow.Title = Lang.TitleInstalling;

			await Task.Run(Python.Check);
			if (Globals.PYTHON == null) { MessageBox.Show(Lang.PythonNotFound, Lang.TitleWarning); }
			else
			{
				Task ffmpegTask = Task.Run(FFmpeg.Check);
				Task downCheckTask = Task.Run(Downloader.Check);
				await Task.WhenAll(ffmpegTask, downCheckTask);
				if (Globals.DOWNLOADER == false)
				{
					MessageBoxResult installDownloader = MessageBox.Show(Lang.DownloaderNotFound, Lang.TitleWarning, MessageBoxButton.YesNo);
					if (installDownloader == MessageBoxResult.Yes)
					{
						installWindow.Show();

						Task downInstallTask = Task.Run(Downloader.Install);
						downCheckTask = downInstallTask.ContinueWith(task => Downloader.Check());
						await Task.WhenAll(downCheckTask);
						installWindow.Close();
						if (Globals.DOWNLOADER == false) { MessageBox.Show(Lang.DownloaderFailed, Lang.TitleError); }
						else { MainWindow.ShowDialog(); }
					}
					else { MessageBox.Show(Lang.DownloaderDeny, Lang.TitleWarning); }
				}
				else { MainWindow.ShowDialog(); }
			}
			this.Shutdown();
		}
	}
}
