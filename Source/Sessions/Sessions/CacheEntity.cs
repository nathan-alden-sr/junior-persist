using System;
using System.Diagnostics;

using Junior.Common;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Pairs an entity with its ID to uniquely identify an entity in a cache.
	/// </summary>
	[DebuggerDisplay("Entity = {_entity}, Id = {_id}")]
	public class CacheEntity<TEntity>
		where TEntity : class
	{
		private readonly TEntity _entity;
		private readonly Guid _id;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheEntity{TEntity}"/> class.
		/// </summary>
		/// <param name="entity">An entity.</param>
		/// <param name="id">The entity's ID.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
		public CacheEntity(TEntity entity, Guid id)
		{
			entity.ThrowIfNull("entity");

			_entity = entity;
			_id = id;
		}

		/// <summary>
		/// Gets the cached entity.
		/// </summary>
		public TEntity Entity
		{
			get
			{
				return _entity;
			}
		}

		/// <summary>
		/// Gets the cached entity's ID.
		/// </summary>
		public Guid Id
		{
			get
			{
				return _id;
			}
		}

		/// <summary>
		/// Creates a new <see cref="CacheEntity{TCastedEntity}"/> instance from the current entity and its ID and by casting the current entity to <typeparamref name="TCastedEntity"/>.
		/// </summary>
		/// <typeparam name="TCastedEntity">The entity type to cast to.</typeparam>
		/// <returns>A new <see cref="CacheEntity{TCastedEntity}"/> instance with the current entity and its ID.</returns>
		public CacheEntity<TCastedEntity> Cast<TCastedEntity>()
			where TCastedEntity : class, TEntity
		{
			return new CacheEntity<TCastedEntity>((TCastedEntity)_entity, _id);
		}

		/// <summary>
		/// Implicitly casts any <see cref="CacheEntity{TEntity}"/> instance to a <see cref="CacheEntity{TEntity}"/> whose entity type is <see cref="object"/>.
		/// </summary>
		/// <param name="cacheEntity">A cache entity.</param>
		/// <returns>A new <see cref="CacheEntity{TCastedEntity}"/> instance with the current entity and its ID and whose entity type is <see cref="object"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheEntity"/> is null.</exception>
		public static implicit operator CacheEntity<object>(CacheEntity<TEntity> cacheEntity)
		{
			cacheEntity.ThrowIfNull("cacheEntity");

			return new CacheEntity<object>(cacheEntity.Entity, cacheEntity.Id);
		}
	}
}