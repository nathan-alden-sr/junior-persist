namespace Junior.Persist.Data
{
	/// <summary>
	/// Represents the results of a SQL query.
	/// </summary>
	public interface IQueryResult<out TResult>
	{
		/// <summary>
		/// Gets a value indicating if the result should be retrieved from a cache instead of the <see cref="Result"/> property.
		/// </summary>
		bool UseCache
		{
			get;
		}
		/// <summary>
		/// Gets the cache key for the query.
		/// </summary>
		CacheKey CacheKey
		{
			get;
		}
		/// <summary>
		/// Gets the query result if <see cref="UseCache"/> is true; otherwise, gets null.
		/// </summary>
		TResult Result
		{
			get;
		}
	}
}