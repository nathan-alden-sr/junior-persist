using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// Represents a transactional session that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public interface ITransactionalCacheKeySession : ISession<CacheKey, object>
	{
		/// <summary>
		/// Commits the current transaction.
		/// </summary>
		void Commit();
	}
}