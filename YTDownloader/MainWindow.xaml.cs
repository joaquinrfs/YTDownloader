using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using YTDownloader.Resources;

namespace YTDownloader
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Loaded += UpdateStatus;
		}

		public async void UpdateStatus(object sender, RoutedEventArgs e)
		{
			Globals.STATUS = Lang.StatusLoaded;
			while (true)
			{
				statusInfo.Text = Globals.STATUS;
				await Task.Delay(200);
			}
			
		}

		private void TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			string regEx = @"^(https://)?(www\.)?youtu(be\.com/watch\?v=|\.be/)[a-zA-Z0-9\-_]{11}(&[a-zA-Z0-9\-_]+=[a-zA-Z0-9]+)*$";
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

		private async void downloadVideo_Click(object sender, RoutedEventArgs e)
		{
			ProgressWindow progressWindow = new ProgressWindow();
			progressWindow.Title = Lang.TitleDownloading;

			CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
			folderDialog.IsFolderPicker = true;
			if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				progressWindow.Show();
				string taskUrl = urlBox.Text, taskPath = folderDialog.FileName;
				Task<bool> t = Task.Run(() => Downloader.Download(taskUrl, taskPath, 0));
				await Task.WhenAll(t);
				if (t.Result == true) { Globals.STATUS = Lang.StatusDownloadSuccess; }
				progressWindow.Close();
			}
		}

		private async void downloadAudio_Click(object sender, RoutedEventArgs e)
		{
			ProgressWindow progressWindow = new ProgressWindow();
			progressWindow.Title = Lang.TitleDownloading;

			CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
			folderDialog.IsFolderPicker = true;
			if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				progressWindow.Show();
				string taskUrl = urlBox.Text, taskPath = folderDialog.FileName;
				Task<bool> t = Task.Run(() => Downloader.Download(taskUrl, taskPath, 1));
				await Task.WhenAll(t);
				if (t.Result == true) { Globals.STATUS = Lang.StatusDownloadSuccess; }
				progressWindow.Close();
			}
		}
	}
}
