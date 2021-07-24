using Core.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.DAL.Interfaces
{
	public interface INetworkMetricsRepository : IRepository<NetworkMetric> {}
}