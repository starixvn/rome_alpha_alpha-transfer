namespace Alpha.Transfer.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Alpha.Web.Helpers;
    using Sunplus.Agent.Model.Entities;

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

        public static bool IsEnableAutoTransfer(string currency, bool isSubAcc, PageAccess subPageAccess)
        {
            if (isSubAcc && !subPageAccess.Transfer)
            {
                return false;
            }

            var s = GetConfigValue("OfferAutoTransfer.Currencies");
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            if (s == "*")
            {
                return true;
            }

            return s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(c => string.Equals(currency, c.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public static string VirtualDirectory
        {
            get
            {
                return AppPathHelper.GetAppPath(GetConfigValue("VirtualDirectory"), "~/");
            }
        }

        public static string SiteGroup => GetConfigValue("SiteLabel") ?? string.Empty;

        public static bool IsMBCSite => !SiteGroup.Equals("789y", StringComparison.OrdinalIgnoreCase);
    }
}