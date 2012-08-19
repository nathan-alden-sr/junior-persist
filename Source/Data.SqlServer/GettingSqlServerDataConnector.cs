using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// A SQL Server data connector that retrieves data.
	/// </summary>
	public abstract class GettingSqlServerDataConnector<TData> : SqlServerDataConnector
		where TData : class
	{
		private readonly GettingSqlServerDataConnectorHelper<TData> _gettingSqlServerDataConnectorHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettingSqlServerDataConnector{TData}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected GettingSqlServerDataConnector(
			IConnectionProvider<SqlConnection> connectionProvider,
			ICommandProvider<SqlConnection, SqlCommand, SqlParameter> commandProvider,
			string connectionKey)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			_gettingSqlServerDataConnectorHelper = new GettingSqlServerDataConnectorHelper<TData>(connectionProvider, commandProvider, connectionKey, GetData);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected TData GetData(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingSqlServerDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected TData GetData(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingSqlServerDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IEnumerable<TData> GetDatas(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingSqlServerDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IEnumerable<TData> GetDatas(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingSqlServerDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Converts a <see cref="DataRow"/> to data.
		/// </summary>
		/// <param name="row">A <see cref="DataRow"/> instance.</param>
		/// <returns>Data populated with data from <paramref name="row"/>.</returns>
		protected abstract TData GetData(DataRow row);
	}
}