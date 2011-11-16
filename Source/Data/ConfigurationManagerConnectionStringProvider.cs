using System.Configuration;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Retrieve a connection string by key from the <see cref="ConfigurationManager.ConnectionStrings"/> array.
	/// </summary>
	public class ConfigurationManagerConnectionStringProvider : IConnectionStringProvider
	{
		/// <summary>
		/// Retrieves a connection string for the specified key.
		/// </summary>
		/// <param name="key">A key.</param>
		/// <returns>The connection string for the specified key.</returns>
		public string ByKey(string key)
		{
			return ConfigurationManager.ConnectionStrings[key].ConnectionString;
		}
	}
}