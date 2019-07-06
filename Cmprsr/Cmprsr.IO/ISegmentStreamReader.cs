using Cmprsr.Common.Entity;
using System.IO;

namespace Cmprsr.IO
{
	public interface ISegmentStreamReader
	{
		Segment Read(Stream stream, int segmentId);
	}
}
