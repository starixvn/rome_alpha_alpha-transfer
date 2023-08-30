using Fanex.Data;
using System.Data.SqlClient;

namespace Alpha.Transfer.Site.Utilities
{
    public class ShardDbSettingProvider : XmlDbSettingProvider
    {
        private readonly IShardInfoAccessor _shardInfoAccessor;

        public ShardDbSettingProvider(IShardInfoAccessor shardInfoAccessor, string resourcePath)
            : base(resourcePath)
        {
            _shardInfoAccessor = shardInfoAccessor;
        }

        public override IDbSetting GetSetting(string key)
        {
            var dbServer = string.Empty;
            if (_shardInfoAccessor.TryUseShardDB(key, out dbServer))
            {
                var originalShardSetting = base.GetSetting(key);
                var connectionStr = new SqlConnectionStringBuilder(originalShardSetting.ConnectionString)
                {
                    DataSource = dbServer
                };

                var dbSetting = new DbSetting(
                    originalShardSetting.Key,
                    originalShardSetting.DbName,
                    connectionStr.ToString(),
                    originalShardSetting.SpName,
                    originalShardSetting.CommandTimeout,
                    originalShardSetting.ColumnMaps,
                    originalShardSetting.FirmParameters);

                return dbSetting;
            }

            return base.GetSetting(key);
        }
    }
}