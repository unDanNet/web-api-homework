using System;
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;
using MyNamespace;

namespace WebApiMetricsManager.Client.SwaggerClient
{
	[GeneratedCode("NSwag", "13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0))")]
	public interface IMetricsAgentSwaggerClient
	{
		/// <summary>Gather CPU metrics (% of processor time) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllCpuMetricsAsync(TimeSpan fromTime, TimeSpan toTime);
    
		/// <summary>Gather CPU metrics (% of processor time) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		void GetAllCpuMetrics(TimeSpan fromTime, TimeSpan toTime);
    
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <summary>Gather CPU metrics (% of processor time) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllCpuMetricsAsync(TimeSpan fromTime, TimeSpan toTime, CancellationToken cancellationToken);
    
		/// <summary>Gather .NET metrics (error events raised) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllDotnetMetricsAsync(TimeSpan fromTime, TimeSpan toTime);
    
		/// <summary>Gather .NET metrics (error events raised) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		void GetAllDotnetMetrics(TimeSpan fromTime, TimeSpan toTime);
    
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <summary>Gather .NET metrics (error events raised) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllDotnetMetricsAsync(TimeSpan fromTime, TimeSpan toTime, CancellationToken cancellationToken);
    
		/// <summary>Gather HDD metrics (disk space left) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllHddMetricsAsync(TimeSpan fromTime, TimeSpan toTime);
    
		/// <summary>Gather HDD metrics (disk space left) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		void GetAllHddMetrics(TimeSpan fromTime, TimeSpan toTime);
    
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <summary>Gather HDD metrics (disk space left) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllHddMetricsAsync(TimeSpan fromTime, TimeSpan toTime, CancellationToken cancellationToken);
    
		/// <summary>Gather Network metrics (packets per second) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllNetworkMetricsAsync(TimeSpan fromTime, TimeSpan toTime);
    
		/// <summary>Gather Network metrics (packets per second) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		void GetAllNetworkMetrics(TimeSpan fromTime, TimeSpan toTime);
    
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <summary>Gather Network metrics (packets per second) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllNetworkMetricsAsync(TimeSpan fromTime, TimeSpan toTime, CancellationToken cancellationToken);
    
		/// <summary>Gather RAM metrics (MBytes of available memory) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllRamMetricsAsync(TimeSpan fromTime, TimeSpan toTime);
    
		/// <summary>Gather RAM metrics (MBytes of available memory) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		void GetAllRamMetrics(TimeSpan fromTime, TimeSpan toTime);
    
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <summary>Gather RAM metrics (MBytes of available memory) at the specified time range.</summary>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>if metrics were gathered successfully.</returns>
		/// <exception cref="ApiException">A server side error occurred.</exception>
		Task GetAllRamMetricsAsync(TimeSpan fromTime, TimeSpan toTime, CancellationToken cancellationToken);
    
	}
}