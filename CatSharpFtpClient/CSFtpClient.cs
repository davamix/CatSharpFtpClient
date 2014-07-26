using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using CatSharpFtpClient.FtpCommands;

namespace CatSharpFtpClient
{
	public class CSFtpClient
	{
		public event RunFinishedEventHandler RunFinished;
		public delegate void RunFinishedEventHandler(object sender);

		public CSFtpClientConfiguration FtpConfiguration { get; set; }

		public Queue<ICommand> Commands { get; set; }

		public CSFtpClient(string _FtpServer_, string _FtpUser_, string _FtpPassword_)
		{
			this.FtpConfiguration = new CSFtpClientConfiguration(_FtpServer_, _FtpUser_, _FtpPassword_)
									{
										KeepAlive = true,
										UseBinary = true
									};
			Initialize();
		}

		public CSFtpClient(CSFtpClientConfiguration _Configuration_)
		{
			this.FtpConfiguration = _Configuration_;
			Initialize();
		}

		private void Initialize()
		{
			this.Commands = new Queue<ICommand>();
		}

		public void AddCommand(ICommand _Command_)
		{
			if (_Command_ != null)
				this.Commands.Enqueue(_Command_);
		}

		public void Run()
		{
			foreach (ICommand Command_ in this.Commands)
			{
				var CommandRun_ = Command_.ExecuteAsync();
				CommandRun_.Start();
			}

			//OnRunFinished();
		}

		private void OnRunFinished()
		{
			if (RunFinished != null)
				RunFinished(this);
		}
	}
}
