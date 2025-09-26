using System.Text.Json.Serialization;

namespace LocalChatBotBackend.Models
{
    /// <summary>
    /// Model for project analysis requests from the Angular frontend (ChatService.ts)
    /// </summary>
    public class ProjectRequest
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
