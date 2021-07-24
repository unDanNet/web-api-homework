using System;
using System.Collections.Generic;

namespace WebApiMetricsAgent.Interfaces
{
	public interface IRepository<T> where T : class
	{
		IList<T> GetAllItems();

		T GetItemById(int id);
		
		IList<T> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime);

		void AddItem(T item);

		void UpdateItem(T item);
		
		void DeleteItem(int itemId);
	}
}