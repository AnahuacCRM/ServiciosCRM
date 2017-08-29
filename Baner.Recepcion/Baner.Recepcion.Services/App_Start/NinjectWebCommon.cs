[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Baner.Recepcion.Services.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Baner.Recepcion.Services.App_Start.NinjectWebCommon), "Stop")]

namespace Baner.Recepcion.Services.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using OperationalManagement;
    using DataInterfaces;
    using BusinessInterfaces;
    using DataLayer;
    using BusinessLayer;
    using DataLayer.CRM;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //OperationalManagement //Debugger
            kernel.Bind<ILogger>().To<DebugerLogger>();
            

            //Repository
            //kernel.Bind<IServerConnection>().To<ServerConnection>().InSingletonScope();
            kernel.Bind<IProspectRepository>().To<ProspectRepository>();
            kernel.Bind<IPickListRepository>().To<PickListRepository>();
            kernel.Bind<ICatalogRepository>().To<CatalogRepository>();
            kernel.Bind<IOpportunityRepository>().To<OpportunityRepository>();
            kernel.Bind<IBannerRepository>().To<BannerRepository>();
            kernel.Bind<ILeadRepository>().To<LeadRepository>();

            //Processors
            kernel.Bind<IProspectProcessor>().To<ProspectProcessor>();
            kernel.Bind<IBannerProcessor>().To<BannerProcessor>();
            kernel.Bind<ILeadProcessor>().To<LeadProcessor>();
        }        
    }
}
