using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using demo_api;
using demo_api.repositories.legacy;
using demo_api.responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace tests
{
    public class QueryingAllLegacyTasksShould
    {
        private class MyFileReader : IFileReader
        {
            public string Read(string filepath)
            {
                return "[]";
            }
        }
        
        [Fact]
        public async Task ReturnOkResult()
        {
            using var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IFileReader, MyFileReader>();
                });
            });
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/legacy-tasks");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task ReturnEmptyTasksList()
        {
            var fileReaderMock = new Mock<IFileReader>();
            fileReaderMock.Setup(reader => reader.Read(It.IsAny<string>())).Returns("[]");

            using var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient(_ => fileReaderMock.Object);
                });
            });
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/legacy-tasks");
            var responseAsJsonString = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskItemResponse>>(responseAsJsonString);
            tasks.Should().BeEmpty();
        }
        
    }
}