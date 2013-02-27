using System.Threading.Tasks;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Represents a way to enroll in a session.
	/// </summary>
	public interface ISessionManager<in TCacheKey, TEntity, TSession> : ISessionCache<TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
		where TSession : class, ISession<TCacheKey, TEntity>
	{
		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <returns>A session.</returns>
		Task<TSession> Enroll();

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		/// <returns>A session.</returns>
		Task<TSession> Enroll(ISessionObserver<TCacheKey, TEntity> observer);
	}
}