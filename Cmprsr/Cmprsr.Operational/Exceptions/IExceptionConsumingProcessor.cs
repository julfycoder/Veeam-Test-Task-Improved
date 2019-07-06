using System;

namespace Cmprsr.Operational.Exceptions
{
	public interface IExceptionConsumingProcessor
	{
		void Process(Action action);

		Exception GetLastException();
	}
}