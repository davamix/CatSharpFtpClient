using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CatSharpFtpClient.FtpCommands
{
	public class PutAsync : CommandBase
	{
		public event FileUploadingEventHandler FileUploading;
		public event FileUploadedEventHandler FileUploaded;
		public event FileUploadCompletedEventHandler FileUploadCompleted;
		public delegate void FileUploadingEventHandler(object sender, FileInfo e);
		public delegate void FileUploadedEventHandler(object sender, FileInfo e);
		public delegate void FileUploadCompletedEventHandler(object sender);

		private List<FileInfo> Files { get; set; }

		public PutAsync(CSFtpClientConfiguration _Configuration_, FileInfo _FileName_)
			:base(_Configuration_)
		{
			this.Files = new List<FileInfo>();
			this.Files.Add(_FileName_);
		}

		public PutAsync(CSFtpClientConfiguration _Configuration_, List<FileInfo> _Files_)
			:base(_Configuration_)
		{
			this.Files = _Files_;
		}

		public override Task ExecuteAsync()
		{
			try
			{
				return new Task(() =>
				{
					foreach (FileInfo File_ in this.Files)
					{
						RunProcess(File_, WebRequestMethods.Ftp.UploadFile);
					}

					OnFileUploadCompleted();
				});
			}
			catch (Exception ex)
			{
				base.OnErrorOccurred(ex.Message);
			}

			return null;
		}

		/// <summary>
		/// Process for upload file to FTP
		/// </summary>
		/// <param name="FtpWebRequest_">Object for upload the file</param>
		/// <param name="_File_">File to upload</param>
		internal override void ExecuteCommand(FtpWebRequest FtpWebRequest_, FileInfo _File_)
		{
			OnFileUploading(_File_);

			using (FileStream FileStream_ = File.OpenRead(_File_.FullName))
			{
				byte[] Buffer_ = new byte[FileStream_.Length];
				FileStream_.Read(Buffer_, 0, Buffer_.Length);

				using (Stream Stream_ = FtpWebRequest_.GetRequestStream())
				{
					Stream_.Write(Buffer_, 0, Buffer_.Length);
				}
			}

			OnFileUploaded(_File_);
		}

		#region "Events section"

		private void OnFileUploading(FileInfo _File_)
		{
			if (FileUploading != null)
				FileUploading(this, _File_);
		}

		private void OnFileUploaded(FileInfo _File_)
		{
			if (FileUploaded != null)
				FileUploaded(this, _File_);
		}

		private void OnFileUploadCompleted()
		{
			if (FileUploadCompleted != null)
				FileUploadCompleted(this);
		}

		#endregion
	}
}
