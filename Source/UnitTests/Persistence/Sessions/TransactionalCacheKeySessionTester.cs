using System.Threading.Tasks;

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
			public async void Must_commit_transaction()
			{
				var transaction = MockRepository.GenerateMock<ITransaction>();
				var transactionManager = MockRepository.GenerateMock<ITransactionManager>();
				var sessionManager = new TransactionalCacheKeySessionManager(transactionManager);

				transaction.Stub(arg => arg.Commit()).Return(Task.FromResult((object)null));
				transactionManager.Stub(arg => arg.Enlist()).Return(Task.FromResult(transaction));

				ITransactionalCacheKeySession systemUnderTest = await sessionManager.Enroll();

				await systemUnderTest.Commit();

				transaction.AssertWasCalled(arg => arg.Commit());
			}
		}
	}
}