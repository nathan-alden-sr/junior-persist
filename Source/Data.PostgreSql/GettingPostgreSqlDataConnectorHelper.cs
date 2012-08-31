using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Junior.Common;

using Npgsql;

namespace Junior.Persist.Data.PostgreSql
{
	internal class GettingPostgreSqlDataConnectorHelper<TData> : PostgreSqlDataConnector
		where TData : class
	{
		private readonly Func<DataRow, TData> _getDataDelegate;

		public GettingPostgreSqlDataConnectorHelper(
			IConnectionProvider<NpgsqlConnection> connectionProvider,
			ICommandProvider<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter> commandProvider,
			string connectionKey,
			Func<DataRow, TData> getDataDelegate)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			getDataDelegate.ThrowIfNull("getDataDelegate");

			_getDataDelegate = getDataDelegate;
		}

		public TData GetData(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return GetData(sql, (IEnumerable<NpgsqlParameter>)parameters);
		}

		public TData GetData(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			IEnumerable<TData> entityDatas = ExecuteProjection(sql, _getDataDelegate, parameters);

			// ReSharper disable PossibleMultipleEnumeration
			if (entityDatas.CountGreaterThan(1))
			{
				throw new TooManyRowsException(String.Format("A query for a single entity row resulted in more than one row.{0}Type: {1}", Environment.NewLine, typeof(TData).FullName));
			}

			return entityDatas.FirstOrDefault();
			// ReSharper restore PossibleMultipleEnumeration
		}

		public IEnumerable<TData> GetDatas(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return ExecuteProjection(sql, _getDataDelegate, parameters);
		}

		public IEnumerable<TData> GetDatas(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return ExecuteProjection(sql, _getDataDelegate, parameters);
		}
	}
}