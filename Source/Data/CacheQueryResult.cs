using System;

using Junior.Common;

namespace Junior.Persist.Data
{
	/// <summary>
	/// A query result that indicates that the results should be retrieved from a cache.
	/// </summary>
	public class CacheQueryResult<TResult> : IQueryResult<TResult>
		where TResult : class
	{
		private readonly CacheKey _cacheKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheQueryResult{TResult}"/> class.
		/// </summary>
		/// <param name="cacheKey">The cache key for the query.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheKey"/> is null.</exception>
		public CacheQueryResult(CacheKey cacheKey)
		{
			cacheKey.ThrowIfNull("cacheKey");

			_cacheKey = cacheKey;
		}

		/// <summary>
		/// Gets true.
		/// </summary>
		public bool UseCache
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets the cache key for the query.
		/// </summary>
		public CacheKey CacheKey
		{
			get
			{
				return _cacheKey;
			}
		}

		/// <summary>
		/// Gets null, since the query result should be read from a cache.
		/// </summary>
		public TResult Result
		{
			get
			{
				return null;
			}
		}
	}
}