using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using BlogSitesi2;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Core.Common;
using Service.Setup;

namespace BlogSitesi2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _container;
        public IWindsorContainer Container
        {
            get { return _container; }

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalConfiguration.Configuration.Filters.Add(new ElmahErrorAttribute());
            CreateIoCContainerAndRegisterAllDependencies();
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session == null)
            //    return;
            //var cultureInfo = (CultureInfo)this.Session[SystemConstraints.LanguageKey];
            //if (cultureInfo == null)
            //{
            //    const string langName = "tr-TR";
            //    cultureInfo = new CultureInfo(langName);
            //    Session[SystemConstraints.LanguageKey] = cultureInfo;
            //}
            //Thread.CurrentThread.CurrentUICulture = cultureInfo;
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
        }


        public override string GetVaryByCustomString(HttpContext context, string custom)
        {

            if (custom.ToLowerInvariant() == "ismobile")
            {
                return context.GetVaryByCustomStringForOverriddenBrowser();
            }
            return base.GetVaryByCustomString(context, custom);
        }

        private void CreateIoCContainerAndRegisterAllDependencies()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            var mvcControllerFactory = new WindsorControllerFactory(Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(mvcControllerFactory);
            var setup = new IoCInitializer(Container);
            setup.RegisterAll();
        }
    }
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }
            return (IController)_kernel.Resolve(controllerType);
        }
    }

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
        }
    }
}