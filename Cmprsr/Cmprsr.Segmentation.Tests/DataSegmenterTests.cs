using Cmprsr.Common.Entity;
using Cmprsr.Segmentation;
using NUnit.Framework;

namespace Tests.Unit
{
	[TestFixture]
	public class DataSegmenterTests
	{
		private DataSegmenter dataSegmenter;
		[SetUp]
		public void Setup()
		{
			dataSegmenter = new DataSegmenter();
		}

		[Test]
		public void GetSegment_WithIdAndData_ReturnsExpectedSegment()
		{
			// Arrange
			int id = 12;
			byte[] data = new byte[142];
			Segment expectedSegment = new Segment { Id = id, Data = data };

			// Act
			var actualSegment = dataSegmenter.GetSegment(id, data);

			// Assert
			Assert.AreEqual(expectedSegment.Id, id);
			Assert.AreEqual(expectedSegment.Data, data);
		}
	}
}