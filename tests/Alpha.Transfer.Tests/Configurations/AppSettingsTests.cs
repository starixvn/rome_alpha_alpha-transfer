namespace Alpha.Transfer.Tests.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using Fixtures;
    using Helpers;
    using Transfer.Configurations;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("AnyValueCreatorCollection")]
    public class AppSettingsTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public AppSettingsTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void StoreProcedurePath_Always_ReturnsValueFromConfig()
        {
            // Arrange
            var storeProcedurePath = anyValueCreatorFixture.Create<string>();
            var excpectedStoreProcedurePath = storeProcedurePath;
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["StoredProcedurePath"] = storeProcedurePath
            };

            // Act
            var actualStoreProcedurePath = AppSettings.StoreProcedurePath;

            // Assert
            Assert.Equal(excpectedStoreProcedurePath, actualStoreProcedurePath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void Verify_NotHaveStoreProcedurePathConfig_ThrowsConfigurationErrorsException(string storeProcedurePath)
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["StoredProcedurePath"] = storeProcedurePath
            };

            // Act
            Exception actualExcelption = ExceptionRecorder.Record(() => AppSettings.Verify());

            // Act & Assert
            Assert.True(actualExcelption is ConfigurationErrorsException);
        }

        [Fact]
        public void Verify_HaveAllRequiredConfig_NotThrowsException()
        {
            // Arrange
            var storeProcedurePath = anyValueCreatorFixture.Create<string>();
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["StoredProcedurePath"] = storeProcedurePath
            };

            // Act
            Exception actualExcelption = ExceptionRecorder.Record(() => AppSettings.Verify());

            // Assert
            Assert.Null(actualExcelption);
        }

        [Fact]
        public void AllowedTotalBalanceTransferSuperIds_Always_ReturnsValueFromConfig()
        {
            // Arrange
            var configValue = anyValueCreatorFixture.Create<string>();
            var excpectedValue = configValue;
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["AllowedTotalBalanceTransferSuperIds"] = configValue
            };

            // Act
            var actualValue = AppSettings.AllowedTotalBalanceTransferSuperIds;

            // Assert
            Assert.Equal(excpectedValue, actualValue);
        }
    }
}
