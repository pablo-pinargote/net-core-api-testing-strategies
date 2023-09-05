using System;
using System.Collections.Generic;
using System.Linq;
using demo_api.models;
using demo_api.payloads;
using demo_api.responses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace demo_api.controllers
{
    
    [Route("tasks")]
    public class TasksController: Controller
    {
        private readonly List<string> _supportedStatusList = new List<string>
        {
            "New",
            "ToDo",
            "InProgress",
            "Pending",
            "Done"
        };
        
        private const string DatabaseName = "todo";
        private const string CollectionName = "tasks";

        private readonly MongoClient _client;

        public TasksController()
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
            _client = new MongoClient(connectionString);
        }

        [HttpGet]
        public IActionResult FetchAllTasks()
        {
            var tasks = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).Find(new BsonDocument())
                .ToList();
            var tasksResponse = tasks.Select(x => new TaskItemResponse{ TaskId = x._id, Description = x.Description, Status = x.Status}).ToList();
            return Ok(tasksResponse);
        }
        
        [HttpGet("{id}")]
        public IActionResult FetchTask(string id)
        {
            var task = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).Find(x=>x._id==id)
                .FirstOrDefault();
            if (task == null)
            {
                return NotFound();
            }
            var tasksResponse = new TaskItemResponse{ TaskId = task._id, Description = task.Description, Status = task.Status};
            return Ok(tasksResponse);
        }

        [HttpPost]
        public IActionResult AddNewTask([FromBody] TaskItemPayload payload)
        {
            if (!_supportedStatusList.Contains(payload.Status))
            {
                return BadRequest("Unsupported status value.");
            }
            var newTaskItem = new TaskItem { _id = ObjectId.GenerateNewId().ToString(), Description = payload.Description, Status = payload.Status };
            _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).InsertOne(newTaskItem);
            return Created($"tasks?id={newTaskItem._id}", new {newTaskItem._id});
        }
        
        [HttpPut("{id}")]
        public IActionResult ReplaceTask(string id, [FromBody] TaskItemPayload payload)
        {
            if (!_supportedStatusList.Contains(payload.Status))
            {
                return BadRequest("Unsupported status value.");
            }
            var taskToUpdate = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).Find(x => x._id == id).FirstOrDefault();
            if (taskToUpdate == null) return NotFound();
            {
                taskToUpdate.Description = payload.Description;
                taskToUpdate.Status = payload.Status;
                var result = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).ReplaceOne(x => x._id == id, taskToUpdate);
                return result.MatchedCount >= 1 ? Ok() : StatusCode(304);
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(string id)
        {
            var taskToDelete = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).Find(x => x._id == id).FirstOrDefault();
            if (taskToDelete == null) return NotFound();
            {
                var result = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).DeleteOne(x => x._id == id);
                return result.DeletedCount >= 1 ? Ok() : StatusCode(304);
            }
        }

        [HttpGet("overview")]
        public IActionResult GetTasksOverview()
        {
            var tasks = _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).Find(new BsonDocument())
                .ToList();
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