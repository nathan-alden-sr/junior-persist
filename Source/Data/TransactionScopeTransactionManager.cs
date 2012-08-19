using System;
using System.Transactions;

using Junior.Persist.Data.PairMappers;
using Junior.Persist.Sessions.Transactions;

using IsolationLevel = Junior.Persist.Sessions.Transactions.IsolationLevel;

namespace Junior.Persist.Data
{
	/// <summary>
	/// Enlists in a transaction using <see cref="TransactionScope"/>. The <see cref="TransactionScope"/> instance is wrapped in a
	/// <see cref="TransactionScopeTransaction"/> instance.
	/// </summary>
	public class TransactionScopeTransactionManager : ITransactionManager
	{
		/// <summary>
		/// Enlists in a transaction.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <returns>The enlisted transaction.</returns>
		public ITransaction Enlist(EnlistmentOption option = EnlistmentOption.AmbientOrNew)
		{
			TransactionScopeOption transactionScopeOption = EnlistmentOptionPairMapper.Instance.Map(option);
			var transactionScope = new TransactionScope(transactionScopeOption);

			return new TransactionScopeTransaction(transactionScope);
		}

		/// <summary>
		/// Enlists in a transaction.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <param name="timeout">A transaction timeout.</param>
		/// <returns>The enlisted transaction.</returns>
		public ITransaction Enlist(EnlistmentOption option, TimeSpan timeout)
		{
			TransactionScopeOption transactionScopeOption = EnlistmentOptionPairMapper.Instance.Map(option);
			var transactionScope = new TransactionScope(transactionScopeOption, timeout);

			return new TransactionScopeTransaction(transactionScope);
		}

		/// <summary>
		/// Enlists in a transaction.
		/// </summary>
		/// <param name="option">An enlistment option.</param>
		/// <param name="isolationLevel">A transaction isolation level.</param>
		/// <param name="timeout">A transaction timeout.</param>
		/// <returns>The enlisted transaction.</returns>
		public ITransaction Enlist(EnlistmentOption option, IsolationLevel isolationLevel, TimeSpan timeout)
		{
			TransactionScopeOption transactionScopeOption = EnlistmentOptionPairMapper.Instance.Map(option);
			var transactionOptions = new TransactionOptions
				{
					IsolationLevel = IsolationLevelPairMapper.Instance.Map(isolationLevel),
					Timeout = timeout
				};
			var transactionScope = new TransactionScope(transactionScopeOption, transactionOptions);

			return new TransactionScopeTransaction(transactionScope);
		}
	}
}