using System;
using System.Threading.Tasks;

using Junior.Persist.Persistence;
using Junior.Persist.Persistence.Finders;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Persist.UnitTests.Persistence
{
	public static class LazyEntityByIdTester
	{
		[TestFixture]
		public class When_retrieving_lazy_entity_reference_using_finder
		{
			[Test]
			public void Must_retrieve_correct_entity()
			{
				var entity = new object();
				var finder = MockRepository.GenerateMock<IByIdFinder<object>>();
				Guid entityId = Guid.NewGuid();

				finder.Stub(arg => arg.ById(entityId)).Return(Task.FromResult(entity));

				var systemUnderTest = new LazyEntityById<object>(entityId, finder);

				Assert.That(systemUnderTest.Value, Is.SameAs(entity));
			}
		}

		[TestFixture]
		public class When_retrieving_lazy_entity_reference_using_func
		{
			[Test]
			public void Must_retrieve_correct_entity()
			{
				var entity = new object();
				var systemUnderTest = new LazyEntityById<object>(Guid.NewGuid(), (guid, handling) => entity);

				Assert.That(systemUnderTest.Value, Is.SameAs(entity));
			}
		}
	}
}