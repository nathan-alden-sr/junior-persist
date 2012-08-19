using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Junior.Common;
using Junior.Ddd.DomainModel;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// Represents a session keyed on the <see cref="CacheKey"/> class.
	/// </summary>
	public abstract class CacheKeySession<TSession> : ThreadLocalContext<TSession>, ISession<CacheKey, object>
		where TSession : CacheKeySession<TSession>
	{
		// ReSharper disable StaticFieldInGenericType
		private static readonly ThreadLocal<SortedDictionary<CacheKey, IEnumerable<object>>> _entitiesFoundByCacheKey = new ThreadLocal<SortedDictionary<CacheKey, IEnumerable<object>>>(() => new SortedDictionary<CacheKey, IEnumerable<object>>());
		private static readonly ThreadLocal<Dictionary<object, Guid>> _entityIdsByEntity = new ThreadLocal<Dictionary<object, Guid>>(() => new Dictionary<object, Guid>());
		private static readonly ThreadLocal<Dictionary<object, Guid>> _entityIdsByLazyEntity = new ThreadLocal<Dictionary<object, Guid>>(() => new Dictionary<object, Guid>());
		private static ThreadLocal<Guid> _sessionId = new ThreadLocal<Guid>(Guid.NewGuid);
		// ReSharper restore StaticFieldInGenericType
		private readonly ISessionObserver<CacheKey, object> _observer;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKeySession{TSession}"/> class.
		/// </summary>
		internal CacheKeySession()
			: this(new NullSessionObserver<CacheKey, object>())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKeySession{TSession}"/> class.
		/// </summary>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
		internal CacheKeySession(ISessionObserver<CacheKey, object> observer)
		{
			observer.ThrowIfNull("observer");

			_observer = observer;
		}

		/// <summary>
		/// Gets the session's ID.
		/// </summary>
		public Guid SessionId
		{
			get
			{
				return _sessionId.Value;
			}
		}

		/// <summary>
		/// Caches a persisted entity.
		/// </summary>
		/// <param name="cacheEntity">The persisted entity as a <see cref="CacheEntity{TEntity}"/> instance.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheEntity"/> is null.</exception>
		/// <exception cref="SessionException">Thrown when <paramref name="cacheEntity"/> has a different ID than the same entity in the cache.</exception>
		public void EntityWasPersisted(CacheEntity<object> cacheEntity)
		{
			cacheEntity.ThrowIfNull("cacheEntity");

			Guid cachedEntityId;

			if (_entityIdsByEntity.Value.TryGetValue(cacheEntity.Entity, out cachedEntityId) && cachedEntityId != cacheEntity.Id)
			{
				throw new SessionException("Entity has a different ID than the same entity in the cache.");
			}

			_entityIdsByEntity.Value[cacheEntity.Entity] = cacheEntity.Id;

			_observer.EntityPersisted(this, cacheEntity.Entity.GetType(), cacheEntity.Id);
		}

		/// <summary>
		/// Caches a found entity.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <param name="cacheEntity">The persisted entity as a <see cref="CacheEntity{TEntity}"/> instance.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheKey"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheEntity"/> is null.</exception>
		public void EntityWasFound(CacheKey cacheKey, CacheEntity<object> cacheEntity)
		{
			cacheKey.ThrowIfNull("cacheKey");
			cacheEntity.ThrowIfNull("cacheEntity");

			EntitiesWereFound(cacheKey, cacheEntity.ToEnumerable());
		}

		/// <summary>
		/// Caches multiple found entities.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <param name="cacheEntities">The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheKey"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheEntities"/> is null.</exception>
		/// <exception cref="SessionException">Thrown when at least one found entity has a different ID than the same entity in the cache.</exception>
		/// <exception cref="SessionException">Thrown when more than one type of entity for the same cache key is being cached.</exception>
		/// <exception cref="SessionException">Thrown when the same entity is being cached more than once.</exception>
		public void EntitiesWereFound(CacheKey cacheKey, IEnumerable<CacheEntity<object>> cacheEntities)
		{
			cacheKey.ThrowIfNull("cacheKey");
			cacheEntities.ThrowIfNull("cacheEntities");

			cacheEntities = cacheEntities.ToArray();

			if (_entityIdsByEntity.Value
				.Join(cacheEntities, pair => pair.Key, cacheEntity => cacheEntity.Entity, (pair, cacheEntity) => new { CachedEntityId = pair.Value, FoundEntityId = cacheEntity.Id })
				.Any(arg => arg.CachedEntityId != arg.FoundEntityId))
			{
				throw new SessionException("At least one found entity has a different ID than the same entity in the cache.");
			}
			if (cacheEntities
				.Select(arg => arg.Entity.GetType())
				.Distinct()
				.CountGreaterThan(1))
			{
				throw new SessionException("Cannot cache more than one type of entity per cache key.");
			}
			if (!cacheEntities
				     .Select(arg => arg.Entity)
				     .Distinct()
				     .CountEqual(cacheEntities.Count()))
			{
				throw new SessionException("Cannot cache same entity more than once.");
			}

			_entitiesFoundByCacheKey.Value[cacheKey] = cacheEntities.Select(arg => arg.Entity);
			_entityIdsByEntity.Value.ReplaceMany(cacheEntities.Select(arg => new KeyValuePair<object, Guid>(arg.Entity, arg.Id)));

			foreach (CacheEntity<object> cacheEntity in cacheEntities)
			{
				_observer.EntityFound(this, cacheEntity.Entity.GetType(), cacheEntity.Id);
			}
		}

		/// <summary>
		/// Removes an entity from the cache.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		public void RemoveEntity(object entity)
		{
			entity.ThrowIfNull("entity");

			RemoveEntities(entity.ToEnumerable());
		}

		/// <summary>
		/// Removes an entity from the cache.
		/// </summary>
		/// <param name="entityId">The ID of the entity to remove.</param>
		public void RemoveEntity(Guid entityId)
		{
			if (!_entityIdsByEntity.Value.ContainsValue(entityId))
			{
				return;
			}

			KeyValuePair<object, Guid> pair = _entityIdsByEntity.Value.Single(arg => arg.Value == entityId);

			RemoveEntity(pair.Key);
		}

		/// <summary>
		/// Removes entities from the cache.
		/// </summary>
		/// <param name="entities">The entities to remove.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		/// <exception cref="SessionException">Thrown when at least one entity was not cached before being removed.</exception>
		public void RemoveEntities(IEnumerable<object> entities)
		{
			entities.ThrowIfNull("entities");

			entities = entities.ToArray();

			if (entities.Intersect(_entityIdsByEntity.Value.Keys).Count() != entities.Count())
			{
				throw new SessionException("At least one entity was not cached before being removed.");
			}

			IEnumerable<CacheKey> foundCacheKeys = _entitiesFoundByCacheKey.Value
				.Where(arg => arg.Value.Intersect(entities).Any())
				.Select(arg => arg.Key)
				.ToArray();
			IEnumerable<object> foundEntities = _entitiesFoundByCacheKey.Value
				.Join(foundCacheKeys, pair => pair.Key, cacheKey => cacheKey, (pair, cacheKey) => pair.Value)
				.SelectMany(arg => arg);
			IEnumerable<object> cachedEntities = _entityIdsByEntity.Value.Keys.Intersect(entities);
			IEnumerable<object> foundAndCachedEntities = foundEntities.Concat(cachedEntities).Distinct();
			IEnumerable<KeyValuePair<object, Guid>> entitiesToRemove = _entityIdsByEntity.Value
				.Join(foundAndCachedEntities, pair => pair.Key, entity => entity, (pair, entity) => pair)
				.ToArray();

			_entitiesFoundByCacheKey.Value.RemoveMany(foundCacheKeys);
			_entityIdsByEntity.Value.RemoveMany(entitiesToRemove.Select(arg => arg.Key));

			foreach (KeyValuePair<object, Guid> entity in entitiesToRemove)
			{
				_observer.EntityRemoved(this, entity.Key.GetType(), entity.Value);
			}
		}

		/// <summary>
		/// Retrieves a cached entity's ID.
		/// </summary>
		/// <param name="entity">A cached entity.</param>
		/// <returns>The cached entity's ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		/// <exception cref="SessionException">Thrown when <paramref name="entity"/> is not cached.</exception>
		public Guid GetEntityId(object entity)
		{
			entity.ThrowIfNull("entity");

			Guid entityId;

			if (!_entityIdsByEntity.Value.TryGetValue(entity, out entityId))
			{
				throw new SessionException("Entity is not cached.");
			}

			return entityId;
		}

		/// <summary>
		/// Retrieves a cached entity's ID. If the entity is not already cached, then <paramref name="defaultEntityId"/> is returned.
		/// </summary>
		/// <param name="entity">A cached entity.</param>
		/// <param name="defaultEntityId">The ID to return if <paramref name="entity"/> is not cached.</param>
		/// <returns>The cached entity's ID if the entity is cached; otherwise, <paramref name="defaultEntityId"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		public Guid GetEntityId(object entity, Guid defaultEntityId)
		{
			entity.ThrowIfNull("entity");

			Guid entityId;

			return _entityIdsByEntity.Value.TryGetValue(entity, out entityId) ? entityId : defaultEntityId;
		}

		/// <summary>
		/// Retrieves a cached entity from the cache as a <see cref="CacheEntity{TEntity}"/> instance.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <returns>The cached entity as a <see cref="CacheEntity{TEntity}"/> instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheKey"/> is null.</exception>
		/// <exception cref="SessionException">Thrown when there is more than one entity associated with cache key.</exception>
		public CacheEntity<object> GetEntity(CacheKey cacheKey)
		{
			cacheKey.ThrowIfNull("cacheKey");

			IEnumerable<CacheEntity<object>> cacheEntities = GetEntities(cacheKey).ToArray();

			if (cacheEntities.CountGreaterThan(1))
			{
				throw new SessionException("There is more than one entity associated with cache key.");
			}

			return cacheEntities.SingleOrDefault();
		}

		/// <summary>
		/// Retrieves cached entities from the cache as <see cref="CacheEntity{TEntity}"/> instances.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <returns>The cached entitys as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheKey"/> is null.</exception>
		public IEnumerable<CacheEntity<object>> GetEntities(CacheKey cacheKey)
		{
			cacheKey.ThrowIfNull("cacheKey");

			IEnumerable<object> cacheEntities;

			return _entitiesFoundByCacheKey.Value.TryGetValue(cacheKey, out cacheEntities)
				       ? cacheEntities.Join(_entityIdsByEntity.Value, entity => entity, pair => pair.Key, (entity, pair) => new CacheEntity<object>(pair.Key, pair.Value))
				       : Enumerable.Empty<CacheEntity<object>>();
		}

		/// <summary>
		/// Instructs the cache to remove all entities of type <typeparamref name="TEntityToClear"/> from the cache.
		/// </summary>
		/// <typeparam name="TEntityToClear">The type of entity to remove from the cache.</typeparam>
		public void Clear<TEntityToClear>()
		{
			Clear(typeof(TEntityToClear));
		}

		/// <summary>
		/// Instructs the cache to remove all entities of the specified type from the cache.
		/// </summary>
		/// <param name="entityType">The type of entity to remove from the cache.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityType"/> is null.</exception>
		public void Clear(Type entityType)
		{
			entityType.ThrowIfNull("entityType");

			IEnumerable<CacheKey> cacheKeys = _entitiesFoundByCacheKey.Value
				.Where(arg1 => arg1.Value.Any(arg2 => arg2.GetType().IsAssignableFrom(entityType)))
				.Select(arg => arg.Key)
				.ToArray();
			IEnumerable<KeyValuePair<object, Guid>> cacheEntities = _entityIdsByEntity.Value
				.Where(arg => arg.Key.GetType().IsAssignableFrom(entityType))
				.ToArray();

			foreach (CacheKey cacheKey in cacheKeys)
			{
				_entitiesFoundByCacheKey.Value.Remove(cacheKey);
			}
			foreach (KeyValuePair<object, Guid> cacheEntity in cacheEntities)
			{
				_entityIdsByEntity.Value.Remove(cacheEntity.Key);

				_observer.EntityRemoved(this, cacheEntity.Key.GetType(), cacheEntity.Value);
			}
		}

		/// <summary>
		/// Instructs the cache to remove all entities from the cache.
		/// </summary>
		public void ClearAll()
		{
			foreach (KeyValuePair<object, Guid> entity in _entityIdsByEntity.Value)
			{
				_observer.EntityRemoved(this, entity.Key.GetType(), entity.Value);
			}
			_entityIdsByEntity.Value.Clear();
			_entityIdsByLazyEntity.Value.Clear();
			_entitiesFoundByCacheKey.Value.Clear();
		}

		/// <summary>
		/// Notifies the cache that a lazy entity was created.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <param name="entityId">The ID associated with the entity to be lazy-loaded.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="lazyEntity"/> is null.</exception>
		/// <exception cref="SessionException">Thrown when <paramref name="lazyEntity"/> is already cached.</exception>
		public void LazyEntityWasCreated(ILazyEntity<object> lazyEntity, Guid entityId)
		{
			lazyEntity.ThrowIfNull("lazyEntity");

			if (_entityIdsByLazyEntity.Value.ContainsKey(lazyEntity))
			{
				throw new SessionException("Cannot cache lazy entity more than once.");
			}

			_entityIdsByLazyEntity.Value[lazyEntity] = entityId;
		}

		/// <summary>
		/// Retrieves the ID of a lazy-loaded entity.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <returns>The ID of the lazy-loaded entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="lazyEntity"/> is null.</exception>
		/// <exception cref="SessionException">If <paramref name="lazyEntity"/> has been accessed, thrown when the entity is not cached.</exception>
		/// <exception cref="SessionException">If <paramref name="lazyEntity"/> has not been accessed, thrown when the lazy entity is not cached.</exception>
		public Guid GetEntityId(ILazyEntity<object> lazyEntity)
		{
			lazyEntity.ThrowIfNull("lazyEntity");

			Guid cachedEntityId;

			if (lazyEntity.IsValueCreated)
			{
				if (!_entityIdsByEntity.Value.TryGetValue(lazyEntity.Value, out cachedEntityId))
				{
					throw new SessionException("Entity is not cached.");
				}

				return cachedEntityId;
			}
			if (!_entityIdsByLazyEntity.Value.TryGetValue(lazyEntity, out cachedEntityId))
			{
				throw new SessionException("Lazy entity is not cached.");
			}

			return cachedEntityId;
		}

		/// <summary>
		/// The next outer scope on the current thread becomes the current context.
		/// </summary>
		/// <param name="disposing">When true, the next outer scope on the current thread becomes the current context; otherwise, a null operation.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (Current != null)
			{
				return;
			}

			_sessionId = new ThreadLocal<Guid>(Guid.NewGuid);
			ClearAll();
		}
	}
}