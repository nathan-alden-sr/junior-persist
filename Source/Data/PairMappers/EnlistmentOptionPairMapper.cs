using System.Transactions;

using Junior.Common;
using Junior.Persist.Sessions.Transactions;

namespace Junior.Persist.Data.PairMappers
{
	/// <summary>
	/// A pair mapper that maps between <see cref="EnlistmentOption"/> and <see cref="TransactionScopeOption"/>.
	/// </summary>
	public class EnlistmentOptionPairMapper : PairMapper<EnlistmentOption, TransactionScopeOption>
	{
		/// <summary>
		/// The singleton instance of <see cref="EnlistmentOptionPairMapper"/>.
		/// </summary>
		public static readonly EnlistmentOptionPairMapper Instance = new EnlistmentOptionPairMapper();

		private EnlistmentOptionPairMapper()
			: base(
				new Pair<EnlistmentOption, TransactionScopeOption>(EnlistmentOption.AlwaysNew, TransactionScopeOption.RequiresNew),
				new Pair<EnlistmentOption, TransactionScopeOption>(EnlistmentOption.AmbientOrNew, TransactionScopeOption.Required))
		{
		}
	}
}