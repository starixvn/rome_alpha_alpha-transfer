namespace Alpha.Transfer.Site.Tests.Extensions
{
    using Newtonsoft.Json;
    using Xunit;

    public static class AssertExtension
    {
        public static void Equal<T>(T expected, T actual)
        {
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }
    }
}
