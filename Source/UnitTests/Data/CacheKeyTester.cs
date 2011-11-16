using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using Junior.Persist.Data;

using NUnit.Framework;

namespace Junior.Persist.UnitTests.Data
{
	public static class CacheKeyTester
	{
		[TestFixture]
		public class When_comparing_cache_keys_having_different_sql
		{
			[Test]
			public void Must_result_in_non_zero()
			{
				const string sql1 = "sql1";
				const string sql2 = "sql2";
				IEnumerable<DbParameter> parameters = new[]
				                                      	{
				                                      		new SqlParameter("@Test1", 0),
				                                      		new SqlParameter("@Test2", "Test"),
				                                      		new SqlParameter("@Test3", 1.45m)
				                                      	};

				var cacheKey1 = new CacheKey(sql1, parameters);
				var cacheKey2 = new CacheKey(sql2, parameters);
				int result = Comparer<CacheKey>.Default.Compare(cacheKey1, cacheKey2);

				Assert.That(result, Is.EqualTo(-1));
			}
		}

		[TestFixture]
		public class When_comparing_cache_keys_having_the_same_sql_and_same_parameter_names_but_different_parameter_values
		{
			[Test]
			public void Must_result_in_non_zero()
			{
				const string sql = "sql";
				IEnumerable<DbParameter> parameters1 = new[]
				                                       	{
				                                       		new SqlParameter("@Test1", 0),
				                                       		new SqlParameter("@Test2", "Test"),
				                                       		new SqlParameter("@Test3", 1.45m)
				                                       	};
				IEnumerable<DbParameter> parameters2 = new[]
				                                       	{
				                                       		new SqlParameter("@Test1", 0),
				                                       		new SqlParameter("@Test2", "Test"),
				                                       		new SqlParameter("@Test3", 1.452m)
				                                       	};

				var cacheKey1 = new CacheKey(sql, parameters1);
				var cacheKey2 = new CacheKey(sql, parameters2);
				int result = Comparer<CacheKey>.Default.Compare(cacheKey1, cacheKey2);

				Assert.That(result, Is.EqualTo(-1));
			}
		}

		[TestFixture]
		public class When_comparing_cache_keys_having_the_same_sql_and_same_parameters_but_in_a_different_order
		{
			[Test]
			public void Must_result_in_zero()
			{
				const string sql = "sql";
				IEnumerable<DbParameter> parameters = new[]
				                                      	{
				                                      		new SqlParameter("@Test1", 0),
				                                      		new SqlParameter("@Test2", "Test"),
				                                      		new SqlParameter("@Test3", 1.45m)
				                                      	};

				var cacheKey1 = new CacheKey(sql, parameters);
				var cacheKey2 = new CacheKey(sql, parameters.OrderByDescending(arg => arg.ParameterName));
				int result = Comparer<CacheKey>.Default.Compare(cacheKey1, cacheKey2);

				Assert.That(result, Is.EqualTo(0));
			}
		}

		[TestFixture]
		public class When_comparing_cache_keys_having_the_same_sql_but_different_parameters
		{
			[Test]
			public void Must_result_in_non_zero()
			{
				const string sql = "sql";
				IEnumerable<DbParameter> parameters = new[]
				                                      	{
				                                      		new SqlParameter("@Test1", 0),
				                                      		new SqlParameter("@Test2", "Test"),
				                                      		new SqlParameter("@Test3", 1.45m)
				                                      	};

				var cacheKey1 = new CacheKey(sql, parameters.Take(1).Skip(1).Take(1));
				var cacheKey2 = new CacheKey(sql, parameters);
				int result = Comparer<CacheKey>.Default.Compare(cacheKey1, cacheKey2);

				Assert.That(result, Is.EqualTo(-1));
			}
		}

		[TestFixture]
		public class When_comparing_cache_keys_that_are_exactly_the_same
		{
			[Test]
			public void Must_result_in_zero()
			{
				const string sql = "sql";
				IEnumerable<DbParameter> parameters = new[]
				                                      	{
				                                      		new SqlParameter("@Test1", 0),
				                                      		new SqlParameter("@Test2", "Test"),
				                                      		new SqlParameter("@Test3", 1.45m)
				                                      	};

				var cacheKey1 = new CacheKey(sql, parameters);
				var cacheKey2 = new CacheKey(sql, parameters);
				int result = Comparer<CacheKey>.Default.Compare(cacheKey1, cacheKey2);

				Assert.That(result, Is.EqualTo(0));
			}
		}
	}
}