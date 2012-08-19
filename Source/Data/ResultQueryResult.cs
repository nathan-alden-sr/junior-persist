using System;

using Junior.Common;

namespace Junior.Persist.Data
{
	/// <summary>
	/// A esult returned from an executed SQL query.
	/// </summary>
	public class ResultQueryResult<TResult> : IQueryResult<TResult>
		where TResult : class
	{
		private readonly CacheKey _cacheKey;
		private readonly TResult _result;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResultQueryResult{TResult}"/> class.
		/// </summary>
		/// <param name="cacheKey">The cache key for the query.</param>
		/// <param name="result">The query result.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheKey"/> is null.</exception>
		public ResultQueryResult(CacheKey cacheKey, TResult result)
		{
			cacheKey.ThrowIfNull("cacheKey");

			_cacheKey = cacheKey;
			_result = result;
		}

		/// <summary>
		/// Gets false.
		/// </summary>
		public bool UseCache
		{
			get
			{
				return false;
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
		/// Gets the query result.
		/// </summary>
		public TResult Result
		{
			get
			{
				return _result;
			}
		}
	}
}