using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyNamespace;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiMetricsManager.Client;
using WebApiMetricsManager.Client.SwaggerClient;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Repositories;
using WebApiMetricsManager.Jobs;
using WebApiMetricsManager.Jobs.Utils;

namespace WebApiMetricsManager
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

			services.AddSingleton(_dbConfig);

			services.AddSingleton<IAgentsRepository, AgentsRepository>();
			services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
			services.AddSingleton<IDotnetMetricsRepository, DotnetMetricsRepository>();
			services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
			services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
			
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

			services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>().AddTransientHttpErrorPolicy(
				p => p.WaitAndRetryAsync(
					3,
					_ => TimeSpan.FromMilliseconds(1000)
				)
			);

			services.AddHttpClient<IMetricsAgentSwaggerClient, MetricsAgentSwaggerClient>().AddTransientHttpErrorPolicy(
				p => p.WaitAndRetryAsync(
					3,
					_ => TimeSpan.FromMilliseconds(1000)
				)
			);

			services.AddSwaggerGen(sa => {
				sa.SwaggerDoc("v1", new OpenApiInfo {
					Version = "v1",
					Title = "API сервиса менеджера сбора метрик",
					Description = "Здесь можно ознакомиться с API сервиса.",
					Contact = new OpenApiContact {
						Name = "Daniil",
						Email = "elgoogecaf@gmail.com"
					}
				});

				var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

				sa.IncludeXmlComments(xmlFilePath);
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
				op.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса менеджера сбора метрик");
				op.RoutePrefix = string.Empty;
			});
			
			migrationRunner.MigrateUp();
		}
	}
}