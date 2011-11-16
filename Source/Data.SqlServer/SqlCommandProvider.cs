using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// Retrieves <see cref="SqlCommand"/> instances.
	/// </summary>
	public class SqlCommandProvider : ICommandProvider<SqlConnection, SqlCommand, SqlParameter>
	{
		private readonly ICommandTimeoutProvider _commandTimeoutProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlCommandProvider"/> class.
		/// </summary>
		/// <param name="commandTimeoutProvider">A command timeout provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandTimeoutProvider"/> is null.</exception>
		public SqlCommandProvider(ICommandTimeoutProvider commandTimeoutProvider)
		{
			commandTimeoutProvider.ThrowIfNull("commandTimeoutProvider");

			_commandTimeoutProvider = commandTimeoutProvider;
		}

		/// <summary>
		/// Retrieves a <see cref="SqlCommand"/> instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public SqlCommand GetCommand(string connectionKey, SqlConnection connection, string sql, params SqlParameter[] parameters)
		{
			connection.ThrowIfNull("connection");
			connection.ThrowIfNull("sql");

			return GetCommand(connectionKey, connection, sql, (IEnumerable<SqlParameter>)parameters);
		}

		/// <summary>
		/// Retrieves a <see cref="SqlCommand"/> instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public SqlCommand GetCommand(string connectionKey, SqlConnection connection, string sql, IEnumerable<SqlParameter> parameters)
		{
			connection.ThrowIfNull("connection");
			connection.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<SqlParameter>();

			var command = new SqlCommand(sql, connection)
			              	{
			              		CommandTimeout = (int)_commandTimeoutProvider.GetTimeout(connectionKey).TotalSeconds,
			              		CommandType = CommandType.Text
			              	};

			command.Parameters.AddRange(parameters.ToArray());

			return command;
		}
	}
}