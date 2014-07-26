using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatSharpFtpClient.FtpCommands;

namespace TestFtpClient
{
	public class PutCommandTest : TestBase
	{

		public PutCommandTest()
			:base()
		{}
		
		/// <summary>
		/// TEST: Upload a simple file
		/// </summary>
		/// <param name="_Client_"></param>
		public void UploadOneFile()
		{
			var File_ = new FileInfo(@"C:\Users\davamix\Downloads\ToUploadFTP\testFtpUpload.jpeg");

			PutAsync Command_ = new PutAsync(base.Client.FtpConfiguration, File_);
			Command_.FileUploading += OnFileUploading;
			Command_.FileUploaded += OnFileUploaded;
			Command_.FileUploadCompleted += OnFileUploadCompleted;
		}

		/// <summary>
		/// TEST: Create two commands, one with multiple files.
		/// </summary>
		/// <param name="_Client_"></param>
		public void UploadManyFiles()
		{
			var Files_ = new List<FileInfo>();
			var File2_ = new FileInfo(@"C:\Users\davamix\Downloads\mseinstall.exe");

			Directory.GetFiles(@"C:\Users\davamix\Downloads\ToUploadFTP")
						.ToList()
						.ForEach(f => Files_.Add(new FileInfo(f)));

			//Command 1
			PutAsync Command_ = new PutAsync(base.Client.FtpConfiguration, Files_);
			Command_.FileUploading += OnFileUploading;
			Command_.FileUploaded += OnFileUploaded;
			Command_.FileUploadCompleted += OnFileUploadCompleted;
			Command_.ErrorOccurred += OnErrorOccurred;

			//Command 2
			PutAsync Command2_ = new PutAsync(base.Client.FtpConfiguration, File2_);
			Command2_.FileUploading += OnFileUploading;
			Command2_.FileUploaded += OnFileUploaded;
			Command2_.FileUploadCompleted += OnFileUploadCompleted;
			Command2_.ErrorOccurred += OnErrorOccurred;

			base.Client.AddCommand(Command_);
			base.Client.AddCommand(Command2_);

			base.Client.Run();
		}

		private void OnFileUploading(object sender, FileInfo _File_)
		{
			Console.WriteLine(string.Format("Uploading => {0}", _File_.FullName));
		}
		private void OnFileUploaded(object sender, FileInfo _File_)
		{
			Console.WriteLine(string.Format("Uploaded: {0}", _File_.FullName));
		}
		private void OnFileUploadCompleted(object sender)
		{
			Console.WriteLine("Command processed.");
		}
	}
}
