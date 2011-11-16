using System;

using Junior.Common;

using MySql.Data.MySqlClient;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// Retrieves <see cref="MySqlConnection"/> instances.
	/// </summary>
	public class MySqlConnectionProvider : IConnectionProvider<MySqlConnection>
	{
		private readonly IConnectionStringProvider _connectionStringProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="MySqlCommandProvider"/> class.
		/// </summary>
		/// <param name="connectionStringProvider">A connection string provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionStringProvider"/> is null.</exception>
		public MySqlConnectionProvider(IConnectionStringProvider connectionStringProvider)
		{
			connectionStringProvider.ThrowIfNull("connectionStringProvider");

			_connectionStringProvider = connectionStringProvider;
		}

		/// <summary>
		/// Retrieves a <see cref="MySqlConnection"/> instance for the specified connection key.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="openConnection">Indicates if the connection should be opened before it is returned.</param>
		/// <returns>A connection.</returns>
		public MySqlConnection GetConnection(string connectionKey, bool openConnection = true)
		{
			connectionKey.ThrowIfNull("connectionKey");

			var connection = new MySqlConnection(_connectionStringProvider.ByKey(connectionKey));

			if (openConnection)
			{
				connection.Open();
			}

			return connection;
		}
	}
}