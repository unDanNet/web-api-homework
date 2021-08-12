using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using AutoMapper;
using FluentMigrator.Runner;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Repositories;
using WebApiMetricsAgent.Jobs;
using WebApiMetricsAgent.Jobs.Utils;

namespace WebApiMetricsAgent
{
	public class Startup
	{
		private readonly IConfigurationRoot _dbConfig;
		
		public Startup(IWebHostEnvironment hostEnv)
		{
			_dbConfig = new ConfigurationBuilder()
				.SetBasePath(hostEnv.ContentRootPath).AddJsonFile("dbsettings.json")
				.Build();
		}
		

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddSingleton(
				new MapperConfiguration(mp => mp.AddProfile(new MapperProfile())).CreateMapper()
			);
				
			services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddSingleton<IDotnetMetricsRepository, DotnetMetricsRepository>();
			services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
			services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
			services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();

			services.AddSingleton(_dbConfig);

			
			services.AddSingleton<IJobFactory, JobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			services.AddSingleton<CpuMetricJob>();
			services.AddSingleton(new JobSchedule(
				jobType: typeof(CpuMetricJob),
				cronExpression: "0/5 * * * * ?"
			));
			
			services.AddSingleton<RamMetricJob>();
			services.AddSingleton(new JobSchedule(
				jobType: typeof(RamMetricJob),
				cronExpression: "0/5 * * * * ?"
			));
			
			services.AddSingleton<HddMetricJob>();
			services.AddSingleton(new JobSchedule(
				jobType: typeof(HddMetricJob),
				cronExpression: "0/5 * * * * ?"
			));
			
			services.AddSingleton<NetworkMetricJob>();
			services.AddSingleton(new JobSchedule(
				jobType: typeof(NetworkMetricJob),
				cronExpression: "0/5 * * * * ?"
			));
			
			services.AddSingleton<DotnetMetricJob>();
			services.AddSingleton(new JobSchedule(
				jobType: typeof(DotnetMetricJob),
				cronExpression: "0/5 * * * * ?"
			));
			
			
			services.AddFluentMigratorCore().ConfigureRunner(rb =>
				rb.AddSQLite()
					.WithGlobalConnectionString(_dbConfig.GetConnectionString("DefaultConnection"))
					.ScanIn(typeof(Startup).Assembly).For.Migrations()
			).AddLogging(lb => lb.AddFluentMigratorConsole());

			services.AddHostedService<QuartzHostedService>();

			services.AddSwaggerGen(sa => {
				sa.SwaggerDoc("v1", new OpenApiInfo {
					Version = "v1",
					Title = "API сервиса агента сбора метрик",
					Description = "Здесь представлен весь API сервиса",
					Contact = new OpenApiContact {
						Name = "Daniil",
						Email = "elgoogecaf@gmail.com"
					}
				});

				var xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
				
				sa.IncludeXmlComments(xmlCommentsFilePath);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			app.UseSwagger();

			app.UseSwaggerUI(op => {
				op.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса агента сбора метрик");
				op.RoutePrefix = string.Empty;
			});
			
			migrationRunner.MigrateUp();
		}
	}
}