using System;
using System.Configuration;
using System.Web.Configuration;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Core.Interceptor;
using Core.UnitOfWork;
using Domain.Model;
using Domain.Model.BlogPosts;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Repository.BlogPostRepository;
using Service.BlogPostService;

namespace Service.Setup
{
    public class IoCInitializer
    {
        private readonly IWindsorContainer _container;

        public IoCInitializer(IWindsorContainer windsorContainer)
        {
            _container = windsorContainer;
            _container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        //TODO
        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (UnitOfWorkHelper.IsServiceClass(handler.ComponentModel.Implementation))
            {

                //handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NhUnitOfWorkInterceptor)));
                //handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NotificationOfUnitOfWorkInterceptor)));
            }

            //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
            foreach (var method in handler.ComponentModel.Implementation.GetMethods())
            {
                if (UnitOfWorkHelper.HasUnitOfWorkAttribute(method))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
                    //   handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NotificationOfUnitOfWorkInterceptor)));
                    return;
                }
            }
        }

        public static ISessionFactory CreateSessionFactory()
        {
            return CreateSessionFactory(false);
        }



        public static ISessionFactory CreateSessionFactory(bool isDbReCreated = false)
        {
            if (!isDbReCreated)
                return Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2012
                                .Raw("connection.isolation", "ReadCommitted")
                                .ConnectionString(ConfigurationManager.ConnectionStrings[Environment.MachineName].ConnectionString))
                                .Mappings(g => g.FluentMappings.AddFromAssemblyOf<BlogPost>())
                                .ExposeConfiguration(c => c.SetInterceptor(new EntityTimeStampInterceptor()))
                                .Cache(g => g.ProviderClass<NHibernate.Caches.SysCache.SysCacheProvider>()
                                .UseSecondLevelCache()
                                .UseQueryCache())
                                .BuildSessionFactory();



            return Fluently.Configure()
                           .Database(MsSqlConfiguration.MsSql2012
                           .Raw("connection.isolation", "ReadCommitted")
                           .ConnectionString(ConfigurationManager.ConnectionStrings[Environment.MachineName].ConnectionString))
                           .Mappings(g => g.FluentMappings.AddFromAssemblyOf<BlogPost>())
                           .ExposeConfiguration(c => c.SetInterceptor(new EntityTimeStampInterceptor()))
                           .Cache(g => g.ProviderClass<NHibernate.Caches.SysCache.SysCacheProvider>()
                           .UseSecondLevelCache()
                           .UseQueryCache())
                           .ExposeConfiguration(BuildSchema)
                           .BuildSessionFactory();

        }
        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
            new SchemaExport(config).Drop(false, true);
            new SchemaExport(config).Create(false, true);
        }

        public void RegisterNhibernateSessionFactoryAndUnitOfWork()
        {
            _container.Register(Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory).LifeStyle.Singleton);
            _container.Register(Component.For<UnitOfWorkInterceptor>().LifeStyle.Transient);
        }

        public void RegisterRepositories()
        {
            _container.Register(Classes.FromAssembly(typeof(IBlogPostRepository).Assembly)
                                       .Where(type => type.Name.EndsWith("Repository"))
                                       .WithServiceDefaultInterfaces()
                                       .LifestyleTransient());
        }

        public void RegisterServices()
        {
            _container.Register(Classes.FromAssembly(typeof(IBlogPostService).Assembly)
                                       .Where(type => type.Name.EndsWith("Service"))
                                       .WithServiceDefaultInterfaces()
                                       .Configure(g => g.Interceptors(InterceptorReference.ForType<UnitOfWorkInterceptor>())
                                       .First.LifestyleTransient())
                                       .LifestyleTransient());
        }

        public void RegisterAll()
        {
            RegisterNhibernateSessionFactoryAndUnitOfWork();
            RegisterRepositories();
            RegisterServices();
        }
    }
}