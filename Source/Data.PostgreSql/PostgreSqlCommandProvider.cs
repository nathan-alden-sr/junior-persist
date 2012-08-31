using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Junior.Common;

using Npgsql;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// Retrieves <see cref="NpgsqlCommand"/> instances.
	/// </summary>
	public class NpgsqlCommandProvider : ICommandProvider<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter>
	{
		private readonly ICommandTimeoutProvider _commandTimeoutProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="NpgsqlCommandProvider"/> class.
		/// </summary>
		/// <param name="commandTimeoutProvider">A command timeout provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandTimeoutProvider"/> is null.</exception>
		public NpgsqlCommandProvider(ICommandTimeoutProvider commandTimeoutProvider)
		{
			commandTimeoutProvider.ThrowIfNull("commandTimeoutProvider");

			_commandTimeoutProvider = commandTimeoutProvider;
		}

		/// <summary>
		/// Retrieves a <see cref="NpgsqlCommand"/> instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public NpgsqlCommand GetCommand(string connectionKey, NpgsqlConnection connection, string sql, params NpgsqlParameter[] parameters)
		{
			connection.ThrowIfNull("connection");
			connection.ThrowIfNull("sql");

			return GetCommand(connectionKey, connection, sql, (IEnumerable<NpgsqlParameter>)parameters);
		}

		/// <summary>
		/// Retrieves a <see cref="NpgsqlCommand"/> instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public NpgsqlCommand GetCommand(string connectionKey, NpgsqlConnection connection, string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			connection.ThrowIfNull("connection");
			connection.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<NpgsqlParameter>();

			var command = new NpgsqlCommand(sql, connection)
				{
					CommandTimeout = (int)_commandTimeoutProvider.GetTimeout(connectionKey).TotalSeconds,
					CommandType = CommandType.Text
				};

			foreach (NpgsqlParameter parameter in parameters)
			{
				command.Parameters.Add(parameter);
			}
			command.Prepare();

			return command;
		}
	}
}