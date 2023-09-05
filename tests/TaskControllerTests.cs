using System.Collections.Generic;
using demo_api.controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests
{
    public class TaskControllerTests
    {
        [Fact]
        public void FetchAllTasks_ReturnsOkResult()
        {
            var sut = new TasksController();
            var result = sut.FetchAllTasks();
            Assert.IsType<OkObjectResult>(result);
        }
        
    }
}