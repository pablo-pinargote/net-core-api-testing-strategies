using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using demo_api.repositories.legacy;
using demo_api.responses;
using Microsoft.AspNetCore.Mvc;

namespace demo_api.controllers
{
    
    [Route("legacy-tasks")]
    public class LegacyTasksController : Controller
    {
        [HttpGet]
        public IActionResult FetchAllTasks([FromServices] IFileReader fileReader)
        {
            const string folderName = "legacy-db";
            const string fileName = "tasks.json";

            var projectDirectory = Directory.GetCurrentDirectory();
            var filepath = Path.Combine(projectDirectory, folderName, fileName);
            var jsonContent = fileReader.Read(filepath);
            var tasks = JsonSerializer.Deserialize<List<TaskItemResponse>>(jsonContent);
            
            return Ok(tasks);
        }
    }
}