using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Junior.Common;

using Npgsql;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// A PostgreSQL data connector that retrieves data.
	/// </summary>
	public abstract class GettingPostgreSqlDataConnector<TData> : PostgreSqlDataConnector
		where TData : class
	{
		private readonly GettingPostgreSqlDataConnectorHelper<TData> _gettingPostgreSqlDataConnectorHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettingPostgreSqlDataConnector{TData}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected GettingPostgreSqlDataConnector(
			IConnectionProvider<NpgsqlConnection> connectionProvider,
			ICommandProvider<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter> commandProvider,
			string connectionKey)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			connectionProvider.ThrowIfNull("connectionProvider");
			commandProvider.ThrowIfNull("commandProvider");
			connectionKey.ThrowIfNull("connectionKey");

			_gettingPostgreSqlDataConnectorHelper = new GettingPostgreSqlDataConnectorHelper<TData>(connectionProvider, commandProvider, connectionKey, GetData);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<TData> GetData(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<TData> GetData(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<IEnumerable<TData>> GetDatas(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Retrieves data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<IEnumerable<TData>> GetDatas(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Converts a <see cref="DataRow"/> to data.
		/// </summary>
		/// <param name="row">A <see cref="DataRow"/> instance.</param>
		/// <returns>Data populated with data from <paramref name="row"/>.</returns>
		protected abstract TData GetData(DataRow row);
	}
}