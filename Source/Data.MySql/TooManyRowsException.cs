using System;
using System.Runtime.Serialization;

namespace Junior.Persist.Data.MySql
{
	/// <summary>
	/// An exception thrown when a SQL query unexpectedly returns too many rows.
	/// </summary>
	public class TooManyRowsException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TooManyRowsException"/> class.
		/// </summary>
		public TooManyRowsException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TooManyRowsException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public TooManyRowsException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TooManyRowsException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public TooManyRowsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TooManyRowsException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected TooManyRowsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}