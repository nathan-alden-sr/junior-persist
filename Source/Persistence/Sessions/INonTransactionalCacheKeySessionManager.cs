using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// Represents a non-transactional session manager that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public interface INonTransactionalCacheKeySessionManager : ISessionManager<CacheKey, object, INonTransactionalCacheKeySession>
	{
	}
}