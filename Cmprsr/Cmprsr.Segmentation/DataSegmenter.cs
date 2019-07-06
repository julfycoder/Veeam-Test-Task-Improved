using Cmprsr.Common.Entity;

namespace Cmprsr.Segmentation
{
	/// <summary>
	/// Represents generation of the segments.
	/// </summary>
	public class DataSegmenter : IDataSegmenter
	{
		public Segment GetSegment(int id, byte[] data)
		{
			return new Segment
			{
				Id = id,
				Data = data
			};
		}
	}
}