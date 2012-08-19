using System;

using Junior.Common;
using Junior.Persist.Data;
using Junior.Persist.Sessions.Sessions;
using Junior.Persist.Sessions.Transactions;

namespace Junior.Persist.Persistence.Sessions
{
	/// <summary>
	/// A transactional session manager that uses <see cref="CacheKey"/> as its cache key type.
	/// </summary>
	public class TransactionalCacheKeySessionManager : CacheKeySessionManager<ITransactionalCacheKeySession>, ITransactionalCacheKeySessionManager
	{
		private readonly ITransactionManager _transactionManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionalCacheKeySessionManager"/> class.
		/// </summary>
		/// <param name="transactionManager">The current transaction manager.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="transactionManager"/> is null.</exception>
		public TransactionalCacheKeySessionManager(ITransactionManager transactionManager)
		{
			transactionManager.ThrowIfNull("transactionManager");

			_transactionManager = transactionManager;
		}

		/// <summary>
		/// Gets the current session context.
		/// </summary>
		protected override ITransactionalCacheKeySession Current
		{
			get
			{
				return TransactionalCacheKeySession.Current;
			}
		}

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <returns>A session.</returns>
		public override ITransactionalCacheKeySession Enroll()
		{
			return new TransactionalCacheKeySession(_transactionManager.Enlist());
		}

		/// <summary>
		/// Enrolls in a session. An existing session context is used if there is one; otherwise, a new session context is created.
		/// </summary>
		/// <param name="observer">An observer that will receive observed session actions.</param>
		/// <returns>A session.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
		public override ITransactionalCacheKeySession Enroll(ISessionObserver<CacheKey, object> observer)
		{
			observer.ThrowIfNull("observer");

			return new TransactionalCacheKeySession(_transactionManager.Enlist(), observer);
		}
	}
}