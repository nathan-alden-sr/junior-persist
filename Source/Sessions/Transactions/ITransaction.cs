using System;

namespace Junior.Persist.Sessions.Transactions
{
	/// <summary>
	/// Represents a transaction. To rollback a transaction, do not call <see cref="Commit"/>.
	/// </summary>
	public interface ITransaction : IDisposable
	{
		/// <summary>
		/// Commits the transaction.
		/// </summary>
		void Commit();
	}
}