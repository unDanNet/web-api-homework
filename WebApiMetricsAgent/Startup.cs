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
using AutoMapper;
using FluentMigrator.Runner;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Repositories;

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
				
			services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddScoped<IDotnetMetricsRepository, DotnetMetricsRepository>();
			services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();
			services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
			services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();

			services.AddSingleton(_dbConfig);
			
			services.AddFluentMigratorCore().ConfigureRunner(rb =>
				rb.AddSQLite()
					.WithGlobalConnectionString(_dbConfig.GetConnectionString("DefaultConnection"))
					.ScanIn(typeof(Startup).Assembly).For.Migrations()
			).AddLogging(lb => lb.AddFluentMigratorConsole());
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
			
			migrationRunner.MigrateUp();
		}
	}
}