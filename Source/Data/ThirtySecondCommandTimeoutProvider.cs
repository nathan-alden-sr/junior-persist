using System;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Retrieves command timeouts for different database connections.
	/// </summary>
	public class ThirtySecondCommandTimeoutProvider : ICommandTimeoutProvider
	{
		/// <summary>
		/// Retrieves a command timeout for the specified connection key.
		/// </summary>
		/// <param name="connectionKey">A key whose timeout will be retrieved.</param>
		/// <returns>A command timeout.</returns>
		public TimeSpan GetTimeout(string connectionKey)
		{
			return TimeSpan.FromSeconds(30);
		}
	}
}