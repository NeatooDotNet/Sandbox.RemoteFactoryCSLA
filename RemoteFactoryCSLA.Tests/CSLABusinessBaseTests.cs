using Csla;
using Csla.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace RemoteFactoryCSLA.Tests
{
    public class CSLABusinessBaseTests
    {
        private static IServiceProvider serviceProvider;
        public CSLABusinessBaseTests()
        {
            if (serviceProvider == null)
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddCsla(o =>
                            o.DataPortal(o => 
                                o.AddServerSideDataPortal(o => o.RemoveInterceptorProvider(0))));
                // If I include this then it doesn't call the Create method even the first time
                //serviceCollection.AddTransient<IDataPortal<CSLABusinessBase>>(cc => Mock.Of<IDataPortal<CSLABusinessBase>>());
                serviceProvider = serviceCollection.BuildServiceProvider();
            }
        }

        [Fact]
        public void CSLABusinessBase_Create()
        {
            var factory = serviceProvider.GetRequiredService<IDataPortalFactory>();
            var cslaBBFactory = factory.GetPortal<CSLABusinessBase>();

            // Right now this returns all 32k objects which is not ok
            // How do I moq IDataPortal<CSLABusinessBase>???
            var cslaBB = cslaBBFactory.Create();
        }
    }
}
