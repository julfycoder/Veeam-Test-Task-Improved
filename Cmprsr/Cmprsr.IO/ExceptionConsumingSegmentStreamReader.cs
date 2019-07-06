using Cmprsr.Common.Entity;
using Cmprsr.Operational.Exceptions;
using System;
using System.IO;

namespace Cmprsr.IO
{
	public class ExceptionConsumingSegmentStreamReader : IExceptionConsumingSegmentStreamReader
	{
		private readonly ISegmentStreamReader segmentStreamReader;
		private readonly IExceptionConsumingProcessor exceptionConsumingProcessor;

		public ExceptionConsumingSegmentStreamReader(
			ISegmentStreamReader segmentStreamReader,
			IExceptionConsumingProcessor exceptionConsumingProcessor)
		{
			this.segmentStreamReader = segmentStreamReader;
			this.exceptionConsumingProcessor = exceptionConsumingProcessor;
		}

		public Exception GetLastException()
		{
			return exceptionConsumingProcessor.GetLastException();
		}

		public Segment Read(Stream stream, int segmentId)
		{
			Segment segment = null;

			exceptionConsumingProcessor.Process(() => segment = segmentStreamReader.Read(stream, segmentId));

			return segment;
		}
	}
}