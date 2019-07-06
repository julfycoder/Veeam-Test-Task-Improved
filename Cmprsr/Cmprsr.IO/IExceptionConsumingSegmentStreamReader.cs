using System;

namespace Cmprsr.IO
{
	public interface IExceptionConsumingSegmentStreamReader : ISegmentStreamReader
	{
		Exception GetLastException();
	}
}