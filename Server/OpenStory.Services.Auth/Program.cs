using System;
using System.Threading;
using OpenStory.Server;
using OpenStory.Server.Auth;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;
using Ninject;

namespace OpenStory.Services.Auth
{
    internal static class Program 
    {
        public static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            CreateKernel().Get<IBootstrapper>().Start();
            Thread.Sleep(Timeout.Infinite);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new ServerModule(), new AuthServerModule(), new WcfServiceModule());

            kernel.Rebind<NexusConnectionInfo>().ToConstant(GetNexusConnectionInfo());
            kernel.Bind<OsWcfConfiguration>().ToConstant(GetWcfConfiguration());

            return kernel;
        }

        private static NexusConnectionInfo GetNexusConnectionInfo()
        {
            var accessToken = new Guid("18B87A4B-E405-43F4-A1C2-A0AED35E3E15");
            return new NexusConnectionInfo(accessToken);
        }

        private static OsWcfConfiguration GetWcfConfiguration()
        {
            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Auth");
            var configuration = OsWcfConfiguration.For<AuthService>(baseUri);
            return configuration;
        }
    }
}
