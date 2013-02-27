using System.Threading.Tasks;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Represents a way to retrieve instances of a class that can connect to databases.
	/// </summary>
	/// <typeparam name="TConnection">A connection class type.</typeparam>
	public interface IConnectionProvider<TConnection>
		where TConnection : class
	{
		/// <summary>
		/// Retrieves a connection class instance for the specified connection key.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="openConnection">Indicates if the connection should be opened before it is returned.</param>
		/// <returns>A connection.</returns>
		Task<TConnection> GetConnection(string connectionKey, bool openConnection = true);
	}
}