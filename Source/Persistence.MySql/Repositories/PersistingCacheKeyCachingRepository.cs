using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Ddd.DomainModel;
using Junior.Persist.Data;
using Junior.Persist.Data.MySql;
using Junior.Persist.Persistence.Repositories;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.MySql.Repositories
{
	/// <summary>
	/// A repository that persists and caches entities using <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public abstract class PersistingCacheKeyCachingRepository<TEntity, TEntityData, TDataConnector> : IPersistingRepository<TEntity>
		where TEntity : class
		where TEntityData : class, IEntityData
		where TDataConnector : class, IInsertingOrUpdatingDataConnector<TEntityData>
	{
		private readonly TDataConnector _dataConnector;
		private readonly Func<CacheEntity<TEntity>, TEntityData> _entityDataMappingDelegate;
		private readonly IEntityIdFactory _entityIdFactory;
		private readonly ISessionCache<CacheKey, object> _sessionCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="PersistingCacheKeyCachingRepository{TEntity,TEntityData,TDataConnector}"/> class.
		/// </summary>
		/// <param name="sessionCache">The session cache to use when caching entities.</param>
		/// <param name="entityIdFactory">Generates new entity IDs when entities that are not in the cache are persisted.</param>
		/// <param name="dataConnector">A data connector that writes entity data.</param>
		/// <param name="entityDataMappingDelegate">A delegate that converts an entity into entity data.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sessionCache"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityIdFactory"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="dataConnector"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityDataMappingDelegate"/> is null.</exception>
		protected PersistingCacheKeyCachingRepository(
			ISessionCache<CacheKey, object> sessionCache,
			IEntityIdFactory entityIdFactory,
			TDataConnector dataConnector,
			Func<CacheEntity<TEntity>, TEntityData> entityDataMappingDelegate)
		{
			sessionCache.ThrowIfNull("sessionCache");
			entityIdFactory.ThrowIfNull("entityIdFactory");
			dataConnector.ThrowIfNull("dataConnector");
			entityDataMappingDelegate.ThrowIfNull("entityDataMappingDelegate");

			_sessionCache = sessionCache;
			_entityIdFactory = entityIdFactory;
			_dataConnector = dataConnector;
			_entityDataMappingDelegate = entityDataMappingDelegate;
		}

		/// <summary>
		/// Gets the data connector that writes entity data.
		/// </summary>
		protected TDataConnector DataConnector
		{
			get
			{
				return _dataConnector;
			}
		}

		/// <summary>
		/// Gets the session cache used to cache entities.
		/// </summary>
		protected ISessionCache<CacheKey, object> SessionCache
		{
			get
			{
				return _sessionCache;
			}
		}

		/// <summary>
		/// Persists an entity.
		/// </summary>
		/// <param name="entity">The entity to persist.</param>
		/// <returns>The entity's ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		public Guid Persist(TEntity entity)
		{
			entity.ThrowIfNull("entity");

			Guid entityId = _sessionCache.GetEntityId(entity, _entityIdFactory.NewId());
			TEntityData entityData = _entityDataMappingDelegate(new CacheEntity<TEntity>(entity, entityId));

			_dataConnector.InsertOrUpdate(entityData);

			_sessionCache.EntityWasPersisted(new CacheEntity<object>(entity, entityId));

			PersistRelated(entity);

			return entityId;
		}

		/// <summary>
		/// Persists entities and retrieves them as <see cref="CacheEntity{TEntity}"/> instances.
		/// </summary>
		/// <param name="entities">The entities to persist.</param>
		/// <returns>The entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		public IEnumerable<CacheEntity<TEntity>> Persist(IEnumerable<TEntity> entities)
		{
			entities.ThrowIfNull("entities");

			return entities.Select(arg => new CacheEntity<TEntity>(arg, Persist(arg)));
		}

		/// <summary>
		/// Persists lazy entities and retrieves them as <see cref="CacheEntity{TEntity}"/> instances.
		/// </summary>
		/// <param name="entities">The lazy entities to persist.</param>
		/// <returns>The entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		public IEnumerable<CacheEntity<TEntity>> Persist(LazyEntities<TEntity> entities)
		{
			entities.ThrowIfNull("entities");

			return entities.IsValueCreated ? entities.Value.Select(arg => new CacheEntity<TEntity>(arg, Persist(arg))) : null;
		}

		/// <summary>
		/// Persists entities related to <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity whose related entities are to be persisted.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		protected virtual void PersistRelated(TEntity entity)
		{
			entity.ThrowIfNull("entity");
		}
	}

	/// <summary>
	/// A repository that persists and caches entities by parent using <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public abstract class PersistingCacheKeyCachingRepository<TParentEntity, TEntity, TEntityData, TDataConnector> : IPersistingRepository<TParentEntity, TEntity>
		where TParentEntity : class
		where TEntity : class
		where TEntityData : class, IEntityData
		where TDataConnector : class, IInsertingOrUpdatingDataConnector<TEntityData>
	{
		private readonly TDataConnector _dataConnector;
		private readonly IEntityIdFactory _entityIdFactory;
		private readonly Func<TParentEntity, CacheEntity<TEntity>, TEntityData> _parentEntityDataMappingDelegate;
		private readonly ISessionCache<CacheKey, object> _sessionCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="PersistingCacheKeyCachingRepository{TEntity,TEntityData,TDataConnector}"/> class.
		/// </summary>
		/// <param name="sessionCache">The session cache to use when caching entities.</param>
		/// <param name="entityIdFactory">Generates new entity IDs when entities that are not in the cache are persisted.</param>
		/// <param name="dataConnector">A data connector that writes entity data.</param>
		/// <param name="parentEntityDataMappingDelegate">A delegate that converts an entity and its parent relationship into entity data.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sessionCache"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityIdFactory"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="dataConnector"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentEntityDataMappingDelegate"/> is null.</exception>
		protected PersistingCacheKeyCachingRepository(
			ISessionCache<CacheKey, object> sessionCache,
			IEntityIdFactory entityIdFactory,
			TDataConnector dataConnector,
			Func<TParentEntity, CacheEntity<TEntity>, TEntityData> parentEntityDataMappingDelegate)
		{
			sessionCache.ThrowIfNull("sessionCache");
			entityIdFactory.ThrowIfNull("entityIdFactory");
			dataConnector.ThrowIfNull("dataConnector");
			parentEntityDataMappingDelegate.ThrowIfNull("parentEntityDataMappingDelegate");

			_sessionCache = sessionCache;
			_entityIdFactory = entityIdFactory;
			_dataConnector = dataConnector;
			_parentEntityDataMappingDelegate = parentEntityDataMappingDelegate;
		}

		/// <summary>
		/// Gets the data connector that writes entity data.
		/// </summary>
		protected TDataConnector DataConnector
		{
			get
			{
				return _dataConnector;
			}
		}

		/// <summary>
		/// Gets the session cache used to cache entities.
		/// </summary>
		protected ISessionCache<CacheKey, object> SessionCache
		{
			get
			{
				return _sessionCache;
			}
		}

		/// <summary>
		/// Persists an entity and its parent relationship.
		/// </summary>
		/// <param name="parentEntity">The parent entity of the entity being persisted.</param>
		/// <param name="entity">The entity to persist.</param>
		/// <returns>The entity's ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentEntity"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		public Guid Persist(TParentEntity parentEntity, TEntity entity)
		{
			parentEntity.ThrowIfNull("parentEntity");
			entity.ThrowIfNull("entity");

			Guid entityId = _sessionCache.GetEntityId(entity, _entityIdFactory.NewId());
			TEntityData entityData = _parentEntityDataMappingDelegate(parentEntity, new CacheEntity<TEntity>(entity, entityId));

			_dataConnector.InsertOrUpdate(entityData);

			_sessionCache.EntityWasPersisted(new CacheEntity<object>(entity, entityId));

			PersistRelated(entity);

			return entityId;
		}

		/// <summary>
		/// Persists entities and their parent relationship.
		/// </summary>
		/// <param name="parentEntity">The parent entity of the entities being persisted.</param>
		/// <param name="entities">The entities to persist.</param>
		/// <returns>The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentEntity"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		public IEnumerable<CacheEntity<TEntity>> Persist(TParentEntity parentEntity, IEnumerable<TEntity> entities)
		{
			parentEntity.ThrowIfNull("parentEntity");
			entities.ThrowIfNull("entities");

			return entities.Select(arg => new CacheEntity<TEntity>(arg, Persist(parentEntity, arg)));
		}

		/// <summary>
		/// Persists lazy entities and their parent relationship.
		/// </summary>
		/// <param name="parentEntity">The parent entity of the lazy entities being persisted.</param>
		/// <param name="entities">The lazy entities to persist.</param>
		/// <returns>The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentEntity"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		public IEnumerable<CacheEntity<TEntity>> Persist(TParentEntity parentEntity, LazyEntities<TEntity> entities)
		{
			parentEntity.ThrowIfNull("parentEntity");
			entities.ThrowIfNull("entities");

			return entities.IsValueCreated ? entities.Value.Select(arg => new CacheEntity<TEntity>(arg, Persist(parentEntity, arg))) : null;
		}

		/// <summary>
		/// Persists entities and their parent relationship.
		/// </summary>
		/// <param name="parentEntity">The parent entity of the entities being persisted.</param>
		/// <param name="entities">The entities to persist.</param>
		/// <param name="getExistingEntityIdsDelegate">A delegate that retrieves already-persisted entity IDs for the entities being persisted.</param>
		/// <param name="persistingRepository">A persisting repository used to persist entities.</param>
		/// <param name="deletingByIdDataConnector">A deleting by ID data connector used to delete entities.</param>
		/// <param name="afterEntityAddedDelegate">A delegate that is invoked after an entity is added to the repository.</param>
		/// <param name="beforeEntityRemovedDelegate">A delegate that is invoked before an entity is removed from the repository.</param>
		/// <param name="afterEntityUpdatedDelegate">A delegate that is invoked after an entity is updated in the repository.</param>
		/// <returns>The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentEntity"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="getExistingEntityIdsDelegate"/> is null.</exception>
		public IEnumerable<CacheEntity<TEntity>> Persist(
			TParentEntity parentEntity,
			IEnumerable<TEntity> entities,
			Func<BinaryGuid, IEnumerable<BinaryGuid>> getExistingEntityIdsDelegate,
			IPersistingRepository<TParentEntity, TEntity> persistingRepository = null,
			IDeletingByIdDataConnector deletingByIdDataConnector = null,
			Action<TEntity> afterEntityAddedDelegate = null,
			Action<BinaryGuid> beforeEntityRemovedDelegate = null,
			Action<TEntity> afterEntityUpdatedDelegate = null)
		{
			parentEntity.ThrowIfNull("parentEntity");
			entities.ThrowIfNull("entities");
			getExistingEntityIdsDelegate.ThrowIfNull("getExistingEntityIdsDelegate");

			Guid parentEntityId = SessionCache.GetEntityId(parentEntity);
			var synchronizer = new EnumerableSynchronizer<BinaryGuid, TEntity>(getExistingEntityIdsDelegate(new BinaryGuid(parentEntityId)), entities);
			// ReSharper disable ImplicitlyCapturedClosure
			Action<TEntity> addedItemDelegate =
				arg =>
				// ReSharper restore ImplicitlyCapturedClosure
					{
						if (persistingRepository != null)
						{
							persistingRepository.Persist(parentEntity, arg);
						}
						if (afterEntityAddedDelegate != null)
						{
							afterEntityAddedDelegate(arg);
						}
					};
			Action<TEntity> commonItemDelegate =
				// ReSharper disable ImplicitlyCapturedClosure
				arg =>
				// ReSharper restore ImplicitlyCapturedClosure
					{
						if (persistingRepository != null)
						{
							persistingRepository.Persist(parentEntity, arg);
						}
						if (afterEntityUpdatedDelegate != null)
						{
							afterEntityUpdatedDelegate(arg);
						}
					};
			Action<BinaryGuid> removedItemDelegate =
				arg =>
					{
						if (beforeEntityRemovedDelegate != null)
						{
							beforeEntityRemovedDelegate(arg);
						}
						if (deletingByIdDataConnector != null)
						{
							deletingByIdDataConnector.DeleteById(arg);
							SessionCache.RemoveEntity(arg);
						}
					};

			synchronizer.Synchronize(addedItemDelegate, removedItemDelegate, commonItemDelegate);

			IEnumerable<TEntity> currentEntities = synchronizer.AddedElements.Concat(synchronizer.CommonElements);

			return currentEntities.Select(arg => new CacheEntity<TEntity>(arg, SessionCache.GetEntityId(arg)));
		}

		/// <summary>
		/// Persists lazy entities and their parent relationship.
		/// </summary>
		/// <param name="parentEntity">The parent entity of the lazy entities being persisted.</param>
		/// <param name="entities">The lazy entities to persist.</param>
		/// <param name="getExistingEntityIdsDelegate">A delegate that retrieves already-persisted entity IDs for the lazy entities being persisted.</param>
		/// <param name="persistingRepository">A persisting repository used to persist entities.</param>
		/// <param name="deletingByIdDataConnector">A deleting by ID data connector used to delete entities.</param>
		/// <param name="afterEntityAddedDelegate">A delegate that is invoked after an entity is added to the repository.</param>
		/// <param name="beforeEntityRemovedDelegate">A delegate that is invoked before an entity is removed from the repository.</param>
		/// <param name="afterEntityUpdatedDelegate">A delegate that is invoked after an entity is updated in the repository.</param>
		/// <returns>The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentEntity"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entities"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="getExistingEntityIdsDelegate"/> is null.</exception>
		public IEnumerable<CacheEntity<TEntity>> Persist(
			TParentEntity parentEntity,
			LazyEntities<TEntity> entities,
			Func<BinaryGuid, IEnumerable<BinaryGuid>> getExistingEntityIdsDelegate,
			IPersistingRepository<TParentEntity, TEntity> persistingRepository = null,
			IDeletingByIdDataConnector deletingByIdDataConnector = null,
			Action<TEntity> afterEntityAddedDelegate = null,
			Action<BinaryGuid> beforeEntityRemovedDelegate = null,
			Action<TEntity> afterEntityUpdatedDelegate = null)
		{
			parentEntity.ThrowIfNull("parentEntity");
			entities.ThrowIfNull("entities");
			getExistingEntityIdsDelegate.ThrowIfNull("getExistingEntityIdsDelegate");

			return entities.IsValueCreated
				       ? Persist(
					       parentEntity,
					       entities.Value,
					       getExistingEntityIdsDelegate,
					       persistingRepository,
					       deletingByIdDataConnector,
					       afterEntityAddedDelegate,
					       beforeEntityRemovedDelegate,
					       afterEntityUpdatedDelegate)
				       : null;
		}

		/// <summary>
		/// Persists entities related to <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity whose related entities are to be persisted.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		protected virtual void PersistRelated(TEntity entity)
		{
			entity.ThrowIfNull("entity");
		}
	}
}