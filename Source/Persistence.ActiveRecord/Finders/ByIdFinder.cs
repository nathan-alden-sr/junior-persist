using System;

using Castle.ActiveRecord;

using Junior.Common;
using Junior.Persist.Persistence.Finders;

namespace Junior.Persist.Persistence.ActiveRecord.Finders
{
	/// <summary>
	/// A finder that finds entities by ID.
	/// </summary>
	public abstract class ByIdFinder<TEntity, TEntityData> : ActiveRecordFinder<TEntity, TEntityData>, IByIdFinder<TEntity>
		where TEntity : class
		where TEntityData : class
	{
		/// <summary>
		/// Finds an entity by ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		/// <exception cref="EntityNotFoundException">Thrown when <paramref name="entityNotFoundHandling"/> is <see cref="EntityNotFoundHandling.ThrowException"/> and an entity was not found.</exception>
		public TEntity ById(Guid id, EntityNotFoundHandling entityNotFoundHandling)
		{
			try
			{
				return GetEntity(ActiveRecordBase<TEntityData>.Find((BinaryGuid)id));
			}
			catch (NotFoundException exception)
			{
				if (entityNotFoundHandling == EntityNotFoundHandling.ReturnNull)
				{
					return null;
				}
				throw GetEntityNotFoundException(exception, id);
			}
		}
	}
}