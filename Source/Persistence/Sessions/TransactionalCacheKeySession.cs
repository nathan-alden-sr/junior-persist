using System;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;
using Junior.Persist.Sessions.Transactions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// A transactional session that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public class TransactionalCacheKeySession : CacheKeySession<TransactionalCacheKeySession>, ITransactionalCacheKeySession
	{
		private readonly ITransaction _transaction;

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionalCacheKeySession"/> class.
		/// </summary>
		/// <param name="transaction">The current transaction.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="transaction"/> is null.</exception>
		internal TransactionalCacheKeySession(ITransaction transaction)
		{
			transaction.ThrowIfNull("transaction");

			_transaction = transaction;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionalCacheKeySession"/> class.
		/// </summary>
		/// <param name="transaction">The current transaction.</param>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="transaction"/> is null.</exception>
		internal TransactionalCacheKeySession(ITransaction transaction, ISessionObserver<CacheKey, object> observer)
			: base(observer)
		{
			transaction.ThrowIfNull("transaction");

			_transaction = transaction;
		}

		/// <summary>
		/// Commits the current transaction.
		/// </summary>
		public void Commit()
		{
			this.ThrowIfDisposed(Disposed);

			_transaction.Commit();
		}

		/// <summary>
		/// The next outer scope on the current thread becomes the current context.
		/// </summary>
		/// <param name="disposing">When true, the next outer scope on the current thread becomes the current context; otherwise, a null operation.</param>
		protected override void Dispose(bool disposing)
		{
			this.ThrowIfDisposed(Disposed);

			_transaction.Dispose();

			base.Dispose(disposing);
		}
	}
}