using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using demo_api.models;
using demo_api.responses;
using Microsoft.AspNetCore.Mvc;

namespace demo_api.controllers
{
    
    [Route("legacy-tasks")]
    public class LegacyTasksController : Controller
    {
        
        private readonly List<string> _supportedStatusList = new List<string>
        {
            "New",
            "ToDo",
            "InProgress",
            "Pending",
            "Done"
        };
        
        [HttpGet]
        public IActionResult FetchAllTasks()
        {
            const string folderName = "legacy-db";
            const string fileName = "tasks.json";

            var projectDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(projectDirectory, folderName, fileName);
            var jsonContent = System.IO.File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonContent);
            var tasksResponse = tasks.Select(x => new TaskItemResponse{ TaskId = x._id, Description = x.Description, Status = x.Status}).ToList();
            return Ok(tasksResponse);
        }
        
        [HttpGet("{id}")]
        public IActionResult FetchTask(string id)
        {
            const string folderName = "legacy-db";
            const string fileName = "tasks.json";

            var projectDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(projectDirectory, folderName, fileName);
            var jsonContent = System.IO.File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonContent);
            var task = tasks.FirstOrDefault(x => x._id == id);
            if (task == null)
            {
                return NotFound();
            }
            var tasksResponse = new TaskItemResponse{ TaskId = task._id, Description = task.Description, Status = task.Status};
            return Ok(tasksResponse);
        }
        
        [HttpGet("overview")]
        public IActionResult GetTasksOverview()
        {
            const string folderName = "legacy-db";
            const string fileName = "tasks.json";

            var projectDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(projectDirectory, folderName, fileName);
            var jsonContent = System.IO.File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonContent);
            var overview = _supportedStatusList
                .GroupJoin(tasks,
                    supportedStatus => supportedStatus,
                    taskItem => taskItem.Status,
                    (supportedStatus, taskItemsForStatus) => new
                    {
                        Status = supportedStatus,
                        Count = taskItemsForStatus.Count()
                        
                    })
                .ToList();
            return Ok(overview);
        }
        
    }
}