using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using YTDownloader.Resources;

namespace YTDownloader
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Python.Check();
			if (Globals.PYTHON == null)
			{
				MessageBox.Show(Lang.PythonNotFound, Lang.TitleWarning);
				this.Close();
			}

			Downloader.Check();
			if (Globals.DOWNLOADER == false)
			{
				MessageBoxResult installDownloader = MessageBox.Show(Lang.DownloaderNotFound, Lang.TitleWarning, MessageBoxButton.YesNo);
				if (installDownloader == MessageBoxResult.Yes)
				{
					Downloader.Install();
					Downloader.Check();
					if (Globals.DOWNLOADER == false)
					{
						MessageBox.Show(Lang.DownloaderFailed, Lang.TitleError);
						this.Close();
					}
				}
				else
				{
					MessageBox.Show(Lang.DownloaderDeny, Lang.TitleWarning);
					this.Close();
				}
			}
		}

		private void UpdateStatus()
		{
			while (true)
			{
				statusInfo.Text = Globals.STATUS;
				Thread.Sleep(1000);
			}
		}

		private void TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			string regEx = @"^(https://)?(www\.)?youtu(be\.com/watch\?v=|\.be/)[a-zA-Z0-9]{11}(&[a-zA-Z0-9\-_]+=[a-zA-Z0-9]+)*$";
			Match regMatch = Regex.Match(urlBox.Text, regEx);
			if (urlBox.Text.Length > 0 && regMatch.Success)
			{
				downloadVideo.IsEnabled = true;
				downloadAudio.IsEnabled = true;
			}
			else
			{
				downloadVideo.IsEnabled = false;
				downloadAudio.IsEnabled = false;
			}
		}

		private void downloadVideo_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
			folderDialog.IsFolderPicker = true;
			folderDialog.ShowDialog();
			string saveLocation = folderDialog.FileName;
		}

		private void downloadAudio_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
			folderDialog.IsFolderPicker = true;
			folderDialog.ShowDialog();
		}
	}
}
