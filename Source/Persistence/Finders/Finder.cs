using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;

namespace Junior.Persist.Persistence.Finders
{
	/// <summary>
	/// Base class for all finders. Provides helper methods for converting entity data into entities.
	/// </summary>
	public abstract class Finder<TEntity, TEntityData>
		where TEntity : class
		where TEntityData : class
	{
		/// <summary>
		/// Converts entity data into an entity.
		/// </summary>
		/// <param name="entityData">Entity data.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityData"/> is null.</exception>
		/// <exception cref="EntityNotFoundException">Thrown when <paramref name="entityNotFoundHandling"/> is <see cref="EntityNotFoundHandling.ThrowException"/> and entity data was not found.</exception>
		protected TEntity GetEntity(TEntityData entityData, EntityNotFoundHandling entityNotFoundHandling)
		{
			entityData.ThrowIfNull("entityData");

			if (entityData == null && entityNotFoundHandling == EntityNotFoundHandling.ThrowException)
			{
				throw new EntityNotFoundException(String.Format("{0} not found.", typeof(TEntity).Name));
			}

			return entityData.IfNotNull(GetEntity);
		}

		/// <summary>
		/// Converts entity data into entities.
		/// </summary>
		/// <param name="entityDatas">Entity data.</param>
		/// <returns>Entities.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityDatas"/> is null.</exception>
		protected IEnumerable<TEntity> GetEntities(IEnumerable<TEntityData> entityDatas)
		{
			entityDatas.ThrowIfNull("entityDatas");

			return entityDatas.Select(GetEntity);
		}

		/// <summary>
		/// Converts entity data into an entity. 
		/// </summary>
		/// <param name="entityData">Entity data.</param>
		/// <returns>An entity.</returns>
		protected abstract TEntity GetEntity(TEntityData entityData);
	}
}