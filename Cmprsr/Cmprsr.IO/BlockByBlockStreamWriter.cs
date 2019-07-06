using Cmprsr.Common.Entity;
using Cmprsr.Parallelization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cmprsr.IO
{
	/// <summary>
	/// Represents operations for block-by-block writing from one stream to another.
	/// </summary>
	public class BlockByBlockStreamWriter : IBlockByBlockStreamWriter
	{
		private readonly IExceptionConsumingSegmentStreamReader exceptionConsumingSegmentStreamReader;
		private readonly IExceptionConsumingSegmentStreamWriter exceptionConsumingSegmentStreamWriter;
		private readonly IParallelProcessor parallelProcessor;

		private int segmentId;
		private Segment inputSegment;
		private Stack<Segment> inputSegmentsBuffer;

		public BlockByBlockStreamWriter(
			IParallelProcessor parallelProcessor,
			IExceptionConsumingSegmentStreamReader exceptionConsumingSegmentStreamReader,
			IExceptionConsumingSegmentStreamWriter exceptionConsumingSegmentStreamWriter)
		{
			this.parallelProcessor = parallelProcessor;
			this.exceptionConsumingSegmentStreamReader = exceptionConsumingSegmentStreamReader;
			this.exceptionConsumingSegmentStreamWriter = exceptionConsumingSegmentStreamWriter;
		}

		/// <summary>
		/// Writes data from one stream to another using block-by-block algorithm.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		public void Write(Stream source, Stream destination)
		{
			InitializeWriting();

			Exception exception = null;

			do
			{
				if (exception == null)
				{
					TryWriteInputSegment(destination);

					TryReadInputSegment(source);

					exception = exceptionConsumingSegmentStreamWriter.GetLastException() ?? exceptionConsumingSegmentStreamReader.GetLastException();
				}
				else if (parallelProcessor.GetActiveThreadsCount() == 0)
				{
					throw exception;
				}
			}
			// Process while there is still remaining data in the stream OR not all of the streams completed their work
			while (!IsSegmentsBufferEmpty() || parallelProcessor.GetActiveThreadsCount() != 0 || InputDataExists());
		}

		private void InitializeWriting()
		{
			segmentId = 0;

			inputSegmentsBuffer = new Stack<Segment>();

			inputSegment = new Segment();
		}

		private void TryWriteInputSegment(Stream destination)
		{
			// If there is an available thread && here is data in the input buffer
			if (parallelProcessor.AvailableThreadsExist())
			{
				Segment segmentsBufferSegment = TryGetSegmentsBufferSegment();

				if (segmentsBufferSegment != null)
				{
					// Write segment in a separate thread
					parallelProcessor.Process(() =>
					{
						// Start writing followed by saving exception if it happens
						lock (destination) exceptionConsumingSegmentStreamWriter.Write(destination, segmentsBufferSegment);
					});
				}
			}
		}

		private void TryReadInputSegment(Stream source)
		{
			// If there is an available thread && not all of the threads are involved in reading data
			if (parallelProcessor.AvailableThreadsExist() && !IsInputBufferSizeThresholdReached()
					&& InputDataExists())
			{
				// Get Segment
				parallelProcessor.Process(() =>
				{
					inputSegment = GetNewSegment(source);

					if (InputDataExists()) TryAddSegmentToSegmentsBuffer(inputSegment);
				});
			}
		}

		private bool InputDataExists()
		{
			return inputSegment != null;
		}

		private bool IsInputBufferSizeThresholdReached()
		{
			lock (inputSegmentsBuffer)
			{
				return inputSegmentsBuffer.Count >= parallelProcessor.GetMaxCountOfThreads();
			}
		}

		private bool IsSegmentsBufferEmpty()
		{
			lock (inputSegmentsBuffer)
			{
				return !inputSegmentsBuffer.Any();
			}
		}

		private void TryAddSegmentToSegmentsBuffer(Segment segment)
		{
			lock (inputSegmentsBuffer) inputSegmentsBuffer.Push(segment);
		}

		private Segment TryGetSegmentsBufferSegment()
		{
			lock (inputSegmentsBuffer)
			{
				return inputSegmentsBuffer.Any() ? inputSegmentsBuffer.Pop() : null;
			}
		}

		private Segment GetNewSegment(Stream stream)
		{
			lock (stream)
			{
				return exceptionConsumingSegmentStreamReader.Read(stream, ++segmentId);
			}
		}
	}
}