namespace Alpha.Transfer.Site.Tests.Collections
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("UserCollection")]
    public class UserCollection : ICollectionFixture<UserFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
