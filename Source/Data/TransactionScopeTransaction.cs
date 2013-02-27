using System;
using System.Threading.Tasks;
using System.Transactions;

using Junior.Common;
using Junior.Persist.Sessions.Transactions;

namespace Junior.Persist.Data
{
	internal class TransactionScopeTransaction : ITransaction
	{
		private readonly TransactionScope _transaction;
		private bool _disposed;

		public TransactionScopeTransaction(TransactionScope transaction)
		{
			transaction.ThrowIfNull("transaction");

			_transaction = transaction;
		}

		public void Dispose()
		{
			this.ThrowIfDisposed(_disposed);

			_transaction.Dispose();

			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public async Task Commit()
		{
			this.ThrowIfDisposed(_disposed);

			await Task.Run(() => _transaction.Complete());
		}
	}
}