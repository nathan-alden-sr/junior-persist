using System;

using Junior.Persist.Persistence;

using NUnit.Framework;

namespace Junior.Persist.UnitTests.Persistence
{
	public static class LazyEntitiesByParentIdTester
	{
		[TestFixture]
		public class When_retrieving_lazy_entity_references
		{
			[Test]
			public void Must_initialize_using_func_supplied_in_constructor()
			{
				var childEntities = new[]
					{
						new object(),
						new object()
					};
				var systemUnderTest = new LazyEntitiesByParentId<object>(
					Guid.NewGuid(),
					entity => childEntities);

				Assert.That(systemUnderTest.Value, Is.SameAs(childEntities));
			}
		}
	}
}