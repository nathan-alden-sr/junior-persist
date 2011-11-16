using System;
using System.Collections.Generic;
using System.Data;

using Junior.Common;

using MySql.Data.MySqlClient;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// A MySQL data connector that retrieves entities by ID.
	/// </summary>
	public abstract class GettingByIdMySqlDataConnector<TEntityData> : MySqlDataConnector, IGettingByIdDataConnector<TEntityData>
		where TEntityData : class, IEntityData
	{
		private readonly GettingMySqlDataConnectorHelper<TEntityData> _gettingMySqlDataConnectorHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="GettingByIdMySqlDataConnector{TEntityData}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected GettingByIdMySqlDataConnector(
			IConnectionProvider<MySqlConnection> connectionProvider,
			ICommandProvider<MySqlConnection, MySqlCommand, MySqlParameter> commandProvider,
			string connectionKey)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			connectionProvider.ThrowIfNull("connectionProvider");
			commandProvider.ThrowIfNull("commandProvider");
			connectionKey.ThrowIfNull("connectionKey");

			_gettingMySqlDataConnectorHelper = new GettingMySqlDataConnectorHelper<TEntityData>(connectionProvider, commandProvider, connectionKey, GetEntityData);
		}

		/// <summary>
		/// Retrieves entity data for an entity with the specified ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <returns>Entity data for an entity with the specified ID.</returns>
		public abstract TEntityData GetById(BinaryGuid id);

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual TEntityData GetEntityData(string sql)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetData(sql);
		}

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual TEntityData GetEntityData(string sql, params MySqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="TooManyRowsException">Thrown when the SQL query unexpectedly returns too many rows.</exception>
		protected virtual TEntityData GetEntityData(string sql, IEnumerable<MySqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetData(sql, parameters);
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual IEnumerable<TEntityData> GetEntityDatas(string sql)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetDatas(sql);
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual IEnumerable<TEntityData> GetEntityDatas(string sql, params MySqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>Entity data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected virtual IEnumerable<TEntityData> GetEntityDatas(string sql, IEnumerable<MySqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			return _gettingMySqlDataConnectorHelper.GetDatas(sql, parameters);
		}

		/// <summary>
		/// Converts a <see cref="DataRow"/> to entity data.
		/// </summary>
		/// <param name="row">A <see cref="DataRow"/> instance.</param>
		/// <returns>Entity data populated with data from <paramref name="row"/>.</returns>
		protected abstract TEntityData GetEntityData(DataRow row);
	}
}