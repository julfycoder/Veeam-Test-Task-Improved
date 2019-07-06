using Cmprsr.Common.Entity;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cmprsr.IO
{
	/// <summary>
	/// Represents operations for writing segments into the specified stream.
	/// </summary>
	public class SyncSegmentStreamWriter : ISegmentStreamWriter
	{
		private int counter;

		private readonly List<Segment> segmentsBuffer;

		public SyncSegmentStreamWriter()
		{
			segmentsBuffer = new List<Segment>();
		}

		/// <summary>
		/// Writes segment into the specified stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="segment"></param>
		public void Write(Stream stream, Segment segment)
		{
			AddSegmentToBuffer(segment);

			WriteBuffer(stream);
		}

		/// <summary>
		/// Writes buffer of segments into the specified stream.
		/// </summary>
		/// <param name="stream">Stream to write to.</param>
		private void WriteBuffer(Stream stream)
		{
			Segment nextSegment;

			// If buffer has at least one segment and there is at least one record that should be written into the stream.
			while (segmentsBuffer.Any() && (nextSegment = segmentsBuffer.FirstOrDefault(segment => segment.Id == counter + 1)) != null)
			{
				// Write segment data to the stream.
				stream.Write(nextSegment.Data, 0, nextSegment.Data.Length);

				// Remove segment from buffer.
				segmentsBuffer.Remove(nextSegment);

				counter++;
			}
		}

		private void AddSegmentToBuffer(Segment segment)
		{
			// Initialize counter.
			if (segment.Id == 1) counter = 0;

			segmentsBuffer.Add(segment);
		}
	}
}