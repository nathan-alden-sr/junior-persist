using Junior.Common;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// Represents a data connector that deletes entities by ID.
	/// </summary>
	public interface IDeletingByIdDataConnector
	{
		/// <summary>
		/// Deletes an entity with the specified ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		void DeleteById(BinaryGuid id);
	}
}