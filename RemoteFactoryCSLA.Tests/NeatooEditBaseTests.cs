using Moq;
using Neatoo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteFactoryCSLA.Tests
{
    public class NeatooEditBaseTests
    {
        [Fact]
        public void NeatooEditBaseTests_Create()
        {
            var neatooEditBase = new NeatooEditBase(new EditBaseServices<NeatooEditBase>(null!));
            var mockFactory = new Mock<INeatooEditBaseFactory>(MockBehavior.Strict);

            mockFactory.Setup(x => x.Create(2)).Returns(Mock.Of<INeatooEditBase>());
            neatooEditBase.Create(mockFactory.Object);

            Assert.Equal(1, neatooEditBase.Id);
            Assert.NotNull(neatooEditBase.Description);
            mockFactory.Verify(x => x.Create(2), Times.Exactly(2));
        }
    }
}
