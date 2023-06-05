using Sunplus.Agent.Model.Entities;
using System;
using System.Web;

namespace Alpha.Transfer.Site.Utilities
{
    public interface IShardInfoAccessor
    {
        bool TryUseShardDB(string key, out string dbServer);
    }

    public class ShardInfoAccessor : IShardInfoAccessor
    {
        const string PrefixSiteDB = "sitedb_";

        public bool TryUseShardDB(string key, out string dbServer)
        {
            if (IsBelongSiteDB(key))
            {
                dbServer = SiteDBServer;
                return true;
            }
            dbServer = string.Empty;

            return false;
        }

        private static bool IsBelongSiteDB(string key) => key.StartsWith(PrefixSiteDB, StringComparison.OrdinalIgnoreCase);

        private static string SiteDBServer => UserSession.Current.AGIP;
    }
}
