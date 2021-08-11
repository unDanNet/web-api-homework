using System;
using System.Net.Http;
using System.Text.Json;
using Core.DTO.Responses;
using Microsoft.Extensions.Logging;
using WebApiMetricsManager.DTO.Requests;

namespace WebApiMetricsManager.Client
{
	public class MetricsAgentClient : IMetricsAgentClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<MetricsAgentClient> _logger;
		
		
		public MetricsAgentClient(HttpClient client, ILogger<MetricsAgentClient> logger)
		{
			_client = client;
			_logger = logger;
		}

		
		public AllCpuMetricsResponses GetAllCpuMetrics(GetAllCpuMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime.TotalSeconds;
			var toTimeArg = request.ToTime.TotalSeconds;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}/api/metrics/cpu/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using (var responseStream = response.Content.ReadAsStreamAsync().Result)
				{
					return JsonSerializer.DeserializeAsync<AllCpuMetricsResponses>(responseStream).Result;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}

		public AllRamMetricsResponses GetAllRamMetrics(GetAllRamMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime.TotalSeconds;
			var toTimeArg = request.ToTime.TotalSeconds;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}/api/metrics/ram/available/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using (var responseStream = response.Content.ReadAsStreamAsync().Result)
				{
					return JsonSerializer.DeserializeAsync<AllRamMetricsResponses>(responseStream).Result;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}

		public AllHddMetricsResponses GetAllHddMetrics(GetAllHddMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime.TotalSeconds;
			var toTimeArg = request.ToTime.TotalSeconds;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}/api/metrics/hdd/left/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using (var responseStream = response.Content.ReadAsStreamAsync().Result)
				{
					return JsonSerializer.DeserializeAsync<AllHddMetricsResponses>(responseStream).Result;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}

		public AllDotnetMetricsResponses GetAllDotnetMetrics(GetAllDotnetMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime.TotalSeconds;
			var toTimeArg = request.ToTime.TotalSeconds;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}/api/metrics/dotnet/errors-count/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using (var responseStream = response.Content.ReadAsStreamAsync().Result)
				{
					return JsonSerializer.DeserializeAsync<AllDotnetMetricsResponses>(responseStream).Result;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}

		public AllNetworkMetricsResponses GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime.TotalSeconds;
			var toTimeArg = request.ToTime.TotalSeconds;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}/api/metrics/network/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using (var responseStream = response.Content.ReadAsStreamAsync().Result)
				{
					return JsonSerializer.DeserializeAsync<AllNetworkMetricsResponses>(responseStream).Result;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}
	}
}