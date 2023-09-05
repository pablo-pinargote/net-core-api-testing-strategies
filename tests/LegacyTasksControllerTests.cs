using demo_api.controllers;
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
    }
}