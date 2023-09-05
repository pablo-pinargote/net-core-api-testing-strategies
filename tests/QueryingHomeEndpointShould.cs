using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using demo_api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace tests
{
    public class QueryingHomeEndpointShould
    {
        
        [Fact]
        public async Task ReturnOkResult()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task GreetsTheUser()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/");
            var responseAsJsonString = await response.Content.ReadAsStringAsync();
            var responseAsDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseAsJsonString);
            responseAsDict.Keys.Should().Contain("hi");
            responseAsDict["hi"].Should().Be("stranger");
        }
        
        [Fact]
        public async Task FetchJsonObjectWithAvailableEndpointsList()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/");
            var responseAsJsonString = await response.Content.ReadAsStringAsync();
            var responseAsDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseAsJsonString);
            responseAsDict.Keys.Should().Contain("theseAreMyEndpoints");
            var endpoints = ((JArray)responseAsDict["theseAreMyEndpoints"]).Select(jToken => jToken.Value<string>()).ToList();
            var expectedElements = new List<string>
            {
                "(GET) exchange-rates",
                "(GET) legacy-tasks",
                "(GET) tasks",
                "(GET) tasks/{id}",
                "(GET) tasks/overview",
                "(POST) tasks",
                "(PUT) tasks/{id}",
                "(DELETE) tasks/{id}"
            };
            endpoints.Count.Should().Be(expectedElements.Count);
            endpoints.Should().Contain(expectedElements);
        }
        
    }
    
}