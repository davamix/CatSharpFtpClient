using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatSharpFtpClient.FtpCommands;
using CatSharpFtpClient;

namespace TestFtpClient
{
	public class GetCommandTest : TestBase
	{
		public GetCommandTest()
			:base()
		{}

		public void DownloadOneFile()
		{
			string FileName_ = "test.txt";
			string PathSave_ = @"C:\Users\davamix\Downloads\ToUploadFTP";

			GetAsync Command_ = new GetAsync(base.Client.FtpConfiguration, FileName_, PathSave_);
			Command_.FileDownloading += OnDownloading;
			Command_.FileDownloaded += OnDownloaded;
			Command_.FileDownloadCompleted += OnDownloadedCompleted;
			Command_.ErrorOccurred += base.OnErrorOccurred;

			base.Client.AddCommand(Command_);
			base.Client.Run();
		}

		public void DownloadManyFiles()
		{
			var FileNames_ = new List<string>();
			string PathSave_ = @"C:\Users\davamix\Downloads\ToUploadFTP";
			
			FileNames_.Add("test.txt");
			FileNames_.Add("testFtpUpload.jpeg");
			FileNames_.Add("FileZilla_Server-0_9_41.exe");

			//Configure command to download binary files.
			var CustomConfiguration_ = new CSFtpClientConfiguration(
									base.Client.FtpConfiguration.Server,
									base.Client.FtpConfiguration.User,
									base.Client.FtpConfiguration.Password)
									{
										UseBinary = true
									};

			GetAsync Command_ = new GetAsync(CustomConfiguration_, FileNames_, PathSave_);
			Command_.FileDownloading += OnDownloading;
			Command_.FileDownloaded += OnDownloaded;
			Command_.FileDownloadCompleted += OnDownloadedCompleted;
			Command_.ErrorOccurred += base.OnErrorOccurred;

			base.Client.AddCommand(Command_);
			base.Client.Run();
		}

		#region Events section

		private void OnDownloading(object sender, FileInfo _File_)
		{
			Console.WriteLine(string.Format("Dowloading => {0}", _File_.Name));
		}
		private void OnDownloaded(object sender, FileInfo _File_)
		{
			Console.WriteLine(String.Format("Downloaded on => {0}", _File_.FullName));
		}
		private void OnDownloadedCompleted(object sender)
		{
			Console.WriteLine("Command processed.");
		}

		#endregion
	}
}
