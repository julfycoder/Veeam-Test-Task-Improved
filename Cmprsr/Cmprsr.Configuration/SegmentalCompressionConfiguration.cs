namespace Cmprsr.Configuration
{
	/// <summary>
	/// Represents configuration for the compression project.
	/// </summary>
	public class SegmentalCompressionConfiguration
	{
		/// <summary>
		/// Gets or sets count of threads, using in the compression operation.
		/// </summary>
		public int ThreadsCount { get; set; }

		/// <summary>
		/// Gets or sets size of the segments in bytes, using in the compression operation.
		/// </summary>
		public int SegmentSize { get; set; }
	}
}
