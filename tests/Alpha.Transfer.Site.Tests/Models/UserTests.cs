namespace Alpha.Transfer.Site.Tests.Models
{
    using System;
    using AlphaPlus.User.Models;
    using NSubstitute;
    using Site.Models;
    using Xunit;

    public class UserTests
    {
        [Fact]
        public void Constructor_Always_GetUserDataFromSession()
        {
            // Arrange
            var customerDateLimit = new DateTime(2021, 4, 1);

            var userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(1);
            userSession.UserName.Returns("username");
            userSession.RoleId.Returns((byte)2);
            userSession.Language.Returns("en-US");
            userSession.SubAccId.Returns(0);
            userSession.UserLoginIp.Returns("192.168.1.1");
            userSession.LangKey.Returns("E");
            userSession.SubUserName.Returns("SubAccName");
            userSession.IsAllowedTotalBalanceTransfer.Returns(false);
            userSession.UserId.Returns(1);
            userSession.IsInternal.Returns(false);
            userSession.GetProperty<DateTime>("CustomerDateLimit").Returns(customerDateLimit);
            userSession.Site.Returns("site");

            // Act
            var user = new User(userSession);

            // Assert
            Assert.True(
                user.Id == 1
                && user.Name == "username"
                && user.LevelId == 2
                && user.Language == "en-US"
                && user.SubAccId == 0
                && user.IP == "192.168.1.1"
                && user.LanguageKey == "E"
                && user.SubAccName == "SubAccName"
                && !user.AllowTotalBalanceTransfer
                && user.UserId == 1
                && !user.IsInternal
                && user.LimitedDateToShowInfo == customerDateLimit
                && user.Site == "site");
        }
    }
}