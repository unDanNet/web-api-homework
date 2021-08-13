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
using WebApiMetricsAgent.Interfaces;
using WebApiMetricsAgent.Repositories;

namespace WebApiMetricsAgent
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddScoped<IDotnetMetricsRepository, DotnetMetricsRepository>();
			services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();
			services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
			services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
			
			ConfigureSqlLiteConnection(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}

		private void ConfigureSqlLiteConnection(IServiceCollection services)
		{
			const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";

			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				PrepareSchemes(connection);
			}
		}

		private void PrepareSchemes(SQLiteConnection connection)
		{
			using var command = new SQLiteCommand(connection);

			command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
			command.ExecuteNonQuery();

			command.CommandText = "CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";
			command.Parameters.AddWithValue("@value", 100);
			command.Parameters.AddWithValue("@time", 600);
			command.Prepare();
			command.ExecuteNonQuery();
			
			command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";
			command.Parameters.AddWithValue("@value", 300);
			command.Parameters.AddWithValue("@time", 2400);
			command.Prepare();
			command.ExecuteNonQuery();
			
			command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";
			command.Parameters.AddWithValue("@value", 10);
			command.Parameters.AddWithValue("@time", 6000);
			command.Prepare();
			command.ExecuteNonQuery();

			command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";
			command.ExecuteNonQuery();

			command.CommandText = "CREATE TABLE dotnetmetrics(id INTEGER PRIMARY KEY, errorsCount INT, time INT)";
			command.ExecuteNonQuery();

			command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
			command.ExecuteNonQuery();

			command.CommandText = "CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY, spaceLeft INT, time INT)";
			command.ExecuteNonQuery();

			command.CommandText = "DROP TABLE IF EXISTS networkmetrics";
			command.ExecuteNonQuery();

			command.CommandText = "CREATE TABLE networkmetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
			command.ExecuteNonQuery();

			command.CommandText = "DROP TABLE IF EXISTS rammetrics";
			command.ExecuteNonQuery();

			command.CommandText = "CREATE TABLE rammetrics(id INTEGER PRIMARY KEY, memoryAvailable INT, time INT)";
			command.ExecuteNonQuery();
		}
	}
}