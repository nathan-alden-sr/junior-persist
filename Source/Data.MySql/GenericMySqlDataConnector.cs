using System;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// A MySQL data connector that is provided with a specific connection string and command timeout.
	/// </summary>
	public abstract class GenericMySqlDataConnector : MySqlDataConnector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericMySqlDataConnector"/> class with no command timeout.
		/// </summary>
		/// <param name="connectionString">A MySQL connection string.</param>
		protected GenericMySqlDataConnector(string connectionString)
			: this(connectionString, TimeSpan.Zero)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericMySqlDataConnector"/> class.
		/// </summary>
		/// <param name="connectionString">A MySQL connection string.</param>
		/// <param name="timeout">A command timeout.</param>
		protected GenericMySqlDataConnector(string connectionString, TimeSpan timeout)
			: base(new ConnectionStringConnectionProvider(connectionString), new MySqlCommandProvider(new TimeSpanCommandTimeoutProvider(timeout)), "")
		{
		}

		private class ConnectionStringConnectionProvider : IConnectionProvider<MySqlConnection>
		{
			private readonly string _connectionString;

			public ConnectionStringConnectionProvider(string connectionString)
			{
				_connectionString = connectionString;
			}

			public async Task<MySqlConnection> GetConnection(string connectionKey, bool openConnection = true)
			{
				var connection = new MySqlConnection(_connectionString);

				if (openConnection)
				{
					await Task.Run(() => connection.Open());
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