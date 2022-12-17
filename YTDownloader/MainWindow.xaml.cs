using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace YTDownloader
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public async void UpdateStatus(object sender, RoutedEventArgs e)
		{
			while (true)
			{
				statusInfo.Text = Globals.STATUS;
				await Task.Run(() => Task.Delay(100));
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
			string saveLocation = null;
			CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
			folderDialog.IsFolderPicker = true;
			if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				saveLocation = folderDialog.FileName;
			}
		}

		private void downloadAudio_Click(object sender, RoutedEventArgs e)
		{
			string saveLocation = null;
			CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
			folderDialog.IsFolderPicker = true;
			if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				saveLocation = folderDialog.FileName;
			}
		}
	}
}
