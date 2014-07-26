using System;
using System.Threading.Tasks;

namespace CatSharpFtpClient.FtpCommands
{
	public interface ICommand
	{
		Task ExecuteAsync();
	}
}
