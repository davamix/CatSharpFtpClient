using CatSharpFtpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFtpClient
{
	public abstract class TestBase
	{
		protected CSFtpClient Client { get; set; }

		public TestBase()
		{
			CSFtpClient Client_ = new CSFtpClient("ftp://127.0.0.1", "davamix", "davamix");
			Client_.RunFinished += OnRunFinished;

			this.Client = Client_;
		}

		protected virtual void OnErrorOccurred(object sender, string _Message_)
		{
			Console.WriteLine(string.Format("ERROR => {0}", _Message_));
		}
		protected virtual void OnRunFinished(object sender)
		{
			Console.WriteLine("All commands are processed");
		}
	}
}
