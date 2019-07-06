using Cmprsr.Common.Entity;

namespace Cmprsr.Segmentation
{
	/// <summary>
	/// Represents generation of the segments.
	/// </summary>
	public interface IDataSegmenter
	{
		Segment GetSegment(int id, byte[] data);
	}
}
