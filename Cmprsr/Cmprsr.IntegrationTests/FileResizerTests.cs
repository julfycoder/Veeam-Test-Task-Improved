using Cmprsr;
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
	public class FileResizerTests
	{
		private readonly string testFileName = "Test";
		private readonly string testArchiveName = "Test.zip";
		private readonly string testDecompressedFileName = "Test(Decompressed)";

		private FileResizer fileResizer;

		[SetUp]
		public void Setup()
		{
			SegmentalCompressionConfiguration configuration = new SegmentalCompressionConfiguration { ThreadsCount = 4, SegmentSize = 1024 };

			ExceptionConsumingProcessor exceptionConsumingProcessor = new ExceptionConsumingProcessor();

			fileResizer = new FileResizer(new BlockByBlockStreamWriter(
				new ParallelProcessor(new AsyncProcessor(), configuration),
				new ExceptionConsumingSegmentStreamReader(
					new SyncSegmentStreamReader(configuration, new DataSegmenter()), exceptionConsumingProcessor),
				new ExceptionConsumingSegmentStreamWriter(
					new SyncSegmentStreamWriter(), exceptionConsumingProcessor)
			));
		}

		[Test]
		public void Compress_FileSizeReduced()
		{
			// Arrange
			GenerateTestFile(testFileName);

			long initialFileSize = 0;

			using (FileStream fileStream = new FileStream(testFileName, FileMode.Open))
			{
				initialFileSize = fileStream.Length;
			}

			// Act
			fileResizer.Compress(testFileName, testArchiveName);

			long archiveLength = 0;

			using (FileStream fileStream = new FileStream(testArchiveName, FileMode.Open))
			{
				archiveLength = fileStream.Length;
			}
			// Assert
			Assert.True(archiveLength < initialFileSize && archiveLength != 0);
		}

		[Test]
		public void Decompress_CreatesTheSameFileAsCompressedOne()
		{
			// Arrange
			GenerateTestFile(testFileName);

			// Act
			fileResizer.Compress(testFileName, testArchiveName);
			fileResizer.Decompress(testArchiveName, testDecompressedFileName);


			byte[] testFileBuffer;

			byte[] testDecompressedFileBuffer;

			using (FileStream testFileStream = new FileStream(testFileName, FileMode.Open))
			{
				testFileBuffer = new byte[testFileStream.Length];
				testFileStream.Read(testFileBuffer, 0, testFileBuffer.Length);
			}

			using (FileStream testDecompressedFileStream = new FileStream(testDecompressedFileName, FileMode.Open))
			{
				testDecompressedFileBuffer = new byte[testDecompressedFileStream.Length];
				testDecompressedFileStream.Read(testDecompressedFileBuffer, 0, testDecompressedFileBuffer.Length);
			}

			// Assert
			Assert.AreEqual(testFileBuffer, testDecompressedFileBuffer);
		}

		[TearDown]
		public void TearDown()
		{
			if (File.Exists(testFileName)) File.Delete(testFileName);
			if (File.Exists(testArchiveName)) File.Delete(testArchiveName);
			if (File.Exists(testDecompressedFileName)) File.Delete(testDecompressedFileName);
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
	}
}