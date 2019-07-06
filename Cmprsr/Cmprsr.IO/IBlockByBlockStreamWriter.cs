using System.IO;

namespace Cmprsr.IO
{
	/// <summary>
	/// Represents operations for block-by-block writing from one stream to another.
	/// </summary>
	public interface IBlockByBlockStreamWriter
	{
		/// <summary>
		/// Writes data from one stream to another using block-by-block algorithm.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		void Write(Stream source, Stream destination);
	}
}