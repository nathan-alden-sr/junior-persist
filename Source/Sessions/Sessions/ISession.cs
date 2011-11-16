using System;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Represents a session. Sessions provide context for entity caching.
	/// </summary>
	public interface ISession<in TCacheKey, TEntity> : ISessionCache<TCacheKey, TEntity>, IDisposable
		where TCacheKey : class
		where TEntity : class
	{
		/// <summary>
		/// Gets the session's ID.
		/// </summary>
		Guid SessionId
		{
			get;
		}
	}
}