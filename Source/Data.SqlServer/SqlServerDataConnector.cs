using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// A SQL Server data connector.
	/// </summary>
	public abstract class SqlServerDataConnector : DataConnector<SqlConnection, SqlCommand, SqlParameter>
	{
		private readonly string _connectionKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlServerDataConnector"/> type.
		/// </summary>
		/// <param name="connectionProvider">A connection provider.</param>
		/// <param name="commandProvider">A command provider.</param>
		/// <param name="connectionKey">A connection key.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionKey"/> is null.</exception>
		protected SqlServerDataConnector(
			IConnectionProvider<SqlConnection> connectionProvider,
			ICommandProvider<SqlConnection, SqlCommand, SqlParameter> commandProvider,
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
		protected SqlParameter GetParameter<T>(string parameterName, T value, SqlDbType type)
		{
			parameterName.ThrowIfNull("parameterName");

			return new SqlParameter(parameterName, type)
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
		protected SqlParameter GetParameter<T>(string parameterName, T value, SqlDbType type, int size)
		{
			parameterName.ThrowIfNull("parameterName");

			return new SqlParameter(parameterName, type, size)
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
		protected SqlParameter GetParameter<T>(string parameterName, T value, SqlDbType type, int size, byte precision, byte scale)
		{
			parameterName.ThrowIfNull("parameterName");

			return new SqlParameter(parameterName, type, size)
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
		protected SqlParameter GetParameter<T>(string parameterName, T value, SqlDbType type, byte precision, byte scale)
		{
			parameterName.ThrowIfNull("parameterName");

			return new SqlParameter(parameterName, type)
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
		protected SqlParameter GetParameter(string parameterName, PreciseDateTime value)
		{
			parameterName.ThrowIfNull("parameterName");

			return new SqlParameter(parameterName, SqlDbType.BigInt, 8)
				{
					Value = GetSqlParameterValue((long)value)
				};
		}

		/// <summary>
		/// Gets a named parameter.
		/// </summary>
		/// <param name="parameterName">The parameter's name.</param>
		/// <param name="value">The parameter's value.</param>
		/// <returns>A named parameter.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is null.</exception>
		protected SqlParameter GetParameter(string parameterName, PreciseDateTime? value)
		{
			parameterName.ThrowIfNull("parameterName");

			return new SqlParameter(parameterName, GetSqlParameterValue((long?)value));
		}

		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>The number of rows affected by the SQL statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<int> ExecuteNonQuery(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteNonQuery(sql, (IEnumerable<SqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>The number of rows affected by the SQL statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<int> ExecuteNonQuery(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<SqlParameter>();

			using (SqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey))
			{
				string formattedSql = FormatSql(sql);
				SqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);

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
		protected async Task<T> ExecuteScalar<T>(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteScalar<T>(sql, (IEnumerable<SqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a SQL statement that returns a scalar value.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A scalar value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<T> ExecuteScalar<T>(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<SqlParameter>();

			using (SqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey))
			{
				string formattedSql = FormatSql(sql);
				SqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);
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
		protected async Task<SqlDataReader> ExecuteReader(string sql, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");

			return await ExecuteReader(sql, (IEnumerable<SqlParameter>)parameters);
		}

		/// <summary>
		/// Executes a SQL statement and returns a data reader.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		/// <returns>A data reader.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		protected async Task<SqlDataReader> ExecuteReader(string sql, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<SqlParameter>();

			SqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey);

			try
			{
				string formattedSql = FormatSql(sql);
				SqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);

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
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<SqlDataReader, T> getProjectedObjectDelegate, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			return await ExecuteProjection(sql, getProjectedObjectDelegate, (IEnumerable<SqlParameter>)parameters);
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
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<SqlDataReader, T> getProjectedObjectDelegate, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			parameters = parameters ?? Enumerable.Empty<SqlParameter>();

			var projections = new List<T>();

			using (SqlDataReader reader = await ExecuteReader(sql, parameters))
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
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<DataRow, T> getProjectedObjectDelegate, params SqlParameter[] parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			return await ExecuteProjection(sql, getProjectedObjectDelegate, (IEnumerable<SqlParameter>)parameters);
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
		protected async Task<IEnumerable<T>> ExecuteProjection<T>(string sql, Func<DataRow, T> getProjectedObjectDelegate, IEnumerable<SqlParameter> parameters)
		{
			sql.ThrowIfNull("sql");
			getProjectedObjectDelegate.ThrowIfNull("getProjectedObjectDelegate");

			parameters = parameters ?? Enumerable.Empty<SqlParameter>();

			var table = new DataTable();

			using (SqlConnection connection = await ConnectionProvider.GetConnection(_connectionKey))
			{
				string formattedSql = FormatSql(sql);
				SqlCommand command = CommandProvider.GetCommand(_connectionKey, connection, formattedSql, parameters);
				var dataAdapter = new SqlDataAdapter(command);

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