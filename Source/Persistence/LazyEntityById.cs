using System;

using Junior.Ddd.DomainModel;
using Junior.Persist.Persistence.Finders;

namespace Junior.Persist.Persistence
{
	/// <summary>
	/// An entity relationship that will only be loaded when it is accessed. The entity is retrieved by its ID.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class LazyEntityById<TEntity> : LazyEntity<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LazyEntityById{TEntity}"/> class.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <param name="finder">The finder that will retrieve the entity.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		public LazyEntityById(
			Guid id,
			IByIdFinder<TEntity> finder,
			EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException)
			: this(id, finder.ById, entityNotFoundHandling)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyEntityById{TEntity}"/> class.
		/// </summary>
		/// <param name="id">An entity ID.</param>
		/// <param name="delegate">A delegate that will retrieve the entity.</param>
		/// <param name="entityNotFoundHandling">Determines how to handle when entity data is not found.</param>
		public LazyEntityById(
			Guid id,
			Func<Guid, EntityNotFoundHandling, TEntity> @delegate,
			EntityNotFoundHandling entityNotFoundHandling = EntityNotFoundHandling.ThrowException)
			: base(() => @delegate(id, entityNotFoundHandling))
		{
		}
	}
}