using AutoMapper;
using Core.DTO.Entities;
using WebApiMetricsAgent.DAL.Models;

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