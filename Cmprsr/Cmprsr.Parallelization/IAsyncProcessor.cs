using System;

namespace Cmprsr.Parallelization
{
	public interface IAsyncProcessor
	{
		event EventHandler ProcessingEnded;
		
		/// <summary>
		/// Launches 'function' in a separate thread.
		/// </summary>
		/// <param name="function">Target function, that should be processed in a separate thread.</param>
		void Process(Action function);
	}
}
