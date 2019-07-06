using System;

namespace Cmprsr.Parallelization
{
	/// <summary>
	/// Represents operations for processing actions in parallel thread.
	/// </summary>
	public interface IParallelProcessor
	{
		/// <summary>
		/// Processes specified action in a parallel thread.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="threadsCount"></param>
		void Process(Action action);

		/// <summary>
		/// Returns count of available threads.
		/// </summary>
		/// <returns></returns>
		int GetActiveThreadsCount();

		/// <summary>
		/// Returns max possible count of threads.
		/// </summary>
		/// <returns></returns>
		int GetMaxCountOfThreads();

		/// <summary>
		/// Returns true if there are still available threads.
		/// </summary>
		/// <returns></returns>
		bool AvailableThreadsExist();
	}
}
