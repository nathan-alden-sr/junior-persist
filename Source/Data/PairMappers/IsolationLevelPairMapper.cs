using Junior.Common;
using Junior.Persist.Sessions.Transactions;

namespace Junior.Persist.Data.PairMappers
{
	/// <summary>
	/// A pair mapper that maps between <see cref="IsolationLevel"/> and <see cref="System.Transactions.IsolationLevel"/>.
	/// </summary>
	public class IsolationLevelPairMapper : PairMapper<IsolationLevel, System.Transactions.IsolationLevel>
	{
		/// <summary>
		/// The singleton instance of <see cref="IsolationLevelPairMapper"/>.
		/// </summary>
		public static readonly IsolationLevelPairMapper Instance = new IsolationLevelPairMapper();

		private IsolationLevelPairMapper()
			: base(
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.Chaos, System.Transactions.IsolationLevel.Chaos),
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.ReadCommitted, System.Transactions.IsolationLevel.ReadCommitted),
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.ReadUncommitted, System.Transactions.IsolationLevel.ReadUncommitted),
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.RepeatableRead, System.Transactions.IsolationLevel.RepeatableRead),
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.Serializable, System.Transactions.IsolationLevel.Serializable),
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.Snapshot, System.Transactions.IsolationLevel.Snapshot),
				new Pair<IsolationLevel, System.Transactions.IsolationLevel>(IsolationLevel.Unspecified, System.Transactions.IsolationLevel.Unspecified))
		{
		}
	}
}