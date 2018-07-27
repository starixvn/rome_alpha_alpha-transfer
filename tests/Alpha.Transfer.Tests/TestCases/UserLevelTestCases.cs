namespace Alpha.Transfer.Tests.TestCases
{
    using System.Collections.Generic;
    using Transfer.Models;

    public static class UserLevelTestCases
    {
        private static IEnumerable<object[]> uplineUserLevelAndDownlineValueDataSources = new List<object[]>()
        {
            new object[] { UserLevel.Super,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Super,  UserLevel.Master.Value, true },
            new object[] { UserLevel.Super,  UserLevel.Agent.Value, false },
            new object[] { UserLevel.Master, UserLevel.Super.Value, false },
            new object[] { UserLevel.Master, UserLevel.Master.Value, false },
            new object[] { UserLevel.Master, UserLevel.Agent.Value, true },
            new object[] { UserLevel.Agent,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Master.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Agent.Value, false }
        };

        private static IEnumerable<object[]> downlineUserLevelAndUplineValueDataSources = new List<object[]>()
        {
            new object[] { UserLevel.Super,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Super,  UserLevel.Master.Value, false },
            new object[] { UserLevel.Super,  UserLevel.Agent.Value, false },
            new object[] { UserLevel.Master, UserLevel.Super.Value, true },
            new object[] { UserLevel.Master, UserLevel.Master.Value, false },
            new object[] { UserLevel.Master, UserLevel.Agent.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Master.Value, true },
            new object[] { UserLevel.Agent,  UserLevel.Agent.Value, false }
        };

        private static IEnumerable<object[]> sameUseLevelDataSource = new List<object[]>()
        {
            new object[] { UserLevel.Super,  UserLevel.Super, true },
            new object[] { UserLevel.Super,  UserLevel.Master, false },
            new object[] { UserLevel.Super,  UserLevel.Agent, false },
            new object[] { UserLevel.Master, UserLevel.Super, false },
            new object[] { UserLevel.Master, UserLevel.Master, true },
            new object[] { UserLevel.Master, UserLevel.Agent, false },
            new object[] { UserLevel.Agent,  UserLevel.Super, false },
            new object[] { UserLevel.Agent,  UserLevel.Master, false },
            new object[] { UserLevel.Agent,  UserLevel.Agent, true }
        };

        public static IEnumerable<object[]> UplineUserLevelAndDownlineValueDataSources => uplineUserLevelAndDownlineValueDataSources;

        public static IEnumerable<object[]> DownlineUserLevelAndUplineValueDataSources => downlineUserLevelAndUplineValueDataSources;

        public static IEnumerable<object[]> SameUseLevelDataSource => sameUseLevelDataSource;
    }
}

