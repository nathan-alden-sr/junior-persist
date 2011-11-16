using System;
using System.Runtime.Serialization;

namespace Junior.Persist.Persistence.Finders
{
	/// <summary>
	/// An exception thrown when an entity was not found.
	/// </summary>
	public class EntityNotFoundException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
		/// </summary>
		public EntityNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public EntityNotFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public EntityNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}