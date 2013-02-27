using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Data.MySql;
using Junior.Persist.Persistence.Repositories;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.MySql.Repositories
{
	/// <summary>
	/// A repository that persists, deletes and caches entities using <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public abstract class PersistingDeletingCacheKeyCachingRepository<TEntity, TEntityData, TDataConnector> : PersistingCacheKeyCachingRepository<TEntity, TEntityData, TDataConnector>, IDeletingRepository<TEntity>
		where TEntity : class
		where TEntityData : class, IEntityData
		where TDataConnector : class, IInsertingOrUpdatingDataConnector<TEntityData>, IDeletingByIdDataConnector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PersistingDeletingCacheKeyCachingRepository{TEntity,TEntityData,TDataConnector}"/> class.
		/// </summary>
		/// <param name="sessionCache">The session cache to use when caching entities.</param>
		/// <param name="entityIdFactory">Generates new entity IDs when entities that are not in the cache are persisted.</param>
		/// <param name="dataConnector">A data connector that writes entity data.</param>
		/// <param name="entityDataMappingDelegate">A delegate that converts an entity into entity data.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sessionCache"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityIdFactory"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="dataConnector"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityDataMappingDelegate"/> is null.</exception>
		protected PersistingDeletingCacheKeyCachingRepository(
			ISessionCache<CacheKey, object> sessionCache,
			IEntityIdFactory entityIdFactory,
			TDataConnector dataConnector,
			Func<CacheEntity<TEntity>, TEntityData> entityDataMappingDelegate)
			: base(sessionCache, entityIdFactory, dataConnector, entityDataMappingDelegate)
		{
		}

		/// <summary>
		/// Gets the entity types that will be cleared from the cache when deleting entities with this repository.
		/// </summary>
		protected abstract IEnumerable<Type> RelatedEntityTypesToClearFromCache
		{
			get;
		}

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public async Task Delete(TEntity entity)
		{
			entity.ThrowIfNull("entity");

			Guid entityId = SessionCache.GetEntityId(entity);

			await DataConnector.DeleteById(entityId);

			SessionCache.RemoveEntity(entity);

			ClearRelatedEntities();
		}

		private void ClearRelatedEntities()
		{
			IEnumerable<Type> entityTypes = RelatedEntityTypesToClearFromCache ?? Enumerable.Empty<Type>();

			foreach (Type entityType in entityTypes)
			{
				SessionCache.Clear(entityType);
			}
		}
	}

	/// <summary>
	/// A repository that persists, deletes and caches entities by parent using <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public abstract class PersistingDeletingCacheKeyCachingRepository<TParentEntity, TEntity, TEntityData, TDataConnector> : PersistingCacheKeyCachingRepository<TParentEntity, TEntity, TEntityData, TDataConnector>, IDeletingRepository<TEntity>
		where TParentEntity : class
		where TEntity : class
		where TEntityData : class, IEntityData
		where TDataConnector : class, IInsertingOrUpdatingDataConnector<TEntityData>, IDeletingByIdDataConnector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PersistingDeletingCacheKeyCachingRepository{TEntity,TEntityData,TDataConnector}"/> class.
		/// </summary>
		/// <param name="sessionCache">The session cache to use when caching entities.</param>
		/// <param name="entityIdFactory">Generates new entity IDs when entities that are not in the cache are persisted.</param>
		/// <param name="dataConnector">A data connector that writes entity data.</param>
		/// <param name="parentEntityDataMappingDelegate">A delegate that converts an entity and its parent relationship into entity data.</param>
		protected PersistingDeletingCacheKeyCachingRepository(
			ISessionCache<CacheKey, object> sessionCache,
			IEntityIdFactory entityIdFactory,
			TDataConnector dataConnector,
			Func<TParentEntity, CacheEntity<TEntity>, TEntityData> parentEntityDataMappingDelegate)
			: base(sessionCache, entityIdFactory, dataConnector, parentEntityDataMappingDelegate)
		{
		}

		/// <summary>
		/// Gets the entity types that will be cleared from the cache when deleting entities with this repository.
		/// </summary>
		protected abstract IEnumerable<Type> RelatedEntityTypesToClearFromCache
		{
			get;
		}

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public async Task Delete(TEntity entity)
		{
			entity.ThrowIfNull("entity");

			Guid entityId = SessionCache.GetEntityId(entity);

			await DataConnector.DeleteById(entityId);

			SessionCache.RemoveEntity(entity);

			ClearRelatedEntities();
		}

		private void ClearRelatedEntities()
		{
			IEnumerable<Type> entityTypes = RelatedEntityTypesToClearFromCache ?? Enumerable.Empty<Type>();

			foreach (Type entityType in entityTypes)
			{
				SessionCache.Clear(entityType);
			}
		}
	}
}