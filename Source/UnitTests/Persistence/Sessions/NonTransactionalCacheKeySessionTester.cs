using System;
using System.Diagnostics;
using System.Linq;

using Junior.Ddd.DomainModel;
using Junior.Persist.Data;
using Junior.Persist.Persistence.Sessions;
using Junior.Persist.Sessions.Sessions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Persist.UnitTests.Persistence.Sessions
{
	public static class NonTransactionalCacheKeySessionTester
	{
		[DebuggerDisplay("{_id}")]
		private class Entity1
		{
#pragma warning disable 169
			private Guid _id = Guid.NewGuid();
#pragma warning restore 169
		}

		[DebuggerDisplay("{_id}")]
		private class Entity2
		{
#pragma warning disable 169
			private Guid _id = Guid.NewGuid();
#pragma warning restore 169
		}

		public abstract class NonTransactionalCacheKeySessionTestFixture
		{
			private readonly NonTransactionalCacheKeySessionManager _sessionManager = new NonTransactionalCacheKeySessionManager();

			public NonTransactionalCacheKeySessionManager SessionManager
			{
				get
				{
					return _sessionManager;
				}
			}
		}

		[TestFixture]
		public class When_adding_created_lazy_entity_to_session : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_add_lazy_entity_if_it_is_not_cached()
			{
				using (INonTransactionalCacheKeySession systemUnderTest = SessionManager.Enroll())
				{
					var lazyEntity = new LazyEntity<object>(() => new object());
					Guid entityId = Guid.NewGuid();

					systemUnderTest.LazyEntityWasCreated(lazyEntity, entityId);

					Assert.That(systemUnderTest.GetEntityId(lazyEntity), Is.EqualTo(entityId));
				}
			}

			[Test]
			public void Must_throw_exception_if_caching_the_same_lazy_entity_more_than_once()
			{
				using (INonTransactionalCacheKeySession systemUnderTest = SessionManager.Enroll())
				{
					var lazyEntity = new LazyEntity<object>(() => new object());

					systemUnderTest.LazyEntityWasCreated(lazyEntity, Guid.NewGuid());

					Assert.Throws<SessionException>(() => systemUnderTest.LazyEntityWasCreated(lazyEntity, Guid.NewGuid()), "Cannot cache lazy entity more than once.");
				}
			}
		}

		[TestFixture]
		public class When_adding_found_entities_to_session : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_replace_cached_entities_with_new_entity_instances()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheKey = new CacheKey("test");
					var cacheEntityA1 = new CacheEntity<object>(new Entity1(), Guid.NewGuid());
					var cacheEntityA2 = new CacheEntity<object>(new Entity1(), Guid.NewGuid());
					var cacheEntityB1 = new CacheEntity<object>(new Entity1(), cacheEntityA1.Id);
					var cacheEntityB2 = new CacheEntity<object>(new Entity1(), cacheEntityA2.Id);

					observer.Expect(arg => arg.EntityFound(session, cacheEntityA1.GetType(), cacheEntityA1.Id)).Repeat.Once();
					observer.Expect(arg => arg.EntityFound(session, cacheEntityA2.GetType(), cacheEntityA2.Id)).Repeat.Once();
					observer.Expect(arg => arg.EntityFound(session, cacheEntityB1.GetType(), cacheEntityB1.Id)).Repeat.Once();
					observer.Expect(arg => arg.EntityFound(session, cacheEntityB2.GetType(), cacheEntityB2.Id)).Repeat.Once();

					session.EntitiesWereFound(cacheKey, new[] { cacheEntityA1, cacheEntityA2 });
					session.EntitiesWereFound(cacheKey, new[] { cacheEntityB1, cacheEntityB2 });
				}
			}

			[Test]
			public void Must_throw_exception_if_caching_more_than_one_entity_type_for_single_cache_key()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					Assert.Throws<SessionException>(
						() => session.EntitiesWereFound(
							new CacheKey("test"),
							new[]
								{
									new CacheEntity<object>(new object(), Guid.NewGuid()),
									new CacheEntity<object>(new Entity1(), Guid.NewGuid())
								}),
						"At least one found entity has a different ID than the same entity in the cache.");
				}
			}

			[Test]
			public void Must_throw_exception_if_caching_more_than_one_entity_type_with_the_same_id()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					Guid id = Guid.NewGuid();

					Assert.Throws<SessionException>(
						() => session.EntitiesWereFound(
							new CacheKey("test"),
							new[]
								{
									new CacheEntity<object>(new Entity1(), id),
									new CacheEntity<object>(new object(), id)
								}),
						"Cannot cache more than one type of entity per cache key.");
				}
			}

			[Test]
			public void Must_throw_exception_if_caching_the_same_entity_more_than_once()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					var entity = new Entity1();

					Assert.Throws<SessionException>(
						() => session.EntitiesWereFound(
							new CacheKey("test"),
							new[]
								{
									new CacheEntity<object>(entity, Guid.NewGuid()),
									new CacheEntity<object>(entity, Guid.NewGuid())
								}),
						"Cannot cache same entity more than once.");
				}
			}

			[Test]
			public void Must_throw_exception_if_same_entity_is_added_with_different_id()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					var cacheKey = new CacheKey("test");
					var entity = new Entity1();
					var cacheEntity1 = new CacheEntity<object>(entity, Guid.NewGuid());
					var cacheEntity2 = new CacheEntity<object>(entity, Guid.NewGuid());

					session.EntityWasPersisted(cacheEntity1);
					Assert.Throws<SessionException>(() => session.EntityWasFound(cacheKey, cacheEntity2));
				}
			}
		}

		[TestFixture]
		public class When_adding_persisted_entities_to_session : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_add_entity_if_entity_is_not_cached()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					Guid id = Guid.NewGuid();
					var entity = new Entity1();

					session.EntityWasPersisted(new CacheEntity<object>(entity, id));

					observer.AssertWasCalled(arg => arg.EntityPersisted(session, entity.GetType(), id));
				}
			}

			[Test]
			public void Must_not_throw_exception_if_same_entity_is_added_more_than_once()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheEntity = new CacheEntity<object>(new Entity1(), Guid.NewGuid());

					session.EntityWasPersisted(cacheEntity);
					Assert.DoesNotThrow(() => session.EntityWasPersisted(cacheEntity));

					observer.AssertWasCalled(arg => arg.EntityPersisted(session, new Entity1().GetType(), cacheEntity.Id), options => options.Repeat.Twice());
				}
			}

			[Test]
			public void Must_throw_exception_if_same_entity_is_added_with_different_id()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					var entity = new Entity1();
					var cacheEntity1 = new CacheEntity<object>(entity, Guid.NewGuid());
					var cacheEntity2 = new CacheEntity<object>(entity, Guid.NewGuid());

					session.EntityWasPersisted(cacheEntity1);
					Assert.Throws<SessionException>(() => session.EntityWasPersisted(cacheEntity2));
				}
			}
		}

		[TestFixture]
		public class When_clearing_all_entities_from_session : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_clear_all_entities()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheKey1 = new CacheKey("test1");
					var cacheKey2 = new CacheKey("test2");
					var cacheEntities1 = new[]
					                     	{
					                     		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                     		new CacheEntity<object>(new Entity1(), Guid.NewGuid())
					                     	};
					var cacheEntities2 = new[]
					                     	{
					                     		new CacheEntity<object>(new Entity2(), Guid.NewGuid()),
					                     		new CacheEntity<object>(new Entity2(), Guid.NewGuid())
					                     	};

					foreach (var cacheEntity in cacheEntities1.Concat(cacheEntities2))
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityFound(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}
					foreach (var cacheEntity in cacheEntities1.Concat(cacheEntities2))
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityRemoved(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}

					foreach (var cacheEntity in cacheEntities1)
					{
						session.EntityWasFound(cacheKey1, cacheEntity);
					}
					foreach (var cacheEntity in cacheEntities2)
					{
						session.EntityWasFound(cacheKey2, cacheEntity);
					}
					session.ClearAll();
				}

				observer.VerifyAllExpectations();
			}
		}

		[TestFixture]
		public class When_clearing_entities_of_a_type_from_session : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_clear_entities_of_that_type()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheKey1 = new CacheKey("test1");
					var cacheKey2 = new CacheKey("test2");
					var cacheKey3 = new CacheKey("test3");
					var cacheEntities1 = new[]
					                     	{
					                     		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                     		new CacheEntity<object>(new Entity1(), Guid.NewGuid())
					                     	};
					var cacheEntities2 = new[]
					                     	{
					                     		new CacheEntity<object>(new Entity2(), Guid.NewGuid()),
					                     		new CacheEntity<object>(new Entity2(), Guid.NewGuid())
					                     	};
					var cacheEntities3 = new[]
					                     	{
					                     		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                     		new CacheEntity<object>(new Entity1(), Guid.NewGuid())
					                     	};

					foreach (var cacheEntity in cacheEntities1.Concat(cacheEntities2).Concat(cacheEntities3))
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityFound(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}
					foreach (var cacheEntity in cacheEntities1.Concat(cacheEntities3))
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityRemoved(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}

					foreach (var cacheEntity in cacheEntities1)
					{
						session.EntityWasFound(cacheKey1, cacheEntity);
					}
					foreach (var cacheEntity in cacheEntities2)
					{
						session.EntityWasFound(cacheKey2, cacheEntity);
					}
					foreach (var cacheEntity in cacheEntities3)
					{
						session.EntityWasFound(cacheKey3, cacheEntity);
					}
					session.Clear<Entity1>();
				}

				observer.VerifyAllExpectations();
			}
		}

		[TestFixture]
		public class When_removing_entities_from_session : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_remove_cache_keys_and_all_their_entities_for_cache_keys_referencing_a_removed_entity()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheKey = new CacheKey("test");
					var cacheEntities = new[]
					                    	{
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid())
					                    	};
					var addedCacheEntity = new CacheEntity<object>(new Entity1(), Guid.NewGuid());

					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityFound(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}
					observer.Expect(arg => arg.EntityPersisted(session, addedCacheEntity.Entity.GetType(), addedCacheEntity.Id)).Repeat.Once();
					observer.Expect(arg => arg.EntityRemoved(session, addedCacheEntity.Entity.GetType(), addedCacheEntity.Id)).Repeat.Once();
					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityRemoved(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}

					session.EntitiesWereFound(cacheKey, cacheEntities);
					session.EntityWasPersisted(addedCacheEntity);
					session.RemoveEntities(new[]
					                            	{
					                            		addedCacheEntity.Entity,
					                            		cacheEntities[1].Entity,
					                            		cacheEntities[3].Entity
					                            	});
				}

				observer.VerifyAllExpectations();
			}

			[Test]
			public void Must_remove_entities_if_entities_are_cached()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheEntities = new[]
					                    	{
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid())
					                    	};

					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityPersisted(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}
					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityRemoved(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}

					foreach (var cacheEntity in cacheEntities)
					{
						session.EntityWasPersisted(cacheEntity);
					}
					session.RemoveEntities(cacheEntities.Select(arg => arg.Entity));
				}

				observer.VerifyAllExpectations();
			}

			[Test]
			public void Must_throw_exception_if_entity_being_removed_is_not_cached()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					var entity = new Entity1();

					Assert.Throws<SessionException>(() => session.RemoveEntity(entity));
				}
			}
		}

		[TestFixture]
		public class When_removing_entities_from_session_by_entity_id : NonTransactionalCacheKeySessionTestFixture
		{
			[Test]
			public void Must_remove_cache_keys_and_all_their_entities_for_cache_keys_referencing_a_removed_entity()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					var cacheKey = new CacheKey("test");
					Guid entityId = Guid.NewGuid();
					var cacheEntities = new[]
					                    	{
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), entityId),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid())
					                    	};
					var addedCacheEntity = new CacheEntity<object>(new Entity1(), Guid.NewGuid());

					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityFound(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}
					observer.Expect(arg => arg.EntityPersisted(session, addedCacheEntity.Entity.GetType(), addedCacheEntity.Id)).Repeat.Once();
					observer.Expect(arg => arg.EntityRemoved(session, addedCacheEntity.Entity.GetType(), addedCacheEntity.Id)).Repeat.Once();
					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityRemoved(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}

					session.EntitiesWereFound(cacheKey, cacheEntities);
					session.EntityWasPersisted(addedCacheEntity);
					session.RemoveEntity(entityId);
				}

				observer.VerifyAllExpectations();
			}

			[Test]
			public void Must_remove_entities_if_entities_are_cached()
			{
				var observer = MockRepository.GenerateMock<ISessionObserver<CacheKey, object>>();

				using (INonTransactionalCacheKeySession session = SessionManager.Enroll(observer))
				{
					Guid entityId = Guid.NewGuid();
					var cacheEntities = new[]
					                    	{
					                    		new CacheEntity<object>(new Entity1(), Guid.NewGuid()),
					                    		new CacheEntity<object>(new Entity1(), entityId)
					                    	};

					foreach (var cacheEntity in cacheEntities)
					{
						CacheEntity<object> tempCacheEntity = cacheEntity;

						observer.Expect(arg => arg.EntityPersisted(session, tempCacheEntity.Entity.GetType(), tempCacheEntity.Id)).Repeat.Once();
					}
					observer.Expect(arg => arg.EntityRemoved(session, cacheEntities[1].Entity.GetType(), cacheEntities[1].Id)).Repeat.Once();

					foreach (var cacheEntity in cacheEntities)
					{
						session.EntityWasPersisted(cacheEntity);
					}
					session.RemoveEntity(entityId);
				}

				observer.VerifyAllExpectations();
			}

			[Test]
			public void Must_not_throw_exception_if_entity_being_removed_is_not_cached()
			{
				using (INonTransactionalCacheKeySession session = SessionManager.Enroll())
				{
					Assert.DoesNotThrow(() => session.RemoveEntity(Guid.NewGuid()));
				}
			}
		}
	}
}