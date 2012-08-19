using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Junior.Common;

using MySql.Data.MySqlClient;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// Retrieves <see cref="MySqlCommand"/> instances.
	/// </summary>
	public class MySqlCommandProvider : ICommandProvider<MySqlConnection, MySqlCommand, MySqlParameter>
	{
		private readonly ICommandTimeoutProvider _commandTimeoutProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="MySqlCommandProvider"/> class.
		/// </summary>
		/// <param name="commandTimeoutProvider">A command timeout provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandTimeoutProvider"/> is null.</exception>
		public MySqlCommandProvider(ICommandTimeoutProvider commandTimeoutProvider)
		{
			commandTimeoutProvider.ThrowIfNull("commandTimeoutProvider");

			_commandTimeoutProvider = commandTimeoutProvider;
		}

		/// <summary>
		/// Retrieves a <see cref="MySqlCommand"/> instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public MySqlCommand GetCommand(string connectionKey, MySqlConnection connection, string sql, params MySqlParameter[] parameters)
		{
			connection.ThrowIfNull("connection");
			connection.ThrowIfNull("sql");

			return GetCommand(connectionKey, connection, sql, (IEnumerable<MySqlParameter>)parameters);
		}

		/// <summary>
		/// Retrieves a <see cref="MySqlCommand"/> instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public MySqlCommand GetCommand(string connectionKey, MySqlConnection connection, string sql, IEnumerable<MySqlParameter> parameters)
		{
			connection.ThrowIfNull("connection");
			connection.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<MySqlParameter>();

			var command = new MySqlCommand(sql, connection)
				{
					CommandTimeout = (int)_commandTimeoutProvider.GetTimeout(connectionKey).TotalSeconds,
					CommandType = CommandType.Text
				};

			foreach (MySqlParameter parameter in parameters)
			{
				command.Parameters.Add(parameter);
			}
			command.Prepare();

			return command;
		}
	}
}