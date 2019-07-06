using Cmprsr.Console.Input.Entities;
using System;

namespace Cmprsr.Console.Input
{
	/// <summary>
	/// Represents parsing of the compresison user input.
	/// </summary>
	public static class CompressionUserInputParser
	{
		/// <summary>
		/// Returns CompressionCmdArguments object containing required command line arguments.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static CompressionCmdArguments GetCompressionCmdArguments(string[] args)
		{
			try
			{
				CompressionCmdArguments cmdArguments = new CompressionCmdArguments
				{
					Mode = args[0]
				};

				if (cmdArguments.Mode == "compress")
				{
					cmdArguments.FileName = args[1];
					cmdArguments.ArchiveName = args[2];
				}
				else if (cmdArguments.Mode == "decompress")
				{
					cmdArguments.FileName = args[2];
					cmdArguments.ArchiveName = args[1];
				}
				else throw new Exception();

				return cmdArguments;
			}
			catch (Exception)
			{
				throw new ArgumentException(GetCommonInputValidationExceptionMessage());
			}
		}

		private static string GetCommonInputValidationExceptionMessage()
		{
			return "Specified command line arguments are incorrect!\r\n" +
					"Check spelling or order of the arguments.\r\n" +
					"Should be: compress [original file name] [archive file name] or decompress [archive file name] [decompressing file name]";
		}
	}
}