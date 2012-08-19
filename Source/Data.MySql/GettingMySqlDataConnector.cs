using System;
using System.Collections.Generic;
using System.Data;

using Junior.Common;

using MySql.Data.MySqlClient;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// A MySQL data connector that retrieves data.
	/// </summary>
	public abstract class GettingMySqlDataConnector<TData> : MySqlDataConnector
		where TData : class
	{
		private readonly GettingMySqlDataConnectorHelper<TData> _gettingMySqlDataConnectorHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettingMySqlDataConnector{TData}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected GettingMySqlDataConnector(
			IConnectionProvider<MySqlConnection> connectionProvider,
			ICommandProvider<MySqlConnection, MySqlCommand, MySqlParameter> commandProvider,
			string connectionKey)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			connectionProvider.ThrowIfNull("connectionProvider");
			commandProvider.ThrowIfNull("commandProvider");
			connectionKey.ThrowIfNull("connectionKey");

			_gettingMySqlDataConnectorHelper = new GettingMySqlDataConnectorHelper<TData>(connectionProvider, commandProvider, connectionKey, GetData);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected TData GetData(string sql, params MySqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected TData GetData(string sql, IEnumerable<MySqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IEnumerable<TData> GetDatas(string sql, params MySqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IEnumerable<TData> GetDatas(string sql, IEnumerable<MySqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Converts a <see cref="DataRow"/> to data.
		/// </summary>
		/// <param name="row">A <see cref="DataRow"/> instance.</param>
		/// <returns>Data populated with data from <paramref name="row"/>.</returns>
		protected abstract TData GetData(DataRow row);
	}
}