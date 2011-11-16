using System.Collections.Generic;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Represents a way to retrieve instances of a class that can execute SQL commands.
	/// </summary>
	/// <typeparam name="TConnection">A connection class type.</typeparam>
	/// <typeparam name="TCommand">A command class type.</typeparam>
	/// <typeparam name="TParameter">A parameter class type.</typeparam>
	public interface ICommandProvider<in TConnection, out TCommand, in TParameter>
		where TConnection : class
		where TCommand : class
		where TParameter : class
	{
		/// <summary>
		/// Retrieves a command class instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		TCommand GetCommand(string connectionKey, TConnection connection, string sql, params TParameter[] parameters);

		/// <summary>
		/// Retrieves a command class instance for the specified connection key, connection, SQL statement and parameters.
		/// </summary>
		/// <param name="connectionKey">A connection key. <paramref name="connectionKey"/> is used to retrieve the command timeout.</param>
		/// <param name="connection">A connection.</param>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A command.</returns>
		TCommand GetCommand(string connectionKey, TConnection connection, string sql, IEnumerable<TParameter> parameters);
	}
}