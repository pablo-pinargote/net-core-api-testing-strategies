using System;
using demo_api.controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests
{
    public class TaskControllerTests
    {
        public TaskControllerTests()
        {
            Environment.SetEnvironmentVariable("MONGODB_CONNECTION_STRING", "mongodb://root:pwd@localhost:9001");
        }
        
        [Fact]
        public void FetchAllTasks_ReturnsOkResult()
        {
            var sut = new TasksController();
            var result = sut.FetchAllTasks();
            Assert.IsType<OkObjectResult>(result);
        }
    }
}