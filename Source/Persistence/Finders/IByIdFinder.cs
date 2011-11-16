using System;

namespace Junior.Persist.Persistence.Finders
{
	/// <summary>
	/// Represents a way to find an entity by ID.
	/// </summary>
	public interface IByIdFinder<out TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Finds an entity by ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		TEntity ById(Guid id, EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException);
	}
}