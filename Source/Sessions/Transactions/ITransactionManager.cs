using System;

namespace Junior.Persist.Sessions.Transactions
{
	/// <summary>
	/// Represents a way to enlist in a transaction.
	/// </summary>
	public interface ITransactionManager
	{
		/// <summary>
		/// Enlists in a transaction.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <returns>The enlisted transaction.</returns>
		ITransaction Enlist(EnlistmentOption option = EnlistmentOption.AmbientOrNew);

		/// <summary>
		/// Enlists in a transaction.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <param name="timeout">A transaction timeout.</param>
		/// <returns>The enlisted transaction.</returns>
		ITransaction Enlist(EnlistmentOption option, TimeSpan timeout);

		/// <summary>
		/// Enlists in a transaction.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <param name="isolationLevel">A transaction isolation level.</param>
		/// <param name="timeout">A transaction timeout.</param>
		/// <returns>The enlisted transaction.</returns>
		ITransaction Enlist(EnlistmentOption option, IsolationLevel isolationLevel, TimeSpan timeout);
	}
}