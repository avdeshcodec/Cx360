[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IncidentManagement.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(IncidentManagement.API.App_Start.NinjectWebCommon), "Stop")]

namespace IncidentManagement.API.App_Start
{

    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using IncidentManagement.Repository.IRepository;
    using IncidentManagement.Repository.Repository;
    using IncidentManagement.Service.IService;
    using IncidentManagement.Service.Service;
    
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

                // Install our Ninject-based IDependencyResolver into the Web API config
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                RegisterServices(kernel);
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
            //All RepositoryLayer dependencies 
            kernel.Bind<IIncidentManagementRepository>().To<IncidentManagementRepository>();
            kernel.Bind<ILIfePlanRepository>().To<LIfePlanRepository>();
            kernel.Bind<IComprehensiveAssessmentRepository>().To<ComprehensiveAssessmentRepository>();
            kernel.Bind<ICANSRepository>().To<CANSRepository>();
            //All Service Layer dependencies 
            kernel.Bind<IIncidentManagementService>().To<IncidentManagementService>();
            kernel.Bind<ILifePlanService>().To<LifePlanService>();
            kernel.Bind<IComprehensiveAssessmentService>().To<ComprehensiveAssessmentService>();
            kernel.Bind<ICANSService>().To<CANSService>();
        }
    }
}
