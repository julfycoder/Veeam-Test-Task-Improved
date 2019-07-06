using Cmprsr.Console.Input;
using Cmprsr.Console.Input.Entities;
using System;

namespace Cmprsr.Console
{
	class Program
	{
		static IFileResizer resizer;
		static void Main(string[] args)
		{
			Initialize();
			// Start interrogating

			try
			{
				// Get command line arguments
				CompressionCmdArguments cmdArguments = CompressionUserInputParser.GetCompressionCmdArguments(args);

				if (cmdArguments.Mode == "compress")
				{
					ShowProgress();
					resizer.Compress(cmdArguments.FileName, cmdArguments.ArchiveName);
				}
				else if (cmdArguments.Mode == "decompress")
				{
					ShowProgress();
					resizer.Decompress(cmdArguments.ArchiveName, cmdArguments.FileName);
				}

				System.Console.WriteLine("Operation finished successfully! Press Enter to exit");
				System.Console.ReadLine();
			}
			catch (Exception ex)
			{
				ShowException(ex);

				System.Console.WriteLine("Press Enter to exit.");

				System.Console.ReadLine();
			}
		}

		static string GetUserInputLine()
		{
			return System.Console.ReadLine();
		}

		static void ShowException(Exception ex)
		{
			System.Console.WriteLine();

			System.Console.WriteLine("Exception occured!:");

			System.Console.WriteLine(ex.Message);

			System.Console.WriteLine();
		}

		static void Initialize()
		{
			try
			{
				DependencyConfigurator.Configure();
			}
			catch(Exception ex)
			{
				System.Console.WriteLine(ex.Message);

				var container = DependencyConfigurator.Container;

				resizer = container.GetInstance<IFileResizer>();
			}
		}

		static void ShowProgress()
		{
			System.Console.WriteLine("Progress...");
		}
	}
}