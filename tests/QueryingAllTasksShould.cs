using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using demo_api;
using demo_api.responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;
using Newtonsoft.Json;
using paranoid.software.ephemerals.MongoDB;
using Xunit;

namespace tests
{
    public class QueryingAllTasksShould
    {
        public QueryingAllTasksShould()
        {
            
        }
        
        [Fact]
        public async Task ReturnOkResult()
        {
            using var ctx = new EphemeralMongoDbContextBuilder()
                .Build(new ConnectionParams{ HostName = "localhost", PortNumber = 27017, Username = "root", Password = "pwd", IsDirectConnection = true});
            var dbName = ctx.DbName;
            Environment.SetEnvironmentVariable("MONGODB_CONNECTION_STRING", "mongodb://root:pwd@localhost:27017");
            Environment.SetEnvironmentVariable("MONGODB_DATABASE", dbName);
            using var factory = new WebApplicationFactory<Startup>();
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/tasks");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task Return1Task()
        {
            using var ctx = new EphemeralMongoDbContextBuilder()
                .AddItems("tasks", new List<dynamic>
                {
                    new {
                        _id = ObjectId.GenerateNewId().ToString(),
                        Description = "HomeController coding",
                        Status = "InProgress"
                    }
                }).AddItemsFromFile("test-data/damaged-task")
                .Build(new ConnectionParams{ HostName = "localhost", PortNumber = 27017, Username = "root", Password = "pwd", IsDirectConnection = true});
            var dbName = ctx.DbName;
            Environment.SetEnvironmentVariable("MONGODB_CONNECTION_STRING", "mongodb://root:pwd@localhost:27017");
            Environment.SetEnvironmentVariable("MONGODB_DATABASE", dbName);
            using var factory = new WebApplicationFactory<Startup>();
            var sut = factory.CreateClient();
            var response = await sut.GetAsync("/tasks");
            var responseAsJsonString = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskItemResponse>>(responseAsJsonString);
            tasks.Count.Should().Be(1);
        }
    }
}