namespace Junior.Persist.Sessions.Transactions
{
	/// <summary>
	/// Indicates how transaction enlistment should occur.
	/// </summary>
	public enum EnlistmentOption
	{
		/// <summary>
		/// Use an ambient transaction, if present; otherwise, enlist in a new transaction.
		/// </summary>
		AmbientOrNew,
		/// <summary>
		/// Always enlist in a new transaction.
		/// </summary>
		AlwaysNew
	}
}