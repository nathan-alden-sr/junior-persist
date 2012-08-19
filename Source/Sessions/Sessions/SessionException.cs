using System;
using System.Runtime.Serialization;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// An exception thrown by a session implementation.
	/// </summary>
	public class SessionException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionException"/> class.
		/// </summary>
		public SessionException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SessionException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public SessionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SessionException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public SessionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SessionException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected SessionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}