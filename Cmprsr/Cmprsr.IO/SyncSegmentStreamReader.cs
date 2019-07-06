using Cmprsr.Common.Entity;
using Cmprsr.Configuration;
using Cmprsr.Segmentation;
using System.IO;
using System.Linq;

namespace Cmprsr.IO
{
	public class SyncSegmentStreamReader : ISegmentStreamReader
	{
		private readonly SegmentalCompressionConfiguration configuration;
		private readonly IDataSegmenter dataSegmenter;

		public SyncSegmentStreamReader(SegmentalCompressionConfiguration configuration, IDataSegmenter dataSegmenter)
		{
			this.configuration = configuration;
			this.dataSegmenter = dataSegmenter;
		}

		public Segment Read(Stream stream, int segmentId)
		{
			byte[] buffer = new byte[configuration.SegmentSize];

			Segment segment = null;

			int readResult = stream.Read(buffer, 0, buffer.Length);

			if (readResult > 0)
			{
				// Adjust buffer size to the stream length in case if we almost reached its end
				if (readResult < configuration.SegmentSize) buffer = buffer.Take(readResult).ToArray();

				segment = dataSegmenter.GetSegment(segmentId, buffer);
			}
			return segment;
		}
	}
}