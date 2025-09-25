namespace LocalChatBotBackend.Interfaces
{
    public interface IProjectAnalyzerService
    {
        Task<object> AnalyzeProjectAsync(string path, string type);
    }
}
