namespace Alpha.Transfer.Site
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Logging;
    using App_Start;
    using Auth.Filters;
    using Configurations;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using Web;
    using Web.Filters;
    using Web.Helpers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyConfig.Init();

            ModelBinders.Binders.DefaultBinder = new QueryEntityModelBinder();

            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(resourcePath: AppSettings.StoreProcedurePath, enableWatching: true)
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            AppSettings.Verify();

            ShareSession.Init();
            LoggingConfig.Init(getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new HttpContextHelper());

            GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));
            GlobalFilters.Filters.Add(new CheckBelongingFilter());
            GlobalFilters.Filters.Add(new AfterLoginFlowFilter());

            StaticResources.Init(GetBuildVersion());
        }

        protected void Application_Error()
        {
            if (Request.IsLocal)
            {
                return;
            }

            Exception ex = Server.GetLastError();
            var currentCustomer = UserSession.Current;

            ExceptionHandler.Handle(ex,
                new Dictionary<string, object>
                {
                    ["CustId"] = currentCustomer?.CustId,
                    ["CustName"] = currentCustomer?.UserName,
                    ["SubAccountName"] = currentCustomer?.SubUserName,
                    ["AdminName"] = currentCustomer?.AdminName
                });
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}