﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WebApiMetricsAgent.Interfaces;
using WebApiMetricsAgent.Models.Entities;

namespace WebApiMetricsAgent.Repositories
{
	public interface INetworkMetricsRepository : IRepository<NetworkMetric> {}
	
	
	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "networkmetrics";
		
		public IList<NetworkMetric> GetAllItems()
		{
			var result = new List<NetworkMetric>();
			
			// open the connection to the database
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				// create the command that will be sent to the database
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME}"
				}; 
				
				// start reading the data from the table
				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						result.Add(new NetworkMetric{
							Id = reader.GetInt32(0),
							Value = reader.GetInt32(1),
							Time = TimeSpan.FromSeconds(reader.GetInt32(2))
						});
					}
				}
			}
			
			return result;
		}

		public NetworkMetric GetItemById(int id)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME} WHERE id = @id",
				};

				command.Parameters.AddWithValue("@id", id);

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					return reader.Read() ? new NetworkMetric {
						Id = reader.GetInt32(0), 
						Value = reader.GetInt32(1), 
						Time = TimeSpan.FromSeconds(reader.GetInt32(2))
					} : null;
				}
			}
		}

		public IList<NetworkMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			var result = new List<NetworkMetric>();
			
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME} WHERE time >= @fromTime AND time <= @toTime"
				};

				command.Parameters.AddWithValue("@fromTime", fromTime.TotalSeconds);
				command.Parameters.AddWithValue("@toTime", toTime.TotalSeconds);

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						result.Add(new NetworkMetric {
							Id = reader.GetInt32(0),
							Value = reader.GetInt32(1),
							Time = TimeSpan.FromSeconds(reader.GetInt32(2))
						});
					}
				}
			}

			return result;
		}

		public void AddItem(NetworkMetric item)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"INSERT INTO {TABLE_NAME}(value, time) VALUES(@value, @time)"
				};
				
				command.Parameters.AddWithValue("@value", item.Value); // add the value for the 'value' parameter
				command.Parameters.AddWithValue("@time", item.Time.TotalSeconds); // add the value for the 'time' parameter
				
				command.Prepare(); // prepare command for the execution
				command.ExecuteNonQuery(); // execute the command
			}
		}

		public void UpdateItem(NetworkMetric item)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))	
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"UPDATE {TABLE_NAME} SET value = @value, time = @time WHERE id = @id"
				};

				command.Parameters.AddWithValue("@id", item.Id);
				command.Parameters.AddWithValue("@value", item.Value);
				command.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
				
				command.Prepare();
				command.ExecuteNonQuery();
			}
		}

		public void DeleteItem(int itemId)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"DELETE FROM {TABLE_NAME} WHERE id = @id"
				};

				command.Parameters.AddWithValue("@id", itemId);
				
				command.Prepare();
				command.ExecuteNonQuery();
			}
		}
	}
}