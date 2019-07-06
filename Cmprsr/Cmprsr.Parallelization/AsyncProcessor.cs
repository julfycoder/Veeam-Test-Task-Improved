using System;
using System.Threading;

namespace Cmprsr.Parallelization
{
	public class AsyncProcessor : IAsyncProcessor
	{
		public event EventHandler ProcessingEnded;

		public void Process(Action function)
		{
			Thread thread = new Thread(() =>
			{
				function();
				InvokeProcessingEnded(this);
			});

			thread.Start();
		}

		private void InvokeProcessingEnded(object sender)
		{
			ProcessingEnded?.Invoke(sender, new EventArgs());
		}
	}
}