using Cmprsr.IO;
using System;
using System.IO;
using System.IO.Compression;

namespace Cmprsr
{
	/// <summary>
	/// Represents operations for compression and decompression of the files.
	/// </summary>
	public class FileResizer : IFileResizer
	{
		private readonly IBlockByBlockStreamWriter blockByBlockStreamWriter;
		public FileResizer(IBlockByBlockStreamWriter blockByBlockStreamWriter)
		{
			this.blockByBlockStreamWriter = blockByBlockStreamWriter;
		}

		/// <summary>
		/// Compresses specified file and creates specified archive.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="archiveName"></param>
		public void Compress(string fileName, string archiveName)
		{
			try
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					using (FileStream archiveStream = new FileStream(archiveName, FileMode.OpenOrCreate))
					{
						using (GZipStream compressStream = new GZipStream(archiveStream, CompressionMode.Compress))
						{
							blockByBlockStreamWriter.Write(fileStream, compressStream);
						}
					}
				}
			}
			catch (Exception ex)
			{
				File.Delete(archiveName);

				throw ex;
			}
		}

		/// <summary>
		/// Decopresses specified archive and creates specified file.
		/// </summary>
		/// <param name="archiveName"></param>
		/// <param name="fileName"></param>
		public void Decompress(string archiveName, string fileName)
		{
			try
			{
				using (FileStream archiveStream = new FileStream(archiveName, FileMode.Open))
				{
					using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
					{
						using (GZipStream decompressStream = new GZipStream(archiveStream, CompressionMode.Decompress))
						{
							blockByBlockStreamWriter.Write(decompressStream, fileStream);
						}
					}
				}
			}
			catch (Exception ex)
			{
				File.Delete(fileName);

				throw ex;
			}
		}
	}
}