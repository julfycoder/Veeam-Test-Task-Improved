using Cmprsr.Configuration;
using Cmprsr.IO;
using Cmprsr.Operational.Exceptions;
using Cmprsr.Parallelization;
using Cmprsr.Segmentation;
using SimpleInjector;

using System.Configuration;
using System.Linq;

namespace Cmprsr
{
	/// <summary>
	/// Represents configuration of dependencies in the project.
	/// </summary>
	public static class DependencyConfigurator
	{
		/// <summary>
		/// Configures all dependencies in the project.
		/// </summary>
		/// <param name="container">Dependecies container.</param>
		public static void Configure(Container container)
		{
			Container = container;

			container.Register<IDataSegmenter, DataSegmenter>(Lifestyle.Singleton);
			container.Register<IAsyncProcessor, AsyncProcessor>(Lifestyle.Singleton);
			container.Register<IParallelProcessor, ParallelProcessor>(Lifestyle.Singleton);
			container.Register<ISegmentStreamWriter, SyncSegmentStreamWriter>(Lifestyle.Singleton);
			container.Register<IExceptionConsumingSegmentStreamWriter, ExceptionConsumingSegmentStreamWriter>(Lifestyle.Singleton);
			container.Register<ISegmentStreamReader, SyncSegmentStreamReader>(Lifestyle.Singleton);
			container.Register<IExceptionConsumingSegmentStreamReader, ExceptionConsumingSegmentStreamReader>(Lifestyle.Singleton);
			container.Register<IExceptionConsumingProcessor, ExceptionConsumingProcessor>(Lifestyle.Singleton);
			container.Register<IBlockByBlockStreamWriter, BlockByBlockStreamWriter>(Lifestyle.Singleton);
			container.Register<IFileResizer, FileResizer>();

			ConfigureConfiguration(container);
		}

		/// <summary>
		/// Configures all dependencies in the project.
		/// </summary>
		public static void Configure()
		{
			Configure(new Container());
		}

		/// <summary>
		/// Dependencies container.
		/// </summary>
		public static Container Container { get; private set; }

		private static void ConfigureConfiguration(Container container)
		{
			int segmentSize = 1048576;
			int threadsCount = 12;

			var appSettings = ConfigurationManager.AppSettings;
			if (appSettings.AllKeys.ToList().Contains("SegmenstSize") && appSettings.AllKeys.ToList().Contains("ThreadsCount"))
			{
				segmentSize = int.Parse(appSettings["SegmentSize"]);
				threadsCount = int.Parse(appSettings["ThreadsCount"]);
			}
			else
			{
				container.RegisterInstance(new SegmentalCompressionConfiguration { SegmentSize = segmentSize, ThreadsCount = threadsCount });

				throw new ConfigurationErrorsException("Error occured while reading from configuration file! All configuration values will be set by default.");
			}
		}
	}
}