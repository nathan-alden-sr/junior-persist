using System;

namespace Junior.Persist.Persistence.Repositories
{
	/// <summary>
	/// Represents a factory that generates new entity IDs.
	/// </summary>
	public interface IEntityIdFactory
	{
		/// <summary>
		/// Generates a new entity ID.
		/// </summary>
		/// <returns>The new entity ID.</returns>
		Guid NewId();
	}
}