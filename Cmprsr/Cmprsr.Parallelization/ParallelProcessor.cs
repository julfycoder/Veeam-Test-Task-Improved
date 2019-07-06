using Cmprsr.Configuration;
using System;

namespace Cmprsr.Parallelization
{
	/// <summary>
	/// Represents operations for processing actions in parallel thread.
	/// </summary>
	public class ParallelProcessor : IParallelProcessor
	{
		private readonly IAsyncProcessor asyncProcessor;
		private readonly SegmentalCompressionConfiguration configuration;

		private int threadsCount;

		public ParallelProcessor(
			IAsyncProcessor asyncProcessor,
			SegmentalCompressionConfiguration configuration)
		{
			this.asyncProcessor = asyncProcessor;
			this.configuration = configuration;
			this.asyncProcessor.ProcessingEnded += AsyncProcessor_ProcessingEnded;
		}

		/// <summary>
		/// Returns true if there are still available threads.
		/// </summary>
		/// <returns></returns>
		public bool AvailableThreadsExist()
		{
			return GetActiveThreadsCount() < GetMaxCountOfThreads();
		}

		/// <summary>
		/// Returns count of available threads.
		/// </summary>
		/// <returns></returns>
		public int GetActiveThreadsCount()
		{
			return threadsCount;
		}

		/// <summary>
		/// Returns max possible count of threads.
		/// </summary>
		/// <returns></returns>
		public int GetMaxCountOfThreads()
		{
			return configuration.ThreadsCount;
		}

		/// <summary>
		/// Processes specified action in a parallel thread.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="threadsCount"></param>
		public void Process(Action function)
		{
			threadsCount++;
			asyncProcessor.Process(() => function());
		}

		private void AsyncProcessor_ProcessingEnded(object sender, EventArgs e)
		{
			threadsCount--;
		}
	}
}