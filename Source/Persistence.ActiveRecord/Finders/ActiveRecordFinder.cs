using System;
using System.Collections.Generic;
using System.Linq;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

using Junior.Common;
using Junior.Persist.Persistence.Finders;

using NHibernate.Criterion;

namespace Junior.Persist.Persistence.ActiveRecord.Finders
{
	/// <summary>
	/// A finder that uses Castle ActiveRecord to find entities.
	/// </summary>
	public abstract class ActiveRecordFinder<TEntity, TEntityData> : Finder<TEntity, TEntityData>
		where TEntity : class
		where TEntityData : class
	{
		/// <summary>
		/// Retrieves an entity using a criterion.
		/// </summary>
		/// <param name="criterion">A criterion.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		/// <exception cref="EntityNotFoundException">Thrown when <paramref name="entityNotFoundHandling"/> is <see cref="EntityNotFoundHandling.ThrowException"/> and an entity was not found.</exception>
		protected TEntity GetEntity(ICriterion criterion, EntityNotFoundHandling entityNotFoundHandling)
		{
			return GetEntity(criterion.ToEnumerable(), entityNotFoundHandling);
		}

		/// <summary>
		/// Retrieves an entity using criteria.
		/// </summary>
		/// <param name="criteria">Criteria.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		/// <exception cref="EntityNotFoundException">Thrown when <paramref name="entityNotFoundHandling"/> is <see cref="EntityNotFoundHandling.ThrowException"/> and an entity was not found.</exception>
		protected TEntity GetEntity(IEnumerable<ICriterion> criteria, EntityNotFoundHandling entityNotFoundHandling)
		{
			try
			{
				TEntityData entityData = ActiveRecordBase<TEntityData>.FindOne(criteria.ToArray());

				if (entityData == null)
				{
					if (entityNotFoundHandling == EntityNotFoundHandling.ReturnNull)
					{
						return null;
					}
					throw GetEntityNotFoundException();
				}

				return GetEntity(entityData);
			}
			catch (ActiveRecordException exception)
			{
				throw GetEntityNotFoundException(exception);
			}
		}

		/// <summary>
		/// Retrieves an entity using detached criteria.
		/// </summary>
		/// <param name="criteria">Detached criteria.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		/// <exception cref="EntityNotFoundException">Thrown when <paramref name="entityNotFoundHandling"/> is <see cref="EntityNotFoundHandling.ThrowException"/> and an entity was not found.</exception>
		protected TEntity GetEntity(DetachedCriteria criteria, EntityNotFoundHandling entityNotFoundHandling)
		{
			try
			{
				TEntityData entityData = ActiveRecordBase<TEntityData>.FindOne(criteria);

				if (entityData == null)
				{
					if (entityNotFoundHandling == EntityNotFoundHandling.ReturnNull)
					{
						return null;
					}
					throw GetEntityNotFoundException();
				}

				return GetEntity(entityData);
			}
			catch (ActiveRecordException exception)
			{
				throw GetEntityNotFoundException(exception);
			}
		}

		private static EntityNotFoundException GetEntityNotFoundException(Exception exception = null)
		{
			return new EntityNotFoundException(typeof(TEntity).Name + " not found.", exception);
		}

		/// <summary>
		/// Retrieves an <see cref="EntityNotFoundException"/> instance given a <see cref="NotFoundException"/> and an entity ID.
		/// </summary>
		/// <param name="exception">A <see cref="NotFoundException"/> instance.</param>
		/// <param name="id">An entity ID.</param>
		/// <returns>An <see cref="EntityNotFoundException"/> that wraps <paramref name="exception"/> and <paramref name="id"/>.</returns>
		protected static EntityNotFoundException GetEntityNotFoundException(NotFoundException exception, Guid id)
		{
			return new EntityNotFoundException(String.Format("{0} with ID {1:N} not found.", typeof(TEntity).Name, id), exception);
		}
	}
}