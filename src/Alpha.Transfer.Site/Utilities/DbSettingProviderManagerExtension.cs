
using Fanex.Data;
using Alpha.Transfer.Configurations;

namespace Alpha.Transfer.Site.Utilities
{
    public static class DbSettingProviderManagerExtension
    {   
        public static DbSettingProviderManager UseShardDbSettingProvider(this DbSettingProviderManager dbSettingProviderManager)
            => dbSettingProviderManager.Use(new ShardDbSettingProvider(new ShardInfoAccessor(),
                    resourcePath: AppSettings.StoreProcedurePath));

    }
}