using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatSharpFtpClient
{
	public class CSFtpClientConfiguration
	{
		public string Server { get; private set; }
		public string User { get; private set; }
		public string Password { get; private set; }
		public bool KeepAlive { get; set; }
		public bool UseBinary { get; set; }

		public CSFtpClientConfiguration(string _FtpServer_, string _FtpUser_, string _FtpPassword_)
		{
			this.Server = _FtpServer_;
			this.User = _FtpUser_;
			this.Password = _FtpPassword_;
		}
	}
}
