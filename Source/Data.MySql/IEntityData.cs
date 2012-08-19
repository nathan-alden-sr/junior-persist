using Junior.Common;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// Represents entity data.
	/// </summary>
	public interface IEntityData
	{
		/// <summary>
		/// Gets an entity ID.
		/// </summary>
		BinaryGuid Id
		{
			get;
		}
	}
}