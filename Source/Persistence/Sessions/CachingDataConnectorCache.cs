using System;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// A cache implementation used by data connectors to determine if an entity is already cached.
	/// <see cref="CachingDataConnectorCache"/> joins data connectors with the session cache indirectly.
	/// </summary>
	public class CachingDataConnectorCache : ICache<CacheKey>
	{
		private readonly ISessionCache<CacheKey, object> _sessionCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="CachingDataConnectorCache"/> class.
		/// </summary>
		/// <param name="sessionCache">A session cache.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sessionCache"/> is null.</exception>
		public CachingDataConnectorCache(ISessionCache<CacheKey, object> sessionCache)
		{
			sessionCache.ThrowIfNull("sessionCache");

			_sessionCache = sessionCache;
		}

		/// <summary>
		/// Determines if the specified cache key has been cached.
		/// </summary>
		/// <param name="key">A cache key.</param>
		/// <returns>true if the specified cache key has been cached; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
		public bool IsCached(CacheKey key)
		{
			key.ThrowIfNull("key");

			return _sessionCache.GetEntity(key) != null;
		}
	}
}