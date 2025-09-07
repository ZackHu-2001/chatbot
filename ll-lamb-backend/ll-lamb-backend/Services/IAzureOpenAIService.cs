namespace LlLambBackend.Services;

public interface IAzureOpenAIService
{
    Task<string> GetChatCompletionAsync(string modelEndpoint, string apiKey, List<(string role, string content)> messages);
}