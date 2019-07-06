using Cmprsr.Common.Entity;
using System.IO;

namespace Cmprsr.IO
{
	/// <summary>
	/// Represents operations for writing segments into the specified stream.
	/// </summary>
	public interface ISegmentStreamWriter
	{
		/// <summary>
		/// Writes segment into the specified stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="segment"></param>
		void Write(Stream stream, Segment segment);
	}
}