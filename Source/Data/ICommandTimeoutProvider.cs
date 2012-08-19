using System;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Represents a way to retrieve command timeouts for different database connections.
	/// </summary>
	public interface ICommandTimeoutProvider
	{
		/// <summary>
		/// Retrieves a command timeout for the specified connection key.
		/// </summary>
		/// <param name="connectionKey">A key whose timeout will be retrieved.</param>
		/// <returns>A command timeout.</returns>
		TimeSpan GetTimeout(string connectionKey);
	}
}