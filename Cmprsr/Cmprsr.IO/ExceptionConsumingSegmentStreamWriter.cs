using Cmprsr.Common.Entity;
using Cmprsr.Operational.Exceptions;
using System;
using System.IO;

namespace Cmprsr.IO
{
	public class ExceptionConsumingSegmentStreamWriter : IExceptionConsumingSegmentStreamWriter
	{
		private readonly ISegmentStreamWriter segmentStreamWriter;
		private readonly IExceptionConsumingProcessor exceptionConsumingProcessor;

		public ExceptionConsumingSegmentStreamWriter(
			ISegmentStreamWriter segmentStreamWriter,
			IExceptionConsumingProcessor exceptionConsumingProcessor)
		{
			this.segmentStreamWriter = segmentStreamWriter;
			this.exceptionConsumingProcessor = exceptionConsumingProcessor;
		}

		public Exception GetLastException()
		{
			return exceptionConsumingProcessor.GetLastException();
		}

		public void Write(Stream stream, Segment segment)
		{
			exceptionConsumingProcessor.Process(() => segmentStreamWriter.Write(stream, segment));
		}
	}
}