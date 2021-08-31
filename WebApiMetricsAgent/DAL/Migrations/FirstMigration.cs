using FluentMigrator;
using Microsoft.Extensions.Configuration;

namespace WebApiMetricsAgent.DAL.Migrations
{
	[Migration(1)]
	public class FirstMigration : Migration
	{
		private readonly IConfigurationSection _tables;
		
		public FirstMigration(IConfigurationRoot dbConfig)
		{
			_tables = dbConfig.GetSection("TablesNames");
		}

		public override void Up()
		{
			Create.Table(_tables["Cpu"])
				.WithColumn("Id").AsInt64().PrimaryKey().Identity()
				.WithColumn("Value").AsInt32()
				.WithColumn("Time").AsInt64();
			
			Create.Table(_tables["Dotnet"])
				.WithColumn("Id").AsInt64().PrimaryKey().Identity()
				.WithColumn("ErrorsCount").AsInt32()
				.WithColumn("Time").AsInt64();
			
			Create.Table(_tables["Hdd"])
				.WithColumn("Id").AsInt64().PrimaryKey().Identity()
				.WithColumn("SpaceLeft").AsInt32()
				.WithColumn("Time").AsInt64();
			
			Create.Table(_tables["Network"])
				.WithColumn("Id").AsInt64().PrimaryKey().Identity()
				.WithColumn("Value").AsInt32()
				.WithColumn("Time").AsInt64();
			
			Create.Table(_tables["Ram"])
				.WithColumn("Id").AsInt64().PrimaryKey().Identity()
				.WithColumn("MemoryAvailable").AsInt32()
				.WithColumn("Time").AsInt64();
		}

		public override void Down()
		{
			Delete.Table(_tables["Cpu"]);
			Delete.Table(_tables["Dotnet"]);
			Delete.Table(_tables["Hdd"]);
			Delete.Table(_tables["Network"]);
			Delete.Table(_tables["Ram"]);
		}
	}
}