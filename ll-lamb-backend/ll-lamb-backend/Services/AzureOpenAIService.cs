using System.Text;
using System.Text.Json;

namespace LlLambBackend.Services;

public class AzureOpenAIService : IAzureOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AzureOpenAIService> _logger;
    
    public AzureOpenAIService(HttpClient httpClient, ILogger<AzureOpenAIService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<string> GetChatCompletionAsync(string modelEndpoint, string apiKey, List<(string role, string content)> messages)
    {
        try
        {
            var requestBody = new
            {
                messages = messages.Select(m => new { role = m.role.ToLower(), content = m.content }).ToArray(),
                max_tokens = 1000,
                temperature = 0.7
            };
            
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);
            
            var response = await _httpClient.PostAsync($"{modelEndpoint}/openai/deployments/gpt-35-turbo/chat/completions?api-version=2023-12-01-preview", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Azure OpenAI API error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                throw new HttpRequestException($"Azure OpenAI API error: {response.StatusCode}");
            }
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonDocument.Parse(responseContent);
            
            var assistantMessage = responseJson.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
                
            return assistantMessage ?? "抱歉，我无法生成回复。";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Azure OpenAI API");
            throw;
        }
    }
}