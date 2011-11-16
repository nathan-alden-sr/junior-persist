using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Finders
{
	/// <summary>
	/// Finds entities and caches found entities.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TEntityData"></typeparam>
	public abstract class CacheKeyCachingFinder<TEntity, TEntityData>
		where TEntity : class
		where TEntityData : class
	{
		private readonly ISessionCache<CacheKey, object> _sessionCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKeyCachingFinder{TEntity,TEntityData}"/> class.
		/// </summary>
		/// <param name="sessionCache">The session cache to use when caching entities.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sessionCache"/> is null.</exception>
		protected CacheKeyCachingFinder(ISessionCache<CacheKey, object> sessionCache)
		{
			sessionCache.ThrowIfNull("sessionCache");

			_sessionCache = sessionCache;
		}

		/// <summary>
		/// Gets the session cache this finder uses.
		/// </summary>
		protected ISessionCache<CacheKey, object> SessionCache
		{
			get
			{
				return _sessionCache;
			}
		}

		/// <summary>
		/// Processes a query result and returns a cached entity.
		/// </summary>
		/// <param name="queryResult">A query result. The query result instructs the finder to either pull the entity from the cache or to use returned result data.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>A cached entity as a <see cref="CacheEntity{TEntity}"/> instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="queryResult"/> is null.</exception>
		protected CacheEntity<TEntity> GetEntity(IQueryResult<TEntityData> queryResult, EntityNotFoundHandling entityNotFoundHandling)
		{
			queryResult.ThrowIfNull("queryResult");

			if (queryResult.UseCache)
			{
				CacheEntity<object> cacheEntity = _sessionCache.GetEntity(queryResult.CacheKey);

				ValidateResult(cacheEntity, entityNotFoundHandling);

				return cacheEntity.IfNotNull(arg => arg.Cast<TEntity>());
			}
			else
			{
				ValidateResult(queryResult.Result, entityNotFoundHandling);

				CacheEntity<TEntity> cacheEntity = queryResult.Result.IfNotNull(GetEntity);

				if (cacheEntity != null)
				{
					_sessionCache.EntityWasFound(queryResult.CacheKey, cacheEntity);
				}

				return cacheEntity;
			}
		}

		/// <summary>
		/// Processes a query result and returns cached entities.
		/// </summary>
		/// <param name="queryResult">A query result. The query result instructs the finder to either pull the entities from the cache or to use returned result data.</param>
		/// <returns>Cached entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="queryResult"/> is null.</exception>
		protected IEnumerable<CacheEntity<TEntity>> GetEntities(IQueryResult<IEnumerable<TEntityData>> queryResult)
		{
			queryResult.ThrowIfNull("queryResult");

			if (queryResult.UseCache)
			{
				return _sessionCache.GetEntities(queryResult.CacheKey).Select(arg => arg.Cast<TEntity>());
			}

			IEnumerable<CacheEntity<TEntity>> cacheEntities = queryResult.Result.Select(GetEntity).ToArray();

			_sessionCache.EntitiesWereFound(queryResult.CacheKey, cacheEntities.Select(arg => (CacheEntity<object>)arg));

			return cacheEntities;
		}

		/// <summary>
		/// Converts entity data into an entity as a <see cref="CacheEntity{TEntity}"/> instance.
		/// </summary>
		/// <param name="entityData">Entity data.</param>
		/// <returns>A cached entity as a <see cref="CacheEntity{TEntity}"/> instance.</returns>
		protected abstract CacheEntity<TEntity> GetEntity(TEntityData entityData);

		/// <summary>
		/// Creates a lazy entity from an entity ID and a finder.
		/// </summary>
		/// <param name="entityId">An entity ID.</param>
		/// <param name="finder">The finder that will be used to retrieve the entity when the lazy entity is accessed.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>A lazy entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="finder"/> is null.</exception>
		protected LazyEntityById<TLazyEntity> Lazy<TLazyEntity>(
			Guid entityId,
			IByIdFinder<TLazyEntity> finder,
			EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException)
			where TLazyEntity : class
		{
			finder.ThrowIfNull("finder");

			var lazyEntity = new LazyEntityById<TLazyEntity>(entityId, finder);

			_sessionCache.LazyEntityWasCreated(lazyEntity, entityId);

			return lazyEntity;
		}

		/// <summary>
		/// Creates a lazy entity from an entity ID and a delegate.
		/// </summary>
		/// <param name="entityId">An entity ID.</param>
		/// <param name="delegate">A delegate that will be used to retrieve the entity when the lazy entity is accessed.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>A lazy entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="delegate"/> is null.</exception>
		protected LazyEntityById<TLazyEntity> Lazy<TLazyEntity>(
			Guid entityId,
			Func<Guid, EntityNotFoundHandling, TLazyEntity> @delegate,
			EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException)
			where TLazyEntity : class
		{
			@delegate.ThrowIfNull("delegate");

			var lazyEntity = new LazyEntityById<TLazyEntity>(entityId, @delegate);

			_sessionCache.LazyEntityWasCreated(lazyEntity, entityId);

			return lazyEntity;
		}

		/// <summary>
		/// Creates a lazy entity from an entity ID and a finder.
		/// </summary>
		/// <param name="entityId">An entity ID.</param>
		/// <param name="finder">The finder that will be used to retrieve the entity when the lazy entity is accessed.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>A lazy entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="finder"/> is null.</exception>
		protected LazyEntityById<TLazyEntity> Lazy<TLazyEntity>(
			Guid? entityId,
			IByIdFinder<TLazyEntity> finder,
			EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException)
			where TLazyEntity : class
		{
			finder.ThrowIfNull("finder");

			LazyEntityById<TLazyEntity> lazyEntity = entityId.IfNotNull(arg => new LazyEntityById<TLazyEntity>(arg, finder));

			if (lazyEntity != null && entityId != null)
			{
				_sessionCache.LazyEntityWasCreated(lazyEntity, entityId.Value);
			}

			return lazyEntity;
		}

		/// <summary>
		/// Creates a lazy entity from an entity ID and a delegate.
		/// </summary>
		/// <param name="entityId">An entity ID.</param>
		/// <param name="delegate">The delegate that will be used to retrieve the entity when the lazy entity is accessed.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>A lazy entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="delegate"/> is null.</exception>
		protected LazyEntityById<TLazyEntity> Lazy<TLazyEntity>(
			Guid? entityId,
			Func<Guid, EntityNotFoundHandling, TLazyEntity> @delegate,
			EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException)
			where TLazyEntity : class
		{
			@delegate.ThrowIfNull("delegate");

			LazyEntityById<TLazyEntity> lazyEntity = entityId.IfNotNull(arg => new LazyEntityById<TLazyEntity>(arg, @delegate));

			if (lazyEntity != null && entityId != null)
			{
				_sessionCache.LazyEntityWasCreated(lazyEntity, entityId.Value);
			}

			return lazyEntity;
		}

		/// <summary>
		/// Creates a lazy entity from a parent entity ID and a delegate.
		/// </summary>
		/// <param name="parentEntityId">The parent entity's ID.</param>
		/// <param name="delegate">The delegate that will be used to retrieve the entity when the lazy entity is accessed.</param>
		/// <returns>A lazy entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="delegate"/> is null.</exception>
		protected LazyEntitiesByParentId<TLazyEntity> Lazy<TLazyEntity>(
			Guid parentEntityId,
			Func<Guid, IEnumerable<TLazyEntity>> @delegate)
			where TLazyEntity : class
		{
			@delegate.ThrowIfNull("delegate");

			return new LazyEntitiesByParentId<TLazyEntity>(parentEntityId, @delegate);
		}

		/// <summary>
		/// Creates a lazy entity from a parent entity ID and a delegate.
		/// </summary>
		/// <param name="parentEntityId">The parent entity's ID.</param>
		/// <param name="delegate">The delegate that will be used to retrieve the entity when the lazy entity is accessed.</param>
		/// <returns>A lazy entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="delegate"/> is null.</exception>
		protected LazyEntitiesByParentId<TLazyEntity> Lazy<TLazyEntity>(
			Guid parentEntityId,
			Func<Guid, IEnumerable<CacheEntity<TLazyEntity>>> @delegate)
			where TLazyEntity : class
		{
			@delegate.ThrowIfNull("delegate");

			return new LazyEntitiesByParentId<TLazyEntity>(parentEntityId, arg1 => @delegate(arg1).Select(arg2 => arg2.Entity));
		}

		private static void ValidateResult(object result, EntityNotFoundHandling entityNotFoundHandling)
		{
			if (result == null && entityNotFoundHandling == EntityNotFoundHandling.ThrowException)
			{
				throw new EntityNotFoundException(String.Format("{0} not found.", typeof(TEntity).Name));
			}
		}
	}
}