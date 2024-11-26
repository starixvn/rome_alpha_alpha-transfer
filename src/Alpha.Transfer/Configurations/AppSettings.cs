namespace Alpha.Transfer.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Alpha.Web.Helpers;
    using AlphaPlus.User.Models;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string StoreProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string FanexDataServiceApi => GetConfigValue("FanexDataServiceApi");

        public static string AllowedTotalBalanceTransferSuperIds => GetConfigValue("AllowedTotalBalanceTransferSuperIds");

        public static string TransferRedisConfigurationOptions => GetConfigValue("RedisConfigurationOptions:Alpha.Transfer");

        public static string GetConfigValue(string configName) => AppSettingValues[configName];

        public static void Verify()
        {
            Require(StoreProcedurePath, nameof(StoreProcedurePath));
        }

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }

        public static IEnumerable<string> TransferToDownlineHistoryOfferSites
            => GetConfigValue("TransferToDownlineHistory.OfferSites")
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(site => site.Trim());

        public static bool IsAllowedTransferToDownlineHistorySite(string site)
            => TransferToDownlineHistoryOfferSites.Contains("*", StringComparer.CurrentCultureIgnoreCase)
            || TransferToDownlineHistoryOfferSites.Contains(site, StringComparer.CurrentCultureIgnoreCase);

        public static string VirtualDirectory
        {
            get
            {
                return AppPathHelper.GetAppPath(GetConfigValue("VirtualDirectory"), "~/");
            }
        }

        public static string SiteLabel => GetConfigValue("SiteLabel") ?? string.Empty;

        public static bool IsMBCSite => !SiteLabel.Equals("789y", StringComparison.OrdinalIgnoreCase);

        public static int MultipleTransferCacheTimeOut => int.Parse(GetConfigValue("MultipleTransferCacheTimeOut"));

        public static bool EnableTransferLogInfo => GetConfigValue("EnableTransferLogInfo") == "1";

        public static string TransferLogCategory => GetConfigValue("TransferLogCategory");

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");
        
        public static bool SupportedNewHandbook
        {
            get
            {
                var marketsSupportedNewHandbook = GetConfigValue("MarketsSupportedNewHandbook");
                if (string.IsNullOrWhiteSpace(marketsSupportedNewHandbook))
                {
                    return false;
                }

                if (string.Equals("*", marketsSupportedNewHandbook, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var label = GetConfigValue("SiteLabel");

                return marketsSupportedNewHandbook
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(s => string.Equals(s, label, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}