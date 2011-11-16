namespace Junior.Persist.Sessions.Transactions
{
	/// <summary>
	/// A transaction that takes no action whether it's committed or rolled back.
	/// </summary>
	public class NullTransaction : ITransaction
	{
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		/// <summary>
		/// Commits the transaction.
		/// </summary>
		public void Commit()
		{
		}
	}
}