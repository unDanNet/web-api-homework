using AutoMapper;
using WebApiMetricsAgent.DAL.Models;
using WebApiMetricsAgent.DTO.Entities;

namespace WebApiMetricsAgent
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<CpuMetric, CpuMetricDto>();
			CreateMap<DotnetMetric, DotnetMetricDto>();
			CreateMap<HddMetric, HddMetricDto>();
			CreateMap<NetworkMetric, NetworkMetricDto>();
			CreateMap<RamMetric, RamMetricDto>();
		}
	}
}