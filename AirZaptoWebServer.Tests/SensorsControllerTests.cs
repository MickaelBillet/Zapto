using AirZapto.WebServer;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AirZaptoWebServer.Tests
{
    public class SensorsControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public SensorsControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Test1()
        {          
            // Arrange
            Assert.True(true);
        }
    }
}