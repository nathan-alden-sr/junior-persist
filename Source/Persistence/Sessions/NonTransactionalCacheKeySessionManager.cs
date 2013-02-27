using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// A non-transactional session manager that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public class NonTransactionalCacheKeySessionManager : CacheKeySessionManager<INonTransactionalCacheKeySession>, INonTransactionalCacheKeySessionManager
	{
		/// <summary>
		/// Gets the current session context.
		/// </summary>
		protected override INonTransactionalCacheKeySession Current
		{
			get
			{
				return NonTransactionalCacheKeySession.Current;
			}
		}

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <returns>A session.</returns>
		public override Task<INonTransactionalCacheKeySession> Enroll()
		{
			return Task.FromResult<INonTransactionalCacheKeySession>(new NonTransactionalCacheKeySession());
		}

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		/// <returns>A session.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
		public override Task<INonTransactionalCacheKeySession> Enroll(ISessionObserver<CacheKey, object> observer)
		{
			observer.ThrowIfNull("observer");

			return Task.FromResult<INonTransactionalCacheKeySession>(new NonTransactionalCacheKeySession(observer));
		}
	}
}