namespace Alpha.Transfer.Site.App_Start
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Alpha.Web.Helpers;
    using AlphaPlus.User.Models;
    using Controllers;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Transfer.Models;

    public static class DependencyConfig
    {
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
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<ICache>(InitRedisCache());
            container.RegisterSingleton<ICacheService, CacheService>();

            UserSession.Init(new HttpContextHelper());
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();

            container.RegisterSingleton<IRemotingRefService, RemotingRefService>();
            container.RegisterSingleton<CacheRefService, CacheRefServiceImpl>();
            container.RegisterSingleton<CustomerService, CustomerServiceImpl>();
            container.RegisterSingleton<BelongingService, BelongingServiceImpl>();

            container.RegisterSingleton<TransferAllAndClearCreditService, TransferAllAndClearCreditServiceImpl>();
            container.RegisterPerWebRequest<CashBalanceTransferService, CashBalanceTransferServiceImpl>();
            container.RegisterPerWebRequest<MultipleTransferService, MultipleTransferServiceImpl>();
            container.RegisterSingleton<TransferToDownlineHistoryService, TransferToDownlineHistoryServiceImpl>();
            container.RegisterPerWebRequest<TransferService, TransferServiceImpl>();

            container.Register(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped);
            container.RegisterPerWebRequest<AutoTransferService, AutoTransferServiceImpl>();
            container.RegisterPerWebRequest<RewardService, RewardServiceImpl>();
        }

        private static RedisCache InitRedisCache()
        {
            var redisConfigurationOptions = Configurations.AppSettings.TransferRedisConfigurationOptions;

            return new RedisCache(
                new RedisCacheOptions
                {
                    ConnectionString = redisConfigurationOptions
                });
        }
    }
}