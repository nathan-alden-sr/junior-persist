using System;
using System.Collections.Generic;

using Junior.Ddd.DomainModel;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Represents a session-based entity cache.
	/// </summary>
	public interface ISessionCache<in TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
	{
		/// <summary>
		/// Notifies the cache that an entity was persisted.
		/// </summary>
		/// <param name="cacheEntity">The persisted entity as a <see cref="CacheEntity{TEntity}"/> instance.</param>
		void EntityWasPersisted(CacheEntity<TEntity> cacheEntity);

		/// <summary>
		/// Notifies the cache that an entity was found.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <param name="cacheEntity">The persisted entity as a <see cref="CacheEntity{TEntity}"/> instance.</param>
		void EntityWasFound(TCacheKey cacheKey, CacheEntity<TEntity> cacheEntity);

		/// <summary>
		/// Notifies the cache that a lazy entity was created.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <param name="entityId">The ID associated with the entity to be lazy-loaded.</param>
		void LazyEntityWasCreated(ILazyEntity<TEntity> lazyEntity, Guid entityId);

		/// <summary>
		/// Notifies the cache that multiple entities were found.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <param name="cacheEntities">The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</param>
		void EntitiesWereFound(TCacheKey cacheKey, IEnumerable<CacheEntity<TEntity>> cacheEntities);

		/// <summary>
		/// Instructs the cache to remove an entity from the cache.
		/// </summary>
		/// <param name="entityId">The ID of the entity to remove.</param>
		void RemoveEntity(Guid entityId);

		/// <summary>
		/// Instructs the cache to remove an entity from the cache.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		void RemoveEntity(TEntity entity);

		/// <summary>
		/// Instructs the cache to remove entities from the cache.
		/// </summary>
		/// <param name="entities">The entities to remove.</param>
		void RemoveEntities(IEnumerable<TEntity> entities);

		/// <summary>
		/// Retrieves a cached entity's ID.
		/// </summary>
		/// <param name="entity">A cached entity.</param>
		/// <returns>The cached entity's ID.</returns>
		Guid GetEntityId(TEntity entity);

		/// <summary>
		/// Retrieves a cached entity's ID. If the entity is not already cached, then <paramref name="defaultEntityId"/> is returned.
		/// </summary>
		/// <param name="entity">A cached entity.</param>
		/// <param name="defaultEntityId">The ID to return if <paramref name="entity"/> is not cached.</param>
		/// <returns>The cached entity's ID if the entity is cached; otherwise, <paramref name="defaultEntityId"/>.</returns>
		Guid GetEntityId(TEntity entity, Guid defaultEntityId);

		/// <summary>
		/// Retrieves the ID of a lazy-loaded entity.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <returns>The ID of the lazy-loaded entity.</returns>
		Guid GetEntityId(ILazyEntity<TEntity> lazyEntity);

		/// <summary>
		/// Retrieves a cached entity from the cache as a <see cref="CacheEntity{TEntity}"/> instance.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <returns>The cached entity as a <see cref="CacheEntity{TEntity}"/> instance.</returns>
		CacheEntity<TEntity> GetEntity(TCacheKey cacheKey);

		/// <summary>
		/// Retrieves cached entities from the cache as <see cref="CacheEntity{TEntity}"/> instances.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <returns>The cached entitys as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		IEnumerable<CacheEntity<TEntity>> GetEntities(TCacheKey cacheKey);

		/// <summary>
		/// Instructs the cache to remove all entities of type <typeparamref name="TEntityToClear"/> from the cache.
		/// </summary>
		/// <typeparam name="TEntityToClear">The type of entity to remove from the cache.</typeparam>
		void Clear<TEntityToClear>()
			where TEntityToClear : TEntity;

		/// <summary>
		/// Instructs the cache to remove all entities of the specified type from the cache.
		/// </summary>
		/// <param name="entityType">The type of entity to remove from the cache.</param>
		void Clear(Type entityType);

		/// <summary>
		/// Instructs the cache to remove all entities from the cache.
		/// </summary>
		void ClearAll();
	}
}