using System;

namespace TestFtpClient
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				//PutTest();
				GetTest();
			}
			catch (Exception ex)
			{
				Console.WriteLine("=>ERROR MESSAGE<=");
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("END");
			Console.Read();
		}

		private static void PutTest()
		{
			PutCommandTest PutTest_ = new PutCommandTest();
			PutTest_.UploadOneFile();
			PutTest_.UploadManyFiles();
		}

		private static void GetTest()
		{
			GetCommandTest GetTest_ = new GetCommandTest();
			//GetTest_.DownloadOneFile();
			GetTest_.DownloadManyFiles();
		}


	}
}
