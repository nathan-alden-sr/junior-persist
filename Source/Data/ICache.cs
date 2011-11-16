using System;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Represents a way to determine if a cache key exists in a cache.
	/// </summary>
	/// <typeparam name="TKey">The type of cache key.</typeparam>
	public interface ICache<in TKey>
		where TKey : IComparable<TKey>
	{
		/// <summary>
		/// Determines if the specified cache key has been cached.
		/// </summary>
		/// <param name="key">A cache key.</param>
		/// <returns>true if the specified cache key has been cached; otherwise, false.</returns>
		bool IsCached(TKey key);
	}
}