using System.Collections.Generic;
using demo_api.controllers;
using demo_api.responses;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests
{
    public class LegacyTasksControllerTests
    {
        [Fact]
        public void FetchAllTasks_ReturnsOkResult()
        {
            var sut = new LegacyTasksController();
            var result = sut.FetchAllTasks();
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void FetchAllTasks_Returns2Tasks()
        {
            var sut = new LegacyTasksController();
            var result = sut.FetchAllTasks();
            var okResult = (OkObjectResult)result;
            Assert.Equal(2, ((List<TaskItemResponse>)okResult.Value).Count);
        }
        
    }
    
}