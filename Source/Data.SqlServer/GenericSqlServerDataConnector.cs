using System;
using System.Data.SqlClient;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// A SQL Server data connector that is provided with a specific connection string and command timeout.
	/// </summary>
	public abstract class GenericSqlServerDataConnector : SqlServerDataConnector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericSqlServerDataConnector"/> class with no command timeout.
		/// </summary>
		/// <param name="connectionString">A SQL Server connection string.</param>
		protected GenericSqlServerDataConnector(string connectionString)
			: this(connectionString, TimeSpan.Zero)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericSqlServerDataConnector"/> class.
		/// </summary>
		/// <param name="connectionString">A SQL Server connection string.</param>
		/// <param name="timeout">A command timeout.</param>
		protected GenericSqlServerDataConnector(string connectionString, TimeSpan timeout)
			: base(new ConnectionStringConnectionProvider(connectionString), new SqlCommandProvider(new TimeSpanCommandTimeoutProvider(timeout)), "")
		{
		}

		private class ConnectionStringConnectionProvider : IConnectionProvider<SqlConnection>
		{
			private readonly string _connectionString;

			public ConnectionStringConnectionProvider(string connectionString)
			{
				connectionString.ThrowIfNull("connectionString");

				_connectionString = connectionString;
			}

			public SqlConnection GetConnection(string connectionKey, bool openConnection = true)
			{
				var connection = new SqlConnection(_connectionString);

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