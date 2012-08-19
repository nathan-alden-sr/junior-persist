using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Ddd.DomainModel;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Base class for all session managers. <see cref="SessionManager{TCacheKey,TEntity,TSession}"/> provides current session context,
	/// delegates method calls to the current session context and provides for session enrollment.
	/// </summary>
	/// <typeparam name="TCacheKey"></typeparam>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TSession"></typeparam>
	public abstract class SessionManager<TCacheKey, TEntity, TSession> : ISessionManager<TCacheKey, TEntity, TSession>
		where TCacheKey : class
		where TEntity : class
		where TSession : class, ISession<TCacheKey, TEntity>
	{
		/// <summary>
		/// Gets the current session context.
		/// </summary>
		protected abstract TSession Current
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating if there is a session context.
		/// </summary>
		public bool HasSession
		{
			get
			{
				return Current != null;
			}
		}

		/// <summary>
		/// Notifies the current session cache that an entity was persisted.
		/// </summary>
		/// <param name="cacheEntity">The persisted entity as a <see cref="CacheEntity{TEntity}"/> instance.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void EntityWasPersisted(CacheEntity<TEntity> cacheEntity)
		{
			ValidateSession();

			Current.EntityWasPersisted(cacheEntity);
		}

		/// <summary>
		/// Notifies the current session cache that an entity was found.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <param name="cacheEntity">The persisted entity as a <see cref="CacheEntity{TEntity}"/> instance.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void EntityWasFound(TCacheKey cacheKey, CacheEntity<TEntity> cacheEntity)
		{
			ValidateSession();

			Current.EntityWasFound(cacheKey, cacheEntity);
		}

		/// <summary>
		/// Notifies the current session cache that a lazy entity was created.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <param name="entityId">The ID associated with the entity to be lazy-loaded.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void LazyEntityWasCreated(ILazyEntity<TEntity> lazyEntity, Guid entityId)
		{
			ValidateSession();

			Current.LazyEntityWasCreated(lazyEntity, entityId);
		}

		/// <summary>
		/// Notifies the current session cache that multiple entities were found.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <param name="cacheEntities">The persisted entities as <see cref="CacheEntity{TEntity}"/> instances.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void EntitiesWereFound(TCacheKey cacheKey, IEnumerable<CacheEntity<TEntity>> cacheEntities)
		{
			ValidateSession();

			Current.EntitiesWereFound(cacheKey, cacheEntities);
		}

		/// <summary>
		/// Instructs the current session cache to remove an entity from the cache.
		/// </summary>
		/// <param name="entityId">The ID of the entity to remove.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void RemoveEntity(Guid entityId)
		{
			ValidateSession();

			Current.RemoveEntity(entityId);
		}

		/// <summary>
		/// Instructs the current session cache to remove an entity from the cache.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void RemoveEntity(TEntity entity)
		{
			ValidateSession();

			Current.RemoveEntity(entity);
		}

		/// <summary>
		/// Instructs the current session cache to remove entities from the cache.
		/// </summary>
		/// <param name="entities">The entities to remove.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void RemoveEntities(IEnumerable<TEntity> entities)
		{
			ValidateSession();

			Current.RemoveEntities(entities);
		}

		/// <summary>
		/// Retrieves a cached entity's ID from the current session cache.
		/// </summary>
		/// <param name="entity">A cached entity.</param>
		/// <returns>The cached entity's ID.</returns>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public Guid GetEntityId(TEntity entity)
		{
			ValidateSession();

			return Current.GetEntityId(entity);
		}

		/// <summary>
		/// Retrieves a cached entity's ID from the current session cache. If the entity is not already cached, then <paramref name="defaultEntityId"/> is returned.
		/// </summary>
		/// <param name="entity">A cached entity.</param>
		/// <param name="defaultEntityId">The ID to return if <paramref name="entity"/> is not cached.</param>
		/// <returns>The cached entity's ID if the entity is cached; otherwise, <paramref name="defaultEntityId"/>.</returns>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public Guid GetEntityId(TEntity entity, Guid defaultEntityId)
		{
			ValidateSession();

			return Current.GetEntityId(entity, defaultEntityId);
		}

		/// <summary>
		/// Retrieves the ID of a lazy-loaded entity from the current session cache.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <returns>The ID of the lazy-loaded entity.</returns>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public Guid GetEntityId(ILazyEntity<TEntity> lazyEntity)
		{
			ValidateSession();

			return Current.GetEntityId(lazyEntity);
		}

		/// <summary>
		/// Retrieves a cached entity from the current session cache as a <see cref="CacheEntity{TEntity}"/> instance.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <returns>The cached entity as a <see cref="CacheEntity{TEntity}"/> instance.</returns>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public CacheEntity<TEntity> GetEntity(TCacheKey cacheKey)
		{
			ValidateSession();

			return Current.GetEntity(cacheKey);
		}

		/// <summary>
		/// Retrieves cached entities from the current session cache as <see cref="CacheEntity{TEntity}"/> instances.
		/// </summary>
		/// <param name="cacheKey">The cache key for the related query.</param>
		/// <returns>The cached entitys as <see cref="CacheEntity{TEntity}"/> instances.</returns>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public IEnumerable<CacheEntity<TEntity>> GetEntities(TCacheKey cacheKey)
		{
			ValidateSession();

			return Current.GetEntities(cacheKey);
		}

		/// <summary>
		/// Instructs the cache to remove all entities of type <typeparamref name="TEntityToClear"/> from the current session cache.
		/// </summary>
		/// <typeparam name="TEntityToClear">The type of entity to remove from the cache.</typeparam>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void Clear<TEntityToClear>()
			where TEntityToClear : TEntity
		{
			ValidateSession();

			Current.Clear<TEntityToClear>();
		}

		/// <summary>
		/// Instructs the cache to remove all entities of the specified type from the current session cache.
		/// </summary>
		/// <param name="entityType">The type of entity to remove from the cache.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void Clear(Type entityType)
		{
			entityType.ThrowIfNull("entityType");

			ValidateSession();

			Current.Clear(entityType);
		}

		/// <summary>
		/// Instructs the cache to remove all entities from the current session cache.
		/// </summary>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void ClearAll()
		{
			ValidateSession();

			Current.ClearAll();
		}

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <returns>A session.</returns>
		public abstract TSession Enroll();

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		/// <returns>A session.</returns>
		public abstract TSession Enroll(ISessionObserver<TCacheKey, TEntity> observer);

		/// <summary>
		/// Validates that a session context is present.
		/// </summary>
		/// <exception cref="SessionException">Thrown when no session context is present.</exception>
		protected void ValidateSession()
		{
			if (!HasSession)
			{
				throw new SessionException("No session context found.");
			}
		}
	}
}