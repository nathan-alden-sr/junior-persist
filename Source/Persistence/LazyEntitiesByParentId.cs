using System;
using System.Collections.Generic;

using Junior.Ddd.DomainModel;

namespace Junior.Persist.Persistence
{
	/// <summary>
	/// A relationship to entities that will only be loaded when they are accessed. The entities are retrieved by their parent entity's ID.
	/// </summary>
	public class LazyEntitiesByParentId<TChildEntity> : LazyEntities<TChildEntity>
		where TChildEntity : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LazyEntitiesByParentId{TChildEntity}"/> class.
		/// </summary>
		/// <param name="parentId">The parent entity's ID.</param>
		/// <param name="delegate">A delegate that will retrieve the entity.</param>
		public LazyEntitiesByParentId(Guid parentId, Func<Guid, IEnumerable<TChildEntity>> @delegate)
			: base(() => @delegate(parentId))
		{
		}
	}
}