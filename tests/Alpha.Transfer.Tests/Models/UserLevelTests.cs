namespace Alpha.Transfer.Tests.Models
{
    using System;
    using Constants;
    using TestCases;
    using Transfer.Models;
    using Xunit;

    public class UserLevelTests
    {
        [Fact]
        public void IsAgentLevel_UserLevelIsAgent_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsAgentLevel();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNotAgentLevel_UserLevelIsAgent_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsNotAgentLevel();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMasterLevel_UserLevelIsMaster_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsMasterLevel();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNotMasterLevel_UserLevelIsMaster_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsNotMasterLevel();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSuperLevel_UserLevelIsSuper_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsSuperLevel();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public void IsMasterOrSuperLevel_UserLevelIsSuperOrMaster_ReturnsTrue(byte levelId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);

            // Act
            var result = userLevel.IsMasterOrSuperLevel();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMemberLevel_UserLevelIsMember_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Member.Value);

            // Act
            var result = userLevel.IsMemberLevel();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNotSuperLevel_UserLevelIsSuper_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsNotSuperLevel();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDirectUplineOf_CustomerLevelIsNull_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsDirectUplineOf(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectUplineOf_Always_ReturnsValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsDirectUplineOf(downlineLevelValue);

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectUplineOf_ValidCustomerLevel_ReturnsValidValue(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Act
            var actualResult = uplineLevel
                .IsDirectUplineOf(Enumeration.FromValue<UserLevel>(downlineLevelValue));

            // Assert
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void IsDirectDownlineOf_CustomerLevelIsNull_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsDirectDownlineOf(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData("DownlineUserLevelAndUplineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectDownlineOf_CustomerLevelIsNotNull_ReturnsValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
        {
            // Arrange
            var uplineLevel = Enumeration.FromValue<UserLevel>(uplineLevelValue);

            // Act
            var actualResult = downlineLevel.IsDirectDownlineOf(uplineLevel);

            // Assert
            Assert.Equal(result, actualResult);
        }

        [Theory]
        [InlineData(RoleId.Super)]
        [InlineData(RoleId.Master)]
        [InlineData(RoleId.Agent)]
        public void Is_UserLevelIsNull_ReturnsFalse(byte userLevelValue)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);

            // Act
            var result = userLevel.Is(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData("SameUseLevelDataSource", MemberType = typeof(UserLevelTestCases))]
        public void Is_UserLevelIsNotNull_ReturnsExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.Is(userLevel2);

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("SameUseLevelDataSource", MemberType = typeof(UserLevelTestCases))]
        public void IsNot_UserLevelIsNotNull_ReturnsExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.IsNot(userLevel2);

            Assert.Equal(!result, actualResult);
        }

        [Theory]
        [InlineData(RoleId.Super, RoleId.Master)]
        [InlineData(RoleId.Master, RoleId.Agent)]
        [InlineData(RoleId.Agent, RoleId.Member)]
        public void GetDirectDownLineLevel_SMALevel_ReturnsDirectDownline(byte roleId, byte expectedDirectDownlineRoleId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var actualDirectDownlineRoleId = userLevel.GetDirectDownLineLevel().Value;

            // Assert
            Assert.Equal(expectedDirectDownlineRoleId, actualDirectDownlineRoleId);
        }

        [Fact]
        public void GetDirectDownLineLevel_MemberLevel_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(RoleId.Member);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => userLevel.GetDirectDownLineLevel());
        }

        [Theory]
        [InlineData(RoleId.Master, RoleId.Super)]
        [InlineData(RoleId.Agent, RoleId.Master)]
        [InlineData(RoleId.Member, RoleId.Agent)]
        public void GetDirectUplineLevel_MAMbLevel_ReturnsDirectUpline(byte roleId, byte expectedDirectUplineRoleId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var actualDirectUplineRoleId = userLevel.GetDirectUplineLevel().Value;

            // Assert
            Assert.Equal(expectedDirectUplineRoleId, actualDirectUplineRoleId);
        }

        [Fact]
        public void GetDirectUplineLevel_SuperLevel_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(RoleId.Super);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => userLevel.GetDirectUplineLevel());
        }

        [Fact]
        public void IsUplineOf_GapBetweenTwoLevelsLessThan1_ReturnsFalse()
        {
            // Arrange
            var userLevel = UserLevel.Master;

            // Act
            var actualIsUplineOf = userLevel.IsUplineOf(4);

            // Assert
            Assert.False(actualIsUplineOf);
        }

        [Fact]
        public void IsUplineOf_GapBetweenTwoLevelsFrom1_ReturnsTrue()
        {
            // Arrange
            var userLevel = UserLevel.Master;

            // Act
            var actualIsUplineOf = userLevel.IsUplineOf(2);

            // Assert
            Assert.True(actualIsUplineOf);
        }

        [Fact]
        public void IsUplineOf_DownlineLevelIsNull_ReturnsFalse()
        {
            // Arrange
            var userLevel = UserLevel.Master;
            UserLevel downlineLevel = null;

            // Act
            var actualIsUplineOf = userLevel.IsUplineOf(downlineLevel);

            // Assert
            Assert.False(actualIsUplineOf);
        }

        [Fact]
        public void IsUplineOf_ValidDownline_ReturnsTrue()
        {
            // Arrange
            var userLevel = UserLevel.Master;
            var downlineLevel = UserLevel.Agent;

            // Act
            var actualIsUplineOf = userLevel.IsUplineOf(downlineLevel);

            // Assert
            Assert.True(actualIsUplineOf);
        }

        [Fact]
        public void IsEqualOrUplineOf_Equal_ReturnsTrue()
        {
            // Arrange
            var userLevel = UserLevel.Master;
            var custLevel = UserLevel.Master;

            // Act
            var actualIsUplineOf = userLevel.IsEqualOrUplineOf(custLevel);

            // Assert
            Assert.True(actualIsUplineOf);
        }

        [Fact]
        public void IsEqualOrUplineOf_Upline_ReturnsTrue()
        {
            // Arrange
            var userLevel = UserLevel.Master;
            var custLevel = UserLevel.Agent;

            // Act
            var actualIsUplineOf = userLevel.IsEqualOrUplineOf(custLevel);

            // Assert
            Assert.True(actualIsUplineOf);
        }

        [Fact]
        public void IsEqualOrUplineOf_NotEqualAndUpline_ReturnsFalse()
        {
            // Arrange
            var userLevel = UserLevel.Master;
            var custLevel = UserLevel.Super;

            // Act
            var actualIsUplineOf = userLevel.IsEqualOrUplineOf(custLevel);

            // Assert
            Assert.False(actualIsUplineOf);
        }
    }
}
