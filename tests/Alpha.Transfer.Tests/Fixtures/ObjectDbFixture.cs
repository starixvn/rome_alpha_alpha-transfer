namespace Alpha.Transfer.Tests.Fixtures
{
    using Fanex.Data;
    using NSubstitute;

    public class ObjectDbFixture
    {
        public ObjectDbFixture()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((settingKey, commandTimeout) => ObjectDb);
        }

        public IObjectDb ObjectDb { get; private set; }
    }
}