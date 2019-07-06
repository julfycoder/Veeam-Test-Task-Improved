using System;

namespace Cmprsr.Operational.Exceptions
{
	public class ExceptionConsumingProcessor : IExceptionConsumingProcessor
	{
		private Exception actionException;
		public Exception GetLastException()
		{
			return actionException;
		}

		public void Process(Action action)
		{
			try
			{
				action();
			}
			catch(Exception ex)
			{
				actionException = ex;
			}
		}
	}
}