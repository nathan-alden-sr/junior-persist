using System;

using Junior.Common;

using Npgsql;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// Retrieves <see cref="NpgsqlConnection"/> instances.
	/// </summary>
	public class NpgsqlConnectionProvider : IConnectionProvider<NpgsqlConnection>
	{
		private readonly IConnectionStringProvider _connectionStringProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="NpgsqlCommandProvider"/> class.
		/// </summary>
		/// <param name="connectionStringProvider">A connection string provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionStringProvider"/> is null.</exception>
		public NpgsqlConnectionProvider(IConnectionStringProvider connectionStringProvider)
		{
			connectionStringProvider.ThrowIfNull("connectionStringProvider");

			_connectionStringProvider = connectionStringProvider;
		}

		/// <summary>
		/// Retrieves a <see cref="NpgsqlConnection"/> instance for the specified connection key.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="openConnection">Indicates if the connection should be opened before it is returned.</param>
		/// <returns>A connection.</returns>
		public NpgsqlConnection GetConnection(string connectionKey, bool openConnection = true)
		{
			connectionKey.ThrowIfNull("connectionKey");

			var connection = new NpgsqlConnection(_connectionStringProvider.ByKey(connectionKey));

			if (openConnection)
			{
				connection.Open();
			}

			return connection;
		}
	}
}