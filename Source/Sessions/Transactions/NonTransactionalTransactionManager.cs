using System;

namespace Junior.Persist.Sessions.Transactions
{
	/// <summary>
	/// A transaction manager that enlists in transactions that take no action whether they are committed or rolled back.
	/// <see cref="NullTransaction"/> is the type of enlisted transaction.
	/// </summary>
	public class NonTransactionalTransactionManager : ITransactionManager
	{
		/// <summary>
		/// Enlists in a transaction that takes no action whether it is committed or rolled back.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <returns>The enlisted transaction.</returns>
		public ITransaction Enlist(EnlistmentOption option)
		{
			return new NullTransaction();
		}

		/// <summary>
		/// Enlists in a transaction that takes no action whether it is committed or rolled back.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <param name="timeout">A transaction timeout.</param>
		/// <returns>The enlisted transaction.</returns>
		public ITransaction Enlist(EnlistmentOption option, TimeSpan timeout)
		{
			return new NullTransaction();
		}

		/// <summary>
		/// Enlists in a transaction that takes no action whether it is committed or rolled back.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <param name="isolationLevel">A transaction isolation level.</param>
		/// <param name="timeout">A transaction timeout.</param>
		/// <returns>The enlisted transaction.</returns>
		public ITransaction Enlist(EnlistmentOption option, IsolationLevel isolationLevel, TimeSpan timeout)
		{
			return new NullTransaction();
		}
	}
}