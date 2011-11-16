using System;

namespace Junior.Persist.Persistence.Repositories
{
	/// <summary>
	/// Represents a repository that persists entities.
	/// </summary>
	public interface IPersistingRepository<in TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Persists an entity.
		/// </summary>
		/// <param name="entity">The entity to persist.</param>
		/// <returns>The entity's ID.</returns>
		Guid Persist(TEntity entity);
	}

	/// <summary>
	/// Represents a repository that persists entities that have a parent entity.
	/// </summary>
	public interface IPersistingRepository<in TParentEntity, in TEntity>
		where TParentEntity : class
		where TEntity : class
	{
		/// <summary>
		/// Persists an entity.
		/// </summary>
		/// <param name="parentEntity">The parent of the entity to persist.</param>
		/// <param name="entity">The entity to persist.</param>
		/// <returns>The entity's ID.</returns>
		Guid Persist(TParentEntity parentEntity, TEntity entity);
	}
}