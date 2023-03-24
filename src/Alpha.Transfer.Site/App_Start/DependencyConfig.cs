namespace Alpha.Transfer.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using Controllers;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using Fanex.Data.Repository;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using StackExchange.Redis;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;

    public static class DependencyConfig
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);

            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(SiteBaseController).Assembly;
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Singleton);
            container.RegisterSingleton<TransferAllAndClearCreditDataAccess, TransferAllAndClearCreditDataAccessImpl>();
            container.RegisterSingleton<MultipleTransferDataAccess, MultipleTransferDataAccessImpl>();
            container.RegisterSingleton<TransferToDownlineHistoryDataAccess, TransferToDownlineHistoryDataAccessImpl>();
            container.RegisterSingleton<TransferDataAccess, TransferDataAccessImpl>();
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<ICache>(InitRedisCache());
            container.RegisterSingleton<ICacheService, CacheService>();

            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();

            container.RegisterSingleton<TransferAllAndClearCreditService, TransferAllAndClearCreditServiceImpl>();
            container.RegisterPerWebRequest<CashBalanceTransferService, CashBalanceTransferServiceImpl>();
            container.RegisterPerWebRequest<MultipleTransferService, MultipleTransferServiceImpl>();
            container.RegisterSingleton<TransferToDownlineHistoryService, TransferToDownlineHistoryServiceImpl>();
            container.RegisterPerWebRequest<TransferService, TransferServiceImpl>();

            container.Register(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped);
            container.RegisterPerWebRequest<AutoTransferService, AutoTransferServiceImpl>();
        }

        private static RedisCache InitRedisCache()
        {
            var redisConfigurationOptions = Configurations.AppSettings.TransferRedisConfigurationOptions;
            var configurationOptions = ConfigurationOptions.Parse(redisConfigurationOptions);

            return new RedisCache(
                new RedisCacheOptions
                {
                    ApplicationName = configurationOptions.ClientName,
                    ConnectionOptions = configurationOptions
                });
        }
    }
}