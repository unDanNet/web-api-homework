using Core.DTO.Responses;
using WebApiMetricsManager.DTO.Requests;

namespace WebApiMetricsManager.Client
{
	public interface IMetricsAgentClient
	{
		AllCpuMetricsResponses GetAllCpuMetrics(GetAllCpuMetricsApiRequest request);

		AllRamMetricsResponses GetAllRamMetrics(GetAllRamMetricsApiRequest request);

		AllHddMetricsResponses GetAllHddMetrics(GetAllHddMetricsApiRequest request);

		AllDotnetMetricsResponses GetAllDotnetMetrics(GetAllDotnetMetricsApiRequest request);

		AllNetworkMetricsResponses GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request);
	}
}