namespace Cmprsr
{
	/// <summary>
	/// Represents operations for compression and decompression of the files.
	/// </summary>
	public interface IFileResizer
	{
		/// <summary>
		/// Compresses specified file and creates specified archive.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="archiveName"></param>
		void Compress(string fileName, string archiveName);

		/// <summary>
		/// Decopresses specified archive and creates specified file.
		/// </summary>
		/// <param name="archiveName"></param>
		/// <param name="fileName"></param>
		void Decompress(string archiveName, string fileName);
	}
}
