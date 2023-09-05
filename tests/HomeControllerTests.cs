using System.Collections.Generic;
using demo_api.controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsOkResult()
        {
            var endpointDataSourceMock = new Mock<EndpointDataSource>();
            endpointDataSourceMock.Setup(ds => ds.Endpoints).Returns(new List<Endpoint> ());
            var sut = new HomeController(endpointDataSourceMock.Object);
            var result = sut.Index();
            Assert.IsType<OkObjectResult>(result);
        }
    }
}