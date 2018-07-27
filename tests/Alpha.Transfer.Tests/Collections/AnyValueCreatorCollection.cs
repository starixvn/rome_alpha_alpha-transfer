namespace Alpha.Transfer.Tests.Collections
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("AnyValueCreatorCollection")]
    public class AnyValueCreatorCollection : ICollectionFixture<AnyValueCreatorFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
