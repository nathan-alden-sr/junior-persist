using System;

using Junior.Common;

namespace Junior.Persist.Persistence.Repositories
{
	/// <summary>
	/// A factory that generates new entity IDs.
	/// </summary>
	public class EntityIdFactory : IEntityIdFactory
	{
		private readonly IGuidFactory _guidFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityIdFactory"/> class.
		/// </summary>
		/// <param name="guidFactory">A GUID factory.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="guidFactory"/> is null.</exception>
		public EntityIdFactory(IGuidFactory guidFactory)
		{
			guidFactory.ThrowIfNull("guidFactory");

			_guidFactory = guidFactory;
		}

		/// <summary>
		/// Generates a new entity ID.
		/// </summary>
		/// <returns>The new entity ID.</returns>
		public Guid NewId()
		{
			return _guidFactory.Random();
		}
	}
}