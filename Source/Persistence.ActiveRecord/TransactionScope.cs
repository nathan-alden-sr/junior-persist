using System.Data;

using Castle.ActiveRecord;

namespace Junior.Persist.Persistence.ActiveRecord
{
	/// <summary>
	/// Wraps the <see cref="Castle.ActiveRecord.TransactionScope"/> class.
	/// </summary>
	public class TransactionScope : Castle.ActiveRecord.TransactionScope
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionScope"/> class.
		/// <see cref="TransactionMode"/> is set to <see cref="TransactionMode.Inherits"/>, <see cref="IsolationLevel"/> is set to <see cref="IsolationLevel.ReadCommitted"/>
		/// and <see cref="OnDispose"/> is set to <see cref="OnDispose.Rollback"/>.
		/// </summary>
		public TransactionScope()
			: base(TransactionMode.Inherits, IsolationLevel.ReadCommitted, OnDispose.Rollback)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionScope"/> class.
		/// </summary>
		/// <param name="onDisposeBehavior">Determines how the transaction behaves when the transaction is disposed.</param>
		public TransactionScope(OnDispose onDisposeBehavior)
			: base(onDisposeBehavior)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionScope"/> class.
		/// </summary>
		/// <param name="mode">Determines how transactions are enrolled.</param>
		public TransactionScope(TransactionMode mode)
			: base(mode)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionScope"/> class.
		/// </summary>
		/// <param name="mode">Determines how transactions are enrolled.</param>
		/// <param name="onDisposeBehavior">Determines how the transaction behaves when the transaction is disposed.</param>
		public TransactionScope(TransactionMode mode, OnDispose onDisposeBehavior)
			: base(mode, onDisposeBehavior)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionScope"/> class.
		/// </summary>
		/// <param name="mode">Determines how transactions are enrolled.</param>
		/// <param name="isolationLevel">The transaction isolation level.</param>
		/// <param name="onDisposeBehavior">Determines how the transaction behaves when the transaction is disposed.</param>
		public TransactionScope(TransactionMode mode, IsolationLevel isolationLevel, OnDispose onDisposeBehavior)
			: base(mode, isolationLevel, onDisposeBehavior)
		{
		}
	}
}