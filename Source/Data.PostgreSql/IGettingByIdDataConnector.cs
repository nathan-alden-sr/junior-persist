using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Persist.Data.PostgreSql
{
	/// <summary>
	/// Represents a data connector that retrieves entities by ID.
	/// </summary>
	/// <typeparam name="TEntityData"></typeparam>
	public interface IGettingByIdDataConnector<TEntityData>
		where TEntityData : class, IEntityData
	{
		/// <summary>
		/// Retrieves entity data for an entity with the specified ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <returns>Entity data for an entity with the specified ID.</returns>
		Task<TEntityData> GetById(BinaryGuid id);
	}
}