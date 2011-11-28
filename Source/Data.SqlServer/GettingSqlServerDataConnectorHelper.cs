using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	internal class GettingSqlServerDataConnectorHelper<TData> : SqlServerDataConnector
		where TData : class
	{
		private readonly Func<DataRow, TData> _getDataDelegate;

		public GettingSqlServerDataConnectorHelper(
			IConnectionProvider<SqlConnection> connectionProvider,
			ICommandProvider<SqlConnection, SqlCommand, SqlParameter> commandProvider,
			string connectionKey,
			Func<DataRow, TData> getDataDelegate)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			getDataDelegate.ThrowIfNull("getDataDelegate");

			_getDataDelegate = getDataDelegate;
		}

		public TData GetData(string sql)
		{
			sql.ThrowIfNull("sql");

			return GetData(sql, Enumerable.Empty<SqlParameter>());
		}

		public TData GetData(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return GetData(sql, (IEnumerable<SqlParameter>)parameters);
		}

		public TData GetData(string sql, IEnumerable<SqlParameter> parameters)
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

		public IEnumerable<TData> GetDatas(string sql)
		{
			sql.ThrowIfNull("sql");

			return ExecuteProjection(sql, _getDataDelegate, Enumerable.Empty<SqlParameter>());
		}

		public IEnumerable<TData> GetDatas(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return ExecuteProjection(sql, _getDataDelegate, parameters);
		}

		public IEnumerable<TData> GetDatas(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return ExecuteProjection(sql, _getDataDelegate, parameters);
		}
	}
}