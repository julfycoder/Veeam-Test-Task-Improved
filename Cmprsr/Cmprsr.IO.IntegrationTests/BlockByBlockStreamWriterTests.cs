using Cmprsr.Configuration;
using Cmprsr.IO;
using Cmprsr.Operational.Exceptions;
using Cmprsr.Parallelization;
using Cmprsr.Segmentation;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Tests.Integration
{
	public class BlockByBlockStreamWriterTests
	{
		private readonly string testFileName = "Test";

		private BlockByBlockStreamWriter blockStreamWriter;

		[SetUp]
		public void Setup()
		{
			SegmentalCompressionConfiguration configuration = new SegmentalCompressionConfiguration { ThreadsCount = 4, SegmentSize = 1024 };

			ExceptionConsumingProcessor exceptionConsumingProcessor = new ExceptionConsumingProcessor();

			blockStreamWriter = new BlockByBlockStreamWriter(
				new ParallelProcessor(new AsyncProcessor(), configuration),
				new ExceptionConsumingSegmentStreamReader(
					new SyncSegmentStreamReader(configuration, new DataSegmenter()), exceptionConsumingProcessor),
				new ExceptionConsumingSegmentStreamWriter(
					new SyncSegmentStreamWriter(), exceptionConsumingProcessor)
			);
		}

		[Test]
		public void Write_CorrectlyTransfersDataFromOneStreamToAnother()
		{
			// Arrange
			GenerateTestFile(testFileName);

			var actualStream = new MemoryStream();

			byte[] expectedBuffer;

			// Initialize expected buffer by filling it with data from file
			using (var fileStream = new FileStream(testFileName, FileMode.Open))
			{
				expectedBuffer = new byte[fileStream.Length];

				fileStream.Read(expectedBuffer, 0, expectedBuffer.Length);
			}

			using (var fileStream = new FileStream(testFileName, FileMode.Open))
			{
				// Act
				blockStreamWriter.Write(fileStream, actualStream);

				// Assert
				Assert.AreEqual(expectedBuffer, actualStream.ToArray());
			}
		}

		[TearDown]
		public void TearDown()
		{
			if (File.Exists(testFileName)) File.Delete(testFileName);
		}

		private byte[] GenerateBufferRandomly(long size)
		{
			Random rnd = new Random();

			byte[] buffer = new byte[size];

			rnd.NextBytes(buffer);

			return buffer;
		}

		private void GenerateTestFile(string fileName)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.Write(string.Join(',', GenerateBufferRandomly(8195).Select(b => b.ToString())));
				}
			}
		}

		private void DeleteTestFile(string fileName)
		{
			File.Delete(fileName);
		}
	}
}