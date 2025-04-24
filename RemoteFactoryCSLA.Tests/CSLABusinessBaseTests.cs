using Backport.System.Threading;
using Csla;
using Csla.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace RemoteFactoryCSLA.Tests
{
    public class CSLABusinessBaseTests
    {


        [Fact]
        public void CSLABusinessBase_Create()
        {
            var mockDP = new Mock<IDataPortal<CSLABusinessBase>>(MockBehavior.Strict);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCsla(o =>
                        o.DataPortal(o =>
                            o.AddServerSideDataPortal(o => o.RemoveInterceptorProvider(0))));

            serviceCollection.AddSingleton(cc => mockDP.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            var cslaBBFactory = serviceProvider.GetService<IDataPortal<CSLABusinessBase>>();

            mockDP.Setup(x => x.Create(2)).Returns(Mock.Of<CSLABusinessBase>());

            var cslaBB = cslaBBFactory.Create();

            Assert.Equal(1, cslaBB.Id);
            Assert.NotNull(cslaBB.Description);
            mockDP.Verify(x => x.Create(2), Times.Exactly(2));
        }
    }
}
