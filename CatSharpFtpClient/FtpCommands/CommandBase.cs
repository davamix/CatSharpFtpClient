using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CatSharpFtpClient.FtpCommands
{
	public abstract class CommandBase : ICommand
	{
		public event ErrorEventHandler ErrorOccurred;
		public delegate void ErrorEventHandler(object sender, string e);

		public CSFtpClientConfiguration FtpConfiguration { get; set; }

		public CommandBase(CSFtpClientConfiguration _Configuration_)
		{
			this.FtpConfiguration = _Configuration_;
		}
		/// <summary>
		/// Configure the request with a params specified.
		/// </summary>
		/// <param name="_UriRequested_">Ftp uri to request. Filename to upload, folder to list...</param>
		/// <param name="_RequestMethod_">Method of request [WebRequestMethods.Ftp.*]</param>
		/// <remarks>Use WebRequestMethods.Ftp as second parameter</remarks>
		/// <returns>A FtpWebRequest configured ready to use.</returns>
		internal virtual FtpWebRequest ConfigureFtpWebRequest(Uri _UriRequested_, string _RequestMethod_)
		{
			FtpWebRequest FtpWebRequest_ = (FtpWebRequest)FtpWebRequest.Create(_UriRequested_);
			FtpWebRequest_.Credentials = new NetworkCredential(this.FtpConfiguration.User, this.FtpConfiguration.Password);
			FtpWebRequest_.KeepAlive = FtpConfiguration.KeepAlive;
			FtpWebRequest_.UseBinary = FtpConfiguration.UseBinary;
			FtpWebRequest_.Method = _RequestMethod_;

			return FtpWebRequest_;
		}

		/// <summary>
		/// Get URI with the server and file path to get/put
		/// </summary>
		/// <param name="_FileName_">File name to upload</param>
		/// <returns>Server URI for get/put file.</returns>
		internal virtual Uri GetUriFilePath(string _FileName_)
		{
			return new Uri(String.Format("{0}/{1}", this.FtpConfiguration.Server, _FileName_));
		}

		/// <summary>
		/// Set request method to command and execute it.
		/// </summary>
		/// <param name="_File_">Working file</param>
		/// <param name="_RequestMethod_">Request method [WebRequestMethods.Ftp]</param>
		/// <returns></returns>
		internal virtual bool RunProcess(FileInfo _File_, string _RequestMethod_)
		{
			bool RetVal_ = false;

			try
			{
				FtpWebRequest FtpWebRequest_ = ConfigureFtpWebRequest(GetUriFilePath(_File_.Name), _RequestMethod_);

				ExecuteCommand(FtpWebRequest_, _File_);

				RetVal_ = true;
			}
			catch (Exception ex)
			{
				OnErrorOccurred(ex.Message);
			}

			return RetVal_;
		}

		public abstract Task ExecuteAsync();
		internal abstract void ExecuteCommand(FtpWebRequest _FtpWebRequest_, FileInfo _File_);

		#region Events section

		internal void OnErrorOccurred(string _Message_)
		{
			if (ErrorOccurred != null)
				ErrorOccurred(this, _Message_);
		}

		#endregion
	}
}
