using Core.Interfaces;
using WebApiMetricsManager.DAL.Models;

namespace WebApiMetricsManager.DAL.Interfaces
{
	public interface IAgentsRepository : IRepository<AgentInfo>
	{
		public AgentInfo AddItemAndGetItBack(AgentInfo item);
	}
}