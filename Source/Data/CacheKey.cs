using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using Junior.Common;

namespace Junior.Persist.Data
{
	/// <summary>
	/// A key used for caching the results of a SQL statement. Keys are unique based on the SQL itself and the parameters and their values.
	/// </summary>
	public class CacheKey : IComparable<CacheKey>
	{
		private readonly IEnumerable<Pair<string, object>> _parameters;
		private readonly string _sql;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKey"/> class.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		public CacheKey(string sql)
			: this(sql, Enumerable.Empty<DbParameter>())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKey"/> class.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">Named parameters.</param>
		public CacheKey(string sql, params DbParameter[] parameters)
			: this(sql, (IEnumerable<DbParameter>)parameters)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKey"/> class.
		/// </summary>
		/// <param name="sql">A SQL statement.</param>
		/// <param name="parameters">The named parameters to use when executing the SQL statement.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
		public CacheKey(string sql, IEnumerable<DbParameter> parameters)
		{
			sql.ThrowIfNull("sql");

			parameters = parameters ?? Enumerable.Empty<DbParameter>();

			_sql = sql;
			_parameters = parameters
				.Select(arg => new Pair<string, object>(arg.ParameterName, arg.Value))
				.OrderBy(arg => arg.First);
		}

		/// <summary>
		/// Compares the current object with another object of the same type. A cache key is equivalent to another cache key if
		/// the SQL statement, its parameters and values are all the same.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="other"/> is null.</exception>
		public int CompareTo(CacheKey other)
		{
			other.ThrowIfNull("other");

			int result = String.CompareOrdinal(_sql, other._sql);

			if (result != 0)
			{
				return result;
			}

			Pair<string, object>[] parameters = _parameters.ToArray();
			Pair<string, object>[] otherParameters = other._parameters.ToArray();

			result = parameters.Length.CompareTo(otherParameters.Length);

			if (result != 0)
			{
				return result;
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				result = String.CompareOrdinal(parameters[i].First, otherParameters[i].First);

				if (result != 0)
				{
					return result;
				}

				result = Comparer<object>.Default.Compare(parameters[i].Second, otherParameters[i].Second);

				if (result != 0)
				{
					return result;
				}
			}

			return 0;
		}
	}
}