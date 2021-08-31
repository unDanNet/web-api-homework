using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
		

		public async Task<AllCpuMetricsResponses> GetAllCpuMetricsAsync(GetAllCpuMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime;
			var toTimeArg = request.ToTime;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}api/metrics/cpu/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					return await JsonSerializer.DeserializeAsync<AllCpuMetricsResponses>(responseStream);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}
		

		public async Task<AllRamMetricsResponses> GetAllRamMetricsAsync(GetAllRamMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime;
			var toTimeArg = request.ToTime;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}api/metrics/ram/available/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					return await JsonSerializer.DeserializeAsync<AllRamMetricsResponses>(responseStream);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}
		

		public async Task<AllHddMetricsResponses> GetAllHddMetricsAsync(GetAllHddMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime;
			var toTimeArg = request.ToTime;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}api/metrics/hdd/left/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					return await JsonSerializer.DeserializeAsync<AllHddMetricsResponses>(responseStream);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}
		

		public async Task<AllDotnetMetricsResponses> GetAllDotnetMetricsAsync(GetAllDotnetMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime;
			var toTimeArg = request.ToTime;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}api/metrics/dotnet/errors-count/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					return await JsonSerializer.DeserializeAsync<AllDotnetMetricsResponses>(responseStream);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			return null;
		}
		

		public async Task<AllNetworkMetricsResponses> GetAllNetworkMetricsAsync(GetAllNetworkMetricsApiRequest request)
		{
			var fromTimeArg = request.FromTime;
			var toTimeArg = request.ToTime;

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.AgentBaseAddress}api/metrics/network/from/{fromTimeArg}/to/{toTimeArg}"
			);

			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					return await JsonSerializer.DeserializeAsync<AllNetworkMetricsResponses>(responseStream);
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