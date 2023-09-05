using System.Text.Json.Serialization;

namespace demo_api.payloads
{
    public class TaskItemPayload
    {
        
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}