namespace Junior.Persist.Data
{
	/// <summary>
	/// Represents a way to retrieve a connection string by key.
	/// </summary>
	public interface IConnectionStringProvider
	{
		/// <summary>
		/// Retrieves a connection string for the specified key.
		/// </summary>
		/// <param name="key">A key.</param>
		/// <returns>The connection string for the specified key.</returns>
		string ByKey(string key);
	}
}