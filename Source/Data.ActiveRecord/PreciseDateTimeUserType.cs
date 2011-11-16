using System;
using System.Data;
using System.Linq;

using Junior.Common;

using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Junior.Persist.Data.ActiveRecord
{
	/// <summary>
	/// An NHibernate user type representing a DateTime as a 64-bit integer.
	/// </summary>
	public class PreciseDateTimeUserType : IUserType
	{
#pragma warning disable 1591
		public SqlType[] SqlTypes
		{
			get
			{
				return NHibernateUtil.Int64.SqlType.ToEnumerable().ToArray();
			}
		}

		public Type ReturnedType
		{
			get
			{
				return typeof(PreciseDateTime);
			}
		}

		public bool IsMutable
		{
			get
			{
				return false;
			}
		}

		public int GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			object value = NHibernateUtil.Int64.NullSafeGet(rs, names[0]);

			return value.IfNotNull(arg => (PreciseDateTime?)new PreciseDateTime((long)arg));
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			if (value == null || value == DBNull.Value)
			{
				NHibernateUtil.Int64.NullSafeSet(cmd, null, index);
			}
			else
			{
				NHibernateUtil.Int64.NullSafeSet(cmd, (long)(PreciseDateTime)value, index);
			}
		}

		public object DeepCopy(object value)
		{
			return value.IfNotNull(arg => (object)((PreciseDateTime)value));
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}

		public object Assemble(object cached, object owner)
		{
			return cached;
		}

		public object Disassemble(object value)
		{
			return value;
		}

		bool IUserType.Equals(object x, object y)
		{
			return Equals(x, y);
		}
#pragma warning restore 1591
	}
}