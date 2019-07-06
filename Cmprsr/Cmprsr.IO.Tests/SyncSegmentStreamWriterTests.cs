using Cmprsr.Common.Entity;
using Cmprsr.IO;
using NUnit.Framework;
using System;
using System.IO;

namespace Tests.Unit
{
	public class SyncSegmentStreamWriterTests
	{
		private SyncSegmentStreamWriter syncStreamWriter;
		[SetUp]
		public void Setup()
		{
			syncStreamWriter = new SyncSegmentStreamWriter();
		}

		[Test]
		public void Write_With3UnorderedSegments_WritesSegmentsInCorrectOrder()
		{
			// Arrange
			var actualStream = new MemoryStream();
			var expectedStream = new MemoryStream();

			var segment1 = new Segment { Id = 1, Data = GenerateBufferRandomly(64) };
			var segment2 = new Segment { Id = 2, Data = GenerateBufferRandomly(64) };
			var segment3 = new Segment { Id = 3, Data = GenerateBufferRandomly(64) };

			expectedStream.Write(segment1.Data, 0, 64);
			expectedStream.Write(segment2.Data, 0, 64);
			expectedStream.Write(segment3.Data, 0, 64);

			// Act
			syncStreamWriter.Write(actualStream, segment2);
			syncStreamWriter.Write(actualStream, segment3);
			syncStreamWriter.Write(actualStream, segment1);

			var actualBuffer = new byte[actualStream.Length];
			var expectedBuffer = new byte[expectedStream.Length];

			// Get data both from actual and expected streams
			actualBuffer = actualStream.ToArray();
			expectedBuffer = expectedStream.ToArray();

			// Assert
			Assert.AreEqual(actualBuffer, expectedBuffer);
		}

		private byte[] GenerateBufferRandomly(long size)
		{
			Random rnd = new Random();

			byte[] buffer = new byte[size];

			rnd.NextBytes(buffer);

			return buffer;
		}
	}
}