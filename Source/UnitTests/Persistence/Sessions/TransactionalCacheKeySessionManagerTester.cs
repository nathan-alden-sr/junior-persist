using System;

using Junior.Persist.Data;
using Junior.Persist.Persistence.Sessions;
using Junior.Persist.Sessions.Sessions;
using Junior.Persist.Sessions.Transactions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Persist.UnitTests.Persistence.Sessions
{
	public static class TransactionalCacheKeySessionManagerTester
	{
		[TestFixture]
		public class When_enrolling_in_nested_nontransactional_session
		{
			[Test]
			public void Must_not_share_same_session()
			{
				var transactionManager = MockRepository.GenerateMock<ITransactionManager>();
				var transactionalSessionManager = new TransactionalCacheKeySessionManager(transactionManager);
				var transaction = MockRepository.GenerateMock<ITransaction>();

				transactionManager.Stub(arg => arg.Enlist(Arg<EnlistmentOption>.Is.Anything)).Return(transaction);

				var systemUnderTest = new NonTransactionalCacheKeySessionManager();

				using (transactionalSessionManager.Enroll())
				{
					using (systemUnderTest.Enroll())
					{
						Assert.That(NonTransactionalCacheKeySession.Current, Is.Not.SameAs(TransactionalCacheKeySession.Current));
					}
				}
			}
		}

		[TestFixture]
		public class When_enrolling_in_nested_transactional_session
		{
			[Test]
			public void Must_share_same_session()
			{
				var transactionManager = MockRepository.GenerateMock<ITransactionManager>();
				var transactionalSessionManager = new TransactionalCacheKeySessionManager(transactionManager);
				var transaction = MockRepository.GenerateMock<ITransaction>();

				transactionManager.Stub(arg => arg.Enlist(Arg<EnlistmentOption>.Is.Anything)).Return(transaction);

				var systemUnderTest = new TransactionalCacheKeySessionManager(transactionManager);

				var cacheKey = new CacheKey("Test");

				using (transactionalSessionManager.Enroll())
				{
					var cacheEntity = new CacheEntity<object>(new object(), Guid.NewGuid());

					transactionalSessionManager.EntityWasFound(cacheKey, cacheEntity);

					using (systemUnderTest.Enroll())
					{
						CacheEntity<object> foundEntity = systemUnderTest.GetEntity(cacheKey);

						Assert.That(foundEntity.Entity, Is.SameAs(cacheEntity.Entity));
					}
				}
			}
		}

		[TestFixture]
		public class When_enrolling_in_transactional_session
		{
			[Test]
			public void Must_enlist_in_transaction()
			{
				var transactionManager = MockRepository.GenerateMock<ITransactionManager>();
				var transaction = MockRepository.GenerateMock<ITransaction>();

				transactionManager.Stub(arg => arg.Enlist()).Return(transaction);

				var systemUnderTest = new TransactionalCacheKeySessionManager(transactionManager);

				systemUnderTest.Enroll();

				transactionManager.AssertWasCalled(arg => arg.Enlist());
			}
		}
	}
}