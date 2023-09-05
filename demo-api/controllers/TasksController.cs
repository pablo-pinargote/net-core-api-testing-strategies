using System.Linq;
using demo_app.legacy.models;
using demo_app.legacy.payloads;
using demo_app.legacy.responses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace demo_app.legacy.controllers
{
    
    [Route("tasks")]
    public class TasksController: Controller
    {
        private const string ConnectionString = "mongodb://root:pwd@localhost:9001";
        private const string DatabaseName = "todo";
        private const string CollectionName = "tasks";

        private readonly MongoClient _client;

        public TasksController()
        {
            _client = new MongoClient(ConnectionString);
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
            var newTaskItem = new TaskItem { _id = ObjectId.GenerateNewId().ToString(), Description = payload.Description, Status = payload.Status };
            _client.GetDatabase(DatabaseName).GetCollection<TaskItem>(CollectionName).InsertOne(newTaskItem);
            return Created($"tasks?id={newTaskItem._id}", new {newTaskItem._id});
        }
        
        [HttpPut("{id}")]
        public IActionResult ReplaceTask(string id, [FromBody] TaskItemPayload payload)
        {
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
        
    }
}