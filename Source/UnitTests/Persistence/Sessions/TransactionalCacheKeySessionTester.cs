using Junior.Persist.Persistence.Sessions;
using Junior.Persist.Sessions.Transactions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Persist.UnitTests.Persistence.Sessions
{
	public static class TransactionalCacheKeySessionTester
	{
		[TestFixture]
		public class When_committing_transactional_session
		{
			[Test]
			public void Must_commit_transaction()
			{
				var transaction = MockRepository.GenerateMock<ITransaction>();
				var transactionManager = MockRepository.GenerateMock<ITransactionManager>();
				var sessionManager = new TransactionalCacheKeySessionManager(transactionManager);

				transactionManager.Stub(arg => arg.Enlist()).Return(transaction);
				ITransactionalCacheKeySession systemUnderTest = sessionManager.Enroll();

				systemUnderTest.Commit();

				transaction.AssertWasCalled(arg => arg.Commit());
			}
		}
	}
}