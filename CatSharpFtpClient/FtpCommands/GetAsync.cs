using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CatSharpFtpClient.FtpCommands
{
	public class GetAsync : CommandBase
	{
		public event FileDownloadingEventHandler FileDownloading;
		public event FileDownloadedEventHandler FileDownloaded;
		public event FileDownloadCompletedEventHandler FileDownloadCompleted;
		public delegate void FileDownloadingEventHandler(object sender, FileInfo _File_);
		public delegate void FileDownloadedEventHandler(object sender, FileInfo _FilePath_);
		public delegate void FileDownloadCompletedEventHandler(object sender);

		//private FileInfo File { get; set; }

		private List<FileInfo> Files { get; set; }

		/// <summary>
		/// Get a GetAsync command
		/// </summary>
		/// <param name="_Configuration_">Configuration for command</param>
		/// <param name="_FileName_">File name in server</param>
		/// <param name="_PathToSave_">Local path where to download. If empty, the file download in current folder</param>
		public GetAsync(CSFtpClientConfiguration _Configuration_, string _FileName_, string _PathToSave_)
			: base(_Configuration_)
		{
			this.Files = new List<FileInfo>();
			this.Files.Add(new FileInfo(Path.Combine(_PathToSave_, _FileName_)));
		}

		public GetAsync(CSFtpClientConfiguration _Configuration_, List<string> _FileNames_, string _PathToSave_)
			: base(_Configuration_)
		{
			this.Files = new List<FileInfo>();

			_FileNames_.ForEach(file => this.Files.Add(new FileInfo(Path.Combine(_PathToSave_, file))));
		}

		public override Task ExecuteAsync()
		{
			try
			{
				return new Task(() =>
				{
					foreach (var File_ in this.Files)
					{
						base.RunProcess(File_, WebRequestMethods.Ftp.DownloadFile);
					}

					OnFileDownloadCompleted();
				});
			}
			catch (Exception ex)
			{
				base.OnErrorOccurred(ex.Message);
			}

			return null;
		}

		internal override void ExecuteCommand(FtpWebRequest _FtpWebRequest_, FileInfo _File_)
		{
			OnFileDownloadloading(_File_);

			using (FtpWebResponse Response_ = (FtpWebResponse)_FtpWebRequest_.GetResponseAsync().Result)
			{
				using (Stream Reader_ = Response_.GetResponseStream())
				{
					using (FileStream FileStream_ = new FileStream(_File_.FullName, FileMode.Create))
					{
						Reader_.CopyTo(FileStream_);
					}
				}
			}

			OnFileDownloaded(_File_);
		}

		#region "Events section"

		private void OnFileDownloadloading(FileInfo _File_)
		{
			if (FileDownloading != null)
				FileDownloading(this, _File_);
		}

		private void OnFileDownloaded(FileInfo _File_)
		{
			if (FileDownloaded != null)
				FileDownloaded(this, _File_);
		}

		private void OnFileDownloadCompleted()
		{
			if (FileDownloadCompleted != null)
				FileDownloadCompleted(this);
		}

		#endregion

	}
}
