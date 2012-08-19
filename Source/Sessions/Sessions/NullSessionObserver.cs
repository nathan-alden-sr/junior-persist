using System;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Handles actions observed in a session by doing nothing.
	/// </summary>
	public class NullSessionObserver<TCacheKey, TEntity> : ISessionObserver<TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
	{
		/// <summary>
		/// Invoked when an entity was persisted. <see cref="EntityPersisted"/> does nothing.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		public void EntityPersisted(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId)
		{
		}

		/// <summary>
		/// Invoked when an entity was found. <see cref="EntityFound"/> does nothing.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		public void EntityFound(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId)
		{
		}

		/// <summary>
		/// Invoked when an entity was removed from the cache. <see cref="EntityRemoved"/> does nothing.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		public void EntityRemoved(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId)
		{
		}
	}
}