using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// Represents a transactional session manager that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public interface ITransactionalCacheKeySessionManager : ISessionManager<CacheKey, object, ITransactionalCacheKeySession>
	{
	}
}