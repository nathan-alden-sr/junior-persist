using System;
using System.Data.SqlClient;

using Junior.Common;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// Extensions for the <see cref="SqlDataReader"/> type.
	/// </summary>
	public static class SqlDataReaderExtensions
	{
		/// <summary>
		/// Gets a value as type <typeparamref name="T"/> from the specified data reader.
		/// </summary>
		/// <param name="reader">A data reader.</param>
		/// <param name="column">A column name.</param>
		/// <typeparam name="T">The data type.</typeparam>
		/// <returns>The column value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="reader"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="column"/> is null.</exception>
		public static T GetValue<T>(this SqlDataReader reader, string column)
		{
			reader.ThrowIfNull("reader");
			column.ThrowIfNull("column");

			int ordinal = reader.GetOrdinal(column);
			Type type = typeof(T);

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && reader.IsDBNull(ordinal))
			{
				return default(T);
			}

			return (T)reader.GetValue(ordinal);
		}

		/// <summary>
		/// Gets a precise date-time from the specified data reader.
		/// </summary>
		/// <param name="reader">A data reader.</param>
		/// <param name="column">A column name.</param>
		/// <returns>A precise date-time</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="reader"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="column"/> is null.</exception>
		public static PreciseDateTime GetPreciseDateTime(this SqlDataReader reader, string column)
		{
			reader.ThrowIfNull("reader");
			column.ThrowIfNull("column");

			int ordinal = reader.GetOrdinal(column);

			return new PreciseDateTime(reader.GetInt64(ordinal));
		}
	}
}