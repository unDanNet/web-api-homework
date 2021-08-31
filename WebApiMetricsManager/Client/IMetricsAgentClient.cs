using System.Threading.Tasks;
using Core.DTO.Responses;
using WebApiMetricsManager.DTO.Requests;

namespace WebApiMetricsManager.Client
{
	public interface IMetricsAgentClient
	{
		Task<AllCpuMetricsResponses> GetAllCpuMetricsAsync(GetAllCpuMetricsApiRequest request);

		Task<AllRamMetricsResponses> GetAllRamMetricsAsync(GetAllRamMetricsApiRequest request);

		Task<AllHddMetricsResponses> GetAllHddMetricsAsync(GetAllHddMetricsApiRequest request);

		Task<AllDotnetMetricsResponses> GetAllDotnetMetricsAsync(GetAllDotnetMetricsApiRequest request);

		Task<AllNetworkMetricsResponses> GetAllNetworkMetricsAsync(GetAllNetworkMetricsApiRequest request);
	}
}