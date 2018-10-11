namespace Alpha.Transfer.Tests.Fixtures
{
    using Fanex.Data;
    using NSubstitute;
    using Transfer.DataAccesses;

    public class ObjectDbFixture
    {
        public ObjectDbFixture()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            TransferObjectDbFactory.CreateInstance = (settingKey) => ObjectDb;
        }

        public IObjectDb ObjectDb { get; private set; }
    }
}
