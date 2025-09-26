namespace LocalChatBotBackend.Models
{
    /// <summary>
    /// Model for chat request from Angular frontend (ChatService.ts)
    /// </summary>
    public class ChatRequest
    {
        public string UserQuestion { get; set; }
        public object ProjectJson { get; set; }
        public bool UseProjectContext { get; set; }
    }
}
