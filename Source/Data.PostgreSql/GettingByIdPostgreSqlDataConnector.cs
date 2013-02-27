using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Junior.Common;

using Npgsql;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// A PostgreSQL data connector that retrieves entities by ID.
	/// </summary>
	public abstract class GettingByIdPostgreSqlDataConnector<TEntityData> : PostgreSqlDataConnector, IGettingByIdDataConnector<TEntityData>
		where TEntityData : class, IEntityData
	{
		private readonly GettingPostgreSqlDataConnectorHelper<TEntityData> _gettingPostgreSqlDataConnectorHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettingByIdPostgreSqlDataConnector{TEntityData}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected GettingByIdPostgreSqlDataConnector(
			IConnectionProvider<NpgsqlConnection> connectionProvider,
			ICommandProvider<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter> commandProvider,
			string connectionKey)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			connectionProvider.ThrowIfNull("connectionProvider");
			commandProvider.ThrowIfNull("commandProvider");
			connectionKey.ThrowIfNull("connectionKey");

			_gettingPostgreSqlDataConnectorHelper = new GettingPostgreSqlDataConnectorHelper<TEntityData>(connectionProvider, commandProvider, connectionKey, GetEntityData);
		}

		/// <summary>
		/// Retrieves entity data for an entity with the specified ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <returns>Entity data for an entity with the specified ID.</returns>
		public abstract Task<TEntityData> GetById(BinaryGuid id);

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual async Task<TEntityData> GetEntityData(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="TooManyRowsException">Thrown when the SQL query unexpectedly returns too many rows.</exception>
		protected virtual async Task<TEntityData> GetEntityData(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual async Task<IEnumerable<TEntityData>> GetEntityDatas(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual async Task<IEnumerable<TEntityData>> GetEntityDatas(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return await _gettingPostgreSqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Converts a <see cref="DataRow"/> to entity data.
		/// </summary>
		/// <param name="row">A <see cref="DataRow"/> instance.</param>
		/// <returns>Entity data populated with data from <paramref name="row"/>.</returns>
		protected abstract TEntityData GetEntityData(DataRow row);
	}
}