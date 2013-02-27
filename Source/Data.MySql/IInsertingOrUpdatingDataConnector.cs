using System.Threading.Tasks;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// Represents a data connector that inserts and updates entities.
	/// </summary>
	public interface IInsertingOrUpdatingDataConnector<in TEntityData>
		where TEntityData : class, IEntityData
	{
		/// <summary>
		/// Insert the specified entity data if the entity it identifies has not been persisted; otherwise, update the entity data.
		/// </summary>
		/// <param name="entityData">Entity data.</param>
		Task InsertOrUpdate(TEntityData entityData);
	}
}