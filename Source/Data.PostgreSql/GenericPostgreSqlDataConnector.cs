using System;

using Npgsql;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// A PostgreSQL data connector that is provided with a specific connection string and command timeout.
	/// </summary>
	public abstract class GenericPostgreSqlDataConnector : PostgreSqlDataConnector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericPostgreSqlDataConnector"/> class with no command timeout.
		/// </summary>
		/// <param name="connectionString">A PostgreSQL connection string.</param>
		protected GenericPostgreSqlDataConnector(string connectionString)
			: this(connectionString, TimeSpan.Zero)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericPostgreSqlDataConnector"/> class.
		/// </summary>
		/// <param name="connectionString">A PostgreSQL connection string.</param>
		/// <param name="timeout">A command timeout.</param>
		protected GenericPostgreSqlDataConnector(string connectionString, TimeSpan timeout)
			: base(new ConnectionStringConnectionProvider(connectionString), new NpgsqlCommandProvider(new TimeSpanCommandTimeoutProvider(timeout)), "")
		{
		}

		private class ConnectionStringConnectionProvider : IConnectionProvider<NpgsqlConnection>
		{
			private readonly string _connectionString;

			public ConnectionStringConnectionProvider(string connectionString)
			{
				_connectionString = connectionString;
			}

			public NpgsqlConnection GetConnection(string connectionKey, bool openConnection = true)
			{
				var connection = new NpgsqlConnection(_connectionString);

				if (openConnection)
				{
					connection.Open();
				}

				return connection;
			}
		}

		private class TimeSpanCommandTimeoutProvider : ICommandTimeoutProvider
		{
			private readonly TimeSpan _timeout;

			public TimeSpanCommandTimeoutProvider(TimeSpan timeout)
			{
				_timeout = timeout;
			}

			public TimeSpan GetTimeout(string connectionKey)
			{
				return _timeout;
			}
		}
	}
}