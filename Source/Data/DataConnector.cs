using System;

using Junior.Common;

namespace Junior.Persist.Data
{
	/// <summary>
	/// A base class for all data connectors. <see cref="DataConnector{TConnection,TCommand,TParameter}"/> provides a connection provider and
	/// a command provider to derived classes.
	/// </summary>
	/// <typeparam name="TConnection">A connection class type.</typeparam>
	/// <typeparam name="TCommand">A command class type.</typeparam>
	/// <typeparam name="TParameter">A parameter class type.</typeparam>
	public abstract class DataConnector<TConnection, TCommand, TParameter>
		where TConnection : class
		where TCommand : class
		where TParameter : class
	{
		private readonly ICommandProvider<TConnection, TCommand, TParameter> _commandProvider;
		private readonly IConnectionProvider<TConnection> _connectionProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataConnector{TConnection,TCommand,TParameter}"/> class.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionProvider"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandProvider"/> is null.</exception>
		protected DataConnector(IConnectionProvider<TConnection> connectionProvider, ICommandProvider<TConnection, TCommand, TParameter> commandProvider)
		{
			connectionProvider.ThrowIfNull("connectionProvider");
			commandProvider.ThrowIfNull("commandProvider");

			_connectionProvider = connectionProvider;
			_commandProvider = commandProvider;
		}

		/// <summary>
		/// Gets the connection provider associated with this data connector.
		/// </summary>
		protected IConnectionProvider<TConnection> ConnectionProvider
		{
			get
			{
				return _connectionProvider;
			}
		}

		/// <summary>
		/// Gets the command provider associated with this data connector.
		/// </summary>
		public ICommandProvider<TConnection, TCommand, TParameter> CommandProvider
		{
			get
			{
				return _commandProvider;
			}
		}
	}
}