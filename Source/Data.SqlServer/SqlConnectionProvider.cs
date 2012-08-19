using System;
using System.Data.SqlClient;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// Retrieves <see cref="SqlConnection"/> instances.
	/// </summary>
	public class SqlConnectionProvider : IConnectionProvider<SqlConnection>
	{
		private readonly IConnectionStringProvider _connectionStringProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlConnectionProvider"/> class.
		/// </summary>
		/// <param name="connectionStringProvider">A connection string provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionStringProvider"/> is null.</exception>
		public SqlConnectionProvider(IConnectionStringProvider connectionStringProvider)
		{
			connectionStringProvider.ThrowIfNull("connectionStringProvider");

			_connectionStringProvider = connectionStringProvider;
		}

		/// <summary>
		/// Retrieves a <see cref="SqlConnection"/> instance for the specified connection key.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="openConnection">Indicates if the connection should be opened before it is returned.</param>
		/// <returns>A connection.</returns>
		public SqlConnection GetConnection(string connectionKey, bool openConnection = true)
		{
			connectionKey.ThrowIfNull("connectionKey");

			var connection = new SqlConnection(_connectionStringProvider.ByKey(connectionKey));

			if (openConnection)
			{
				connection.Open();
			}

			return connection;
		}
	}
}