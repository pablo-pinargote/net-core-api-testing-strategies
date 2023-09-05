using System.Text.Json.Serialization;

namespace demo_api.responses
{
    public class TaskItemResponse
    {
        [JsonPropertyName("taskId")]
        public string TaskId { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}