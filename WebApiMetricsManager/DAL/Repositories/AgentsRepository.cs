using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Core.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Models;
using WebApiMetricsManager.DAL.TypeHandlers;

namespace WebApiMetricsManager.DAL.Repositories
{
	public class AgentsRepository : IAgentsRepository
	{
		private readonly string connectionString;
		private readonly string tableName;

		public AgentsRepository(IConfigurationRoot dbConfig)
		{
			SqlMapper.AddTypeHandler(new UriHandler());
			
			connectionString = dbConfig.GetConnectionString("DefaultConnection");
			tableName = dbConfig.GetFullTableName("Agents");
		}

		public IList<AgentInfo> GetAllItems()
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<AgentInfo>($"SELECT Id, Url, Enabled FROM {tableName}").ToList();
		}

		public AgentInfo GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<AgentInfo>(
				$"SELECT Id, Url, Enabled FROM {tableName} WHERE id = @id",
				new {id}
			);
		}

		/// <summary>
		/// DO NOT USE! <para/>
		/// This method is not supported for agents repository.
		/// </summary>
		/// <exception cref="NotSupportedException"></exception>
		public IList<AgentInfo> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			throw new NotSupportedException();
		}

		public void AddItem(AgentInfo item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(url, enabled) VALUES(@url, @enabled)",
				new {url = item.Url, enabled = item.Enabled}
			);
		}

		public void UpdateItem(AgentInfo item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"UPDATE {tableName} SET url = @url, enabled = @enabled WHERE id = @id",
				new {
					url = item.Url,
					enabled = item.Enabled,
					id = item.Id
				}
			);
		}

		public void DeleteItem(int itemId)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"DELETE FROM {tableName} WHERE id = @id",
				new {id = itemId}
			);
		}

		public AgentInfo AddItemAndGetItBack(AgentInfo item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(url, enabled) VALUES(@url, @enabled)",
				new {url = item.Url, enabled = item.Enabled}
			);

			return connection.QuerySingle<AgentInfo>(
				$"SELECT Id, Url, Enabled FROM {tableName} WHERE id = (SELECT MAX(Id) FROM {tableName})"
			);
		}
	}
}