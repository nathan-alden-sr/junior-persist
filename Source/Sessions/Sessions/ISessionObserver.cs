using System;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Represents actions observed in a session.
	/// </summary>
	public interface ISessionObserver<out TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
	{
		/// <summary>
		/// Invoked when an entity was persisted.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		void EntityPersisted(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId);

		/// <summary>
		/// Invoked when an entity was found.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		void EntityFound(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId);

		/// <summary>
		/// Invoked when an entity was removed from the cache.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		void EntityRemoved(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId);
	}
}