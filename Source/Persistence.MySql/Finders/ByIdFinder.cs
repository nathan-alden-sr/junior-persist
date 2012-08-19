using System;

using Junior.Common;
using Junior.Persist.Data.MySql;
using Junior.Persist.Persistence.Finders;

namespace Junior.Persist.Persistence.MySql.Finders
{
	/// <summary>
	/// Finds an entity by ID.
	/// </summary>
	public abstract class ByIdFinder<TEntity, TEntityData, TDataConnector> : Finder<TEntity, TEntityData>, IByIdFinder<TEntity>
		where TEntity : class
		where TEntityData : class, IEntityData
		where TDataConnector : class, IGettingByIdDataConnector<TEntityData>
	{
		private readonly TDataConnector _dataConnector;

		/// <summary>
		/// Initializes a new instance of the <see cref="ByIdFinder{TEntity,TEntityData,TDataConnector}"/> class.
		/// </summary>
		/// <param name="dataConnector">The data connector that finds entity data.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="dataConnector"/> is null.</exception>
		protected ByIdFinder(TDataConnector dataConnector)
		{
			dataConnector.ThrowIfNull("dataConnector");

			_dataConnector = dataConnector;
		}

		/// <summary>
		/// Gets the data connector that finds entity data.
		/// </summary>
		protected TDataConnector DataConnector
		{
			get
			{
				return _dataConnector;
			}
		}

		/// <summary>
		/// Finds an entity by ID.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		/// <returns>An entity.</returns>
		public TEntity ById(Guid id, EntityNotFoundHandling entityNotFoundHandling)
		{
			TEntityData entityData = _dataConnector.GetById(id);

			return GetEntity(entityData, entityNotFoundHandling);
		}
	}
}