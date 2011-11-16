using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// A non-transactional session that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public class NonTransactionalCacheKeySession : CacheKeySession<NonTransactionalCacheKeySession>, INonTransactionalCacheKeySession
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NonTransactionalCacheKeySession"/> class.
		/// </summary>
		internal NonTransactionalCacheKeySession()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NonTransactionalCacheKeySession"/> class.
		/// </summary>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		internal NonTransactionalCacheKeySession(ISessionObserver<CacheKey, object> observer)
			: base(observer)
		{
		}
	}
}