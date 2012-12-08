namespace Junior.Persist.Sessions.Transactions
{
	/// <summary>
	/// Indicates the transaction isolation level to use.
	/// </summary>
	public enum IsolationLevel
	{
		/// <summary>
		/// Chaos.
		/// </summary>
		Chaos,
		/// <summary>
		/// Read-committed.
		/// </summary>
		ReadCommitted,
		/// <summary>
		/// Read-uncommitted.
		/// </summary>
		ReadUncommitted,
		/// <summary>
		/// Repeatable read.
		/// </summary>
		RepeatableRead,
		/// <summary>
		/// Serializable.
		/// </summary>
		Serializable,
		/// <summary>
		/// Snapshot.
		/// </summary>
		Snapshot,
		/// <summary>
		/// Unspecified.
		/// </summary>
		Unspecified
	}
}