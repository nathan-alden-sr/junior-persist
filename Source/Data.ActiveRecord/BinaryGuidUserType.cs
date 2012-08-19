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
	/// An NHibernate user type representing a GUID stored as an array.
	/// </summary>
	public class BinaryGuidUserType : IUserType
	{
#pragma warning disable 1591
		public SqlType[] SqlTypes
		{
			get
			{
				return NHibernateUtil.BinaryBlob.SqlType.ToEnumerable().ToArray();
			}
		}

		public Type ReturnedType
		{
			get
			{
				return typeof(BinaryGuid);
			}
		}

		public bool IsMutable
		{
			get
			{
				return false;
			}
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			object value = NHibernateUtil.BinaryBlob.NullSafeGet(rs, names[0]);

			return value.IfNotNull(arg => (BinaryGuid?)new BinaryGuid((byte[])arg));
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			if (value == null || value == DBNull.Value)
			{
				NHibernateUtil.BinaryBlob.NullSafeSet(cmd, null, index);
			}
			else
			{
				NHibernateUtil.BinaryBlob.NullSafeSet(cmd, (byte[])(BinaryGuid)value, index);
			}
		}

		public object DeepCopy(object value)
		{
			return value.IfNotNull(arg => (object)((BinaryGuid)value));
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