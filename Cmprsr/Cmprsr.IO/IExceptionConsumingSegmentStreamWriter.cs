using System;

namespace Cmprsr.IO
{
	public interface IExceptionConsumingSegmentStreamWriter : ISegmentStreamWriter
	{
		Exception GetLastException();
	}
}
