using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Junior.Common;

using Npgsql;

using NpgsqlTypes;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// A PostgreSQL data connector.
	/// </summary>
	public abstract class PostgreSqlDataConnector : DataConnector<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter>
	{
		private readonly string _connectionKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="PostgreSqlDataConnector"/> type.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected PostgreSqlDataConnector(
			IConnectionProvider<NpgsqlConnection> connectionProvider,
			ICommandProvider<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter> commandProvider,
			string connectionKey)
			: base(connectionProvider, commandProvider)
		{
			connectionKey.ThrowIfNull("connectionKey");

			_connectionKey = connectionKey;
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <param name="type">The provider's parameter type.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter<T>(string parameterName, T value, NpgsqlDbType type)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, type)
				{
					Value = GetSqlParameterValue(value)
				};
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <param name="type">The provider's parameter type.</param>
		/// <param name="size">The parameter's size.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter<T>(string parameterName, T value, NpgsqlDbType type, int size)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, type, size)
				{
					Value = GetSqlParameterValue(value)
				};
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <param name="type">The provider's parameter type.</param>
		/// <param name="size">The parameter's size.</param>
		/// <param name="precision">The parameter's precision.</param>
		/// <param name="scale">The parameter's scale.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter<T>(string parameterName, T value, NpgsqlDbType type, int size, byte precision, byte scale)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, type, size)
				{
					Precision = precision,
					Scale = scale,
					Value = GetSqlParameterValue(value)
				};
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <param name="type">The provider's parameter type.</param>
		/// <param name="precision">The parameter's precision.</param>
		/// <param name="scale">The parameter's scale.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter<T>(string parameterName, T value, NpgsqlDbType type, byte precision, byte scale)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, type)
				{
					Precision = precision,
					Scale = scale,
					Value = GetSqlParameterValue(value)
				};
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter(string parameterName, BinaryGuid value)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, (byte[])value);
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter(string parameterName, PreciseDateTime value)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, (long)value);
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter(string parameterName, BinaryGuid? value)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, GetSqlParameterValue((byte[])value));
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected NpgsqlParameter GetParameter(string parameterName, PreciseDateTime? value)
		{
			parameterName.ThrowIfNull("parameterName");

			return new NpgsqlParameter(parameterName, GetSqlParameterValue((long?)value));
		}

		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>The number of rows affected by the SQL statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<int> ExecuteNonQuery(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteNonQuery(sql, (IEnumerable<NpgsqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>The number of rows affected by the SQL statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<int> ExecuteNonQuery(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<NpgsqlParameter>();

			using (NpgsqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey))
			{
				string formattedSql = FormatSql(sql);
				NpgsqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);

				return command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executes a SQL statement that returns a scalar value.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A scalar value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<T> ExecuteScalar<T>(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteScalar<T>(sql, (IEnumerable<NpgsqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a SQL statement that returns a scalar value.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A scalar value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<T> ExecuteScalar<T>(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<NpgsqlParameter>();

			using (NpgsqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey))
			{
				string formattedSql = FormatSql(sql);
				NpgsqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);
				object value = command.ExecuteScalar();

				if (value == null || value == DBNull.Value)
				{
					if (typeof(T).IsValueType)
					{
						throw new Exception("Query resulted in NULL but scalar type is a value type.");
					}

					return default(T);
				}

				return (T)value;
			}
		}

		/// <summary>
		/// Executes a SQL statement and returns a data reader.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A data reader.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<NpgsqlDataReader> ExecuteReader(string sql, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteReader(sql, (IEnumerable<NpgsqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a SQL statement and returns a data reader.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A data reader.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<NpgsqlDataReader> ExecuteReader(string sql, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<NpgsqlParameter>();

			NpgsqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey);

			try
			{
				string formattedSql = FormatSql(sql);
				NpgsqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);

				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception)
			{
				connection.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Executes a SQL statement and processes data reader records through a projection delegate.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="getProjectedObjectDelegate">A delegate that converts data reader records into <typeparamref name="T"/> instances.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns><typeparamref name="T"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="getProjectedObjectDelegate"/> is null.</exception>
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<NpgsqlDataReader, T> getProjectedObjectDelegate, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			return await ExecuteProjection(sql, getProjectedObjectDelegate, (IEnumerable<NpgsqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a SQL statement and processes data reader records through a projection delegate.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="getProjectedObjectDelegate">A delegate that converts data reader records into <typeparamref name="T"/> instances.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns><typeparamref name="T"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="getProjectedObjectDelegate"/> is null.</exception>
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<NpgsqlDataReader, T> getProjectedObjectDelegate, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			parameters = parameters ?? Enumerable.Empty<NpgsqlParameter>();

			var projections = new List<T>();

			using (NpgsqlDataReader reader = await ExecuteReader(sql, parameters))
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						projections.Add(getProjectedObjectDelegate(reader));
					}
				}
			}

			return projections;
		}

		/// <summary>
		/// Executes a SQL statement and processes <see cref="DataRow"/> instances through a projection delegate.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="getProjectedObjectDelegate">A delegate that converts <see cref="DataRow"/> instances into <typeparamref name="T"/> instances.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns><typeparamref name="T"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="getProjectedObjectDelegate"/> is null.</exception>
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<DataRow, T> getProjectedObjectDelegate, params NpgsqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			return await ExecuteProjection(sql, getProjectedObjectDelegate, (IEnumerable<NpgsqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a SQL statement and processes <see cref="DataRow"/> instances through a projection delegate.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="getProjectedObjectDelegate">A delegate that converts <see cref="DataRow"/> instances into <typeparamref name="T"/> instances.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns><typeparamref name="T"/> instances.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="getProjectedObjectDelegate"/> is null.</exception>
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<DataRow, T> getProjectedObjectDelegate, IEnumerable<NpgsqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			parameters = parameters ?? Enumerable.Empty<NpgsqlParameter>();

			var table = new DataTable();

			using (NpgsqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey))
			{
				string formattedSql = FormatSql(sql);
				NpgsqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);
				var dataAdapter = new NpgsqlDataAdapter(command);

				dataAdapter.Fill(table);
			}

			return table.Rows.Cast<DataRow>().Select(getProjectedObjectDelegate);
		}

		private static string FormatSql(string sql)
		{
			return sql.Trim();
		}

		private static object GetSqlParameterValue(object value)
		{
			return value ?? DBNull.Value;
		}
	}
}