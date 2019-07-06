namespace Cmprsr.Common.Entity
{
	/// <summary>
	/// Represents data segment.
	/// </summary>
	public class Segment
	{
		/// <summary>
		/// Id of the segment.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Data stored in the segment.
		/// </summary>
		public byte[] Data { get; set; }
	}
}
