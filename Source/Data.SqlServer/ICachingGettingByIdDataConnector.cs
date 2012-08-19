using System;

namespace Junior.Persist.Data.SqlServer
{
	/// <summary>
	/// Represents a data connector that retrieves entities by ID and caches the results.
	/// </summary>
	public interface ICachingGettingByIdDataConnector<out TEntityData>
		where TEntityData : class, IEntityData
	{
		/// <summary>
		/// Retrieves entity data for an entity with the specified ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <returns>A query result specifying either cached entity data or containing the entity data itself for the specified entity ID.</returns>
		IQueryResult<TEntityData> GetById(Guid id);
	}
}