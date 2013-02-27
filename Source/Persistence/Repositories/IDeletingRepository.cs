using System.Threading.Tasks;

namespace Junior.Persist.Persistence.Repositories
{
	/// <summary>
	/// Represents a repository that deletes entities.
	/// </summary>
	public interface IDeletingRepository<in TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		Task Delete(TEntity entity);
	}
}