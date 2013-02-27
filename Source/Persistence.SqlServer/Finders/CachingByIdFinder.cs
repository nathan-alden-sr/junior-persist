using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Data.SqlServer;
using Junior.Persist.Persistence.Finders;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.SqlServer.Finders
{
	/// <summary>
	/// Finds and caches an entity by ID.
	/// </summary>
	public abstract class CachingByIdFinder<TEntity, TEntityData, TDataConnector> : CacheKeyCachingFinder<TEntity, TEntityData>, IByIdFinder<TEntity>
		where TEntity : class
		where TEntityData : class, IEntityData
		where TDataConnector : class, ICachingGettingByIdDataConnector<TEntityData>
	{
		private readonly TDataConnector _dataConnector;

		/// <summary>
		/// Initializes a new instance of the <see cref="CachingByIdFinder{TEntity,TEntityData,TDataConnector}"/> class.
		/// </summary>
		/// <param name="sessionCache">The session cache to use when caching entities.</param>
		/// <param name="dataConnector">A data connector that finds entity data.</param>
		protected CachingByIdFinder(ISessionCache<CacheKey, object> sessionCache, TDataConnector dataConnector)
			: base(sessionCache)
		{
			dataConnector.ThrowIfNull("dataConnector");

			_dataConnector = dataConnector;
		}

		/// <summary>
		/// Gets the data connector that finds entity data.
		/// </summary>
		protected TDataConnector DataConnector
		{
			get
			{
				return _dataConnector;
			}
		}

		/// <summary>
		/// Finds an entity by ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		public async Task<TEntity> ById(Guid id, EntityNotFoundHandling entityNotFoundHandling)
		{
			IQueryResult<TEntityData> queryResult = await _dataConnector.GetById(id);

			return GetEntity(queryResult, entityNotFoundHandling).IfNotNull(arg => arg.Entity);
		}
	}
}