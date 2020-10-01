namespace Alpha.Transfer.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;

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

        public static string AllowedTotalBalanceTransferSuperIds => GetConfigValue("AllowedTotalBalanceTransferSuperIds");

        public static string TransferRedisConfigurationOptions => GetConfigValue("RedisConfigurationOptions:Alpha.Transfer");

        public static string SiteGroup => GetConfigValue("SiteGroup");

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
    }
}