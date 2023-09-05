using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using demo_app.legacy.responses;
using Microsoft.AspNetCore.Mvc;

namespace demo_app.legacy.controllers
{
    
    [Route("legacy-tasks")]
    public class LegacyTasksController : Controller
    {
        [HttpGet]
        public IActionResult FetchAllTasks()
        {
            const string folderName = "legacy-db";
            const string fileName = "tasks.json";

            var projectDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(projectDirectory, folderName, fileName);
            var jsonContent = System.IO.File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItemResponse>>(jsonContent);
            
            return Ok(tasks);
        }
    }
}