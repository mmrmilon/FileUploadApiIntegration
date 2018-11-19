[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FileUploadWebApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(FileUploadWebApp.App_Start.NinjectWebCommon), "Stop")]

namespace FileUploadWebApp.App_Start
{
    using System;
    using System.Web;
    using ApiIntegration.Infrastructure;
    using ApiIntegration.Infrastructure.Repositories;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

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
            kernel.Bind<IDropboxDbContext>().To<DropboxDbContext>();
            kernel.Bind<IDropboxUserToken>().To<DropboxUserToken>();

            kernel.Bind<IGoogleDriveApiConnection>().To<GoogleDriveApiConnection>();

            //kernel.Bind<ICopiedFileReferenceRepository>().To<CopiedFileReferenceRepository>();

            kernel.Bind<IDropboxApiConnection>()
                .To<DropboxApiConnection>()
                .InRequestScope()
                .WithConstructorArgument("clientId", "q0zjwrujrz2ljxu")
                .WithConstructorArgument("clientSecret", "ahj0ibkccup6612")
                .WithConstructorArgument("redirectUri", "http://localhost:49206/dropbox/oauth");
            
            kernel.Bind<IExactOnlineConnection>()
                .To<ExactOnlineConnection>()
                .InRequestScope()
                .WithConstructorArgument("clientId", "0ef65ad5-3b67-4901-acd0-ecceea85ef64")
                .WithConstructorArgument("clientSecret", "UPWYcng2nwf4")
                .WithConstructorArgument("redirectUri", "http://localhost:49206/exact/oauth")
                .WithConstructorArgument("endPoint", "https://start.exactonline.co.uk");
        }
    }
}
