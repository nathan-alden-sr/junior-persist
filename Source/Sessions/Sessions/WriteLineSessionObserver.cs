using System;

using Junior.Common;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Handles actions observed in a session by writing a line to an <see cref="ILineWriter"/>.
	/// </summary>
	public abstract class WriteLineSessionObserver<TCacheKey, TEntity> : ISessionObserver<TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
	{
		private readonly ILineWriter _lineWriter;

		/// <summary>
		/// Initializes a new instance of the <see cref="WriteLineSessionObserver{TCacheKey,TEntity}"/> class.
		/// </summary>
		/// <param name="lineWriter">A line writer that will write observed actions.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="lineWriter"/> is null.</exception>
		protected WriteLineSessionObserver(ILineWriter lineWriter)
		{
			lineWriter.ThrowIfNull("lineWriter");

			_lineWriter = lineWriter;
		}

		/// <summary>
		/// Invoked when an entity was persisted. The entity's type, ID and related session will be written to the provided line writer.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="session"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityType"/> is null.</exception>
		public void EntityPersisted(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId)
		{
			session.ThrowIfNull("session");
			entityType.ThrowIfNull("entityType");

			_lineWriter.WriteLine(
				"Entity of type '{0}' with ID {1} persisted in session {2}",
				new object[]
					{
						entityType.FullName,
						session.SessionId,
						entityId
					});
		}

		/// <summary>
		/// Invoked when an entity was found. The entity's type, ID and related session will be written to the provided line writer.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="session"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityType"/> is null.</exception>
		public void EntityFound(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId)
		{
			session.ThrowIfNull("session");
			entityType.ThrowIfNull("entityType");

			_lineWriter.WriteLine(
				"Entity of type '{0}' with ID {1} found in session {2}",
				new object[]
					{
						entityType.FullName,
						session.SessionId,
						entityId
					});
		}

		/// <summary>
		/// Invoked when an entity was removed from the cache. The entity's type, ID and related session will be written to the provided line writer.
		/// </summary>
		/// <param name="session">The session in which the entity was persisted.</param>
		/// <param name="entityType">The persisted entity's type.</param>
		/// <param name="entityId">The persisted entity's ID.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="session"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityType"/> is null.</exception>
		public void EntityRemoved(ISession<TCacheKey, TEntity> session, Type entityType, Guid entityId)
		{
			session.ThrowIfNull("session");
			entityType.ThrowIfNull("entityType");

			_lineWriter.WriteLine(
				"Entity of type '{0}' with ID {1} removed from session {2}",
				new object[]
					{
						entityType.FullName,
						session.SessionId,
						entityId
					});
		}
	}
}