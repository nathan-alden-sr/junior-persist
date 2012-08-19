using System;

using Junior.Ddd.DomainModel;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// A session manager that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public abstract class CacheKeySessionManager<TSession> : SessionManager<CacheKey, object, TSession>
		where TSession : class, ISession<CacheKey, object>
	{
		/// <summary>
		/// Notifies the cache that a lazy entity was created.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <param name="entityId">The ID associated with the entity to be lazy-loaded.</param>
		/// <exception cref="SessionException">Thrown when there is no session context.</exception>
		public void LazyEntityWasCreated<TEntity>(ILazyEntity<TEntity> lazyEntity, Guid entityId)
			where TEntity : class
		{
			ValidateSession();

			Current.LazyEntityWasCreated(lazyEntity, entityId);
		}

		/// <summary>
		/// Retrieves the ID of a lazy-loaded entity.
		/// </summary>
		/// <param name="lazyEntity">A lazy entity.</param>
		/// <returns>The ID of the lazy-loaded entity.</returns>
		public Guid GetEntityId<TEntity>(ILazyEntity<TEntity> lazyEntity)
			where TEntity : class
		{
			ValidateSession();

			return Current.GetEntityId(lazyEntity);
		}
	}
}