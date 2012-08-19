using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// A SQL Server data connector that retrieves entities by ID and caches the results.
	/// </summary>
	public abstract class CachingGettingByIdSqlServerDataConnector<TEntityData> : SqlServerDataConnector, ICachingGettingByIdDataConnector<TEntityData>
		where TEntityData : class, IEntityData
	{
		private readonly ICache<CacheKey> _cache;

		/// <summary>
		/// Initializes a new instance of the <see cref="CachingGettingByIdSqlServerDataConnector{TEntityData}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="cache">A cache.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cache"/> is null.</exception>
		protected CachingGettingByIdSqlServerDataConnector(
			IConnectionProvider<SqlConnection> connectionProvider,
			ICommandProvider<SqlConnection, SqlCommand, SqlParameter> commandProvider,
			ICache<CacheKey> cache,
			string connectionKey)
			: base(connectionProvider, commandProvider, connectionKey)
		{
			cache.ThrowIfNull("cache");

			_cache = cache;
		}

		/// <summary>
		/// Retrieves entity data for an entity with the specified ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <returns>A query result specifying either cached entity data or containing the entity data itself for the specified entity ID.</returns>
		public abstract IQueryResult<TEntityData> GetById(Guid id);

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A query result specifying either cached entity data or containing the entity data itself.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IQueryResult<TEntityData> GetEntityData(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return GetEntityData(sql, (IEnumerable<SqlParameter>)parameters);
		}

		/// <summary>
		/// Retrieves entity data by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A query result specifying either cached entity data or containing the entity data itself.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="TooManyRowsException">Thrown when the SQL query unexpectedly returns too many rows.</exception>
		protected IQueryResult<TEntityData> GetEntityData(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			var key = new CacheKey(sql);

			if (_cache.IsCached(key))
			{
				return new CacheQueryResult<TEntityData>(key);
			}

			IEnumerable<TEntityData> entityDatas = ExecuteProjection(sql, GetEntityData, parameters);

			// ReSharper disable PossibleMultipleEnumeration
			if (entityDatas.CountGreaterThan(1))
			{
				throw new TooManyRowsException(String.Format("A query for a single entity row resulted in more than one row.{0}Type: {1}", Environment.NewLine, typeof(TEntityData).FullName));
			}

			return new ResultQueryResult<TEntityData>(key, entityDatas.FirstOrDefault());
			// ReSharper restore PossibleMultipleEnumeration
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A query result specifying either cached entity data or containing the entity data itself.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IQueryResult<IEnumerable<TEntityData>> GetEntityDatas(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return GetEntityDatas(sql, (IEnumerable<SqlParameter>)parameters);
		}

		/// <summary>
		/// Retrieves data for multiple entities by executing a SQL query.
		/// </summary>
		/// <param name="sql">A SQL query.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A query result specifying either cached entity data or containing the entity data itself.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected IQueryResult<IEnumerable<TEntityData>> GetEntityDatas(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			var key = new CacheKey(sql);

			if (_cache.IsCached(key))
			{
				return new CacheQueryResult<IEnumerable<TEntityData>>(key);
			}

			return new ResultQueryResult<IEnumerable<TEntityData>>(key, ExecuteProjection(sql, GetEntityData, parameters));
		}

		/// <summary>
		/// Converts a <see cref="DataRow"/> to entity data.
		/// </summary>
		/// <param name="row">A <see cref="DataRow"/> instance.</param>
		/// <returns>Entity data populated with data from <paramref name="row"/>.</returns>
		protected abstract TEntityData GetEntityData(DataRow row);
	}
}