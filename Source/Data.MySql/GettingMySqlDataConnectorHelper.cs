using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Junior.Common;

using MySql.Data.MySqlClient;

namespace Junior.Persist.Data.MySql
{
	internal class GettingMySqlDataConnectorHelper<TData> : MySqlDataConnector
		where TData : class
	{
		private readonly Func<DataRow, TData> _getDataDelegate;

		public GettingMySqlDataConnectorHelper(
			IConnectionProvider<MySqlConnection> connectionProvider,
			ICommandProvider<MySqlConnection, MySqlCommand, MySqlParameter> commandProvider,
			string connectionKey,
			Func<DataRow, TData> getDataDelegate)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			getDataDelegate.ThrowIfNull("getDataDelegate");

			_getDataDelegate = getDataDelegate;
		}

		public async Task<TData> GetData(string sql, params MySqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await GetData(sql, (IEnumerable<MySqlParameter>)parameters);
		}

		public async Task<TData> GetData(string sql, IEnumerable<MySqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			IEnumerable<TData> entityDatas = await ExecuteProjection(sql, _getDataDelegate, parameters);

			// ReSharper disable PossibleMultipleEnumeration
			if (entityDatas.CountGreaterThan(1))
			{
				throw new TooManyRowsException(String.Format("A query for a single entity row resulted in more than one row.{0}Type: {1}", Environment.NewLine, typeof(TData).FullName));
			}

			return entityDatas.FirstOrDefault();
			// ReSharper restore PossibleMultipleEnumeration
		}

		public async Task<IEnumerable<TData>> GetDatas(string sql, params MySqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteProjection(sql, _getDataDelegate, parameters);
		}

		public async Task<IEnumerable<TData>> GetDatas(string sql, IEnumerable<MySqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteProjection(sql, _getDataDelegate, parameters);
		}
	}
}