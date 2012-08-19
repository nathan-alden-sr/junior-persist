using System;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// Represents entity data.
	/// </summary>
	public interface IEntityData
	{
		/// <summary>
		/// Gets an entity ID.
		/// </summary>
		Guid Id
		{
			get;
		}
	}
}