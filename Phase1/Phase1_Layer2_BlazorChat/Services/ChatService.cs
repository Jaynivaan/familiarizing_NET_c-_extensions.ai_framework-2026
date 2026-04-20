using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Phase1_Layer2_BlazorChat.Services;

public class ChatService
{
    //this willlater hold chat logic, ollama calls logic, 
    //database save/load operations logic
    private readonly HttpClient _Http;
    
    public ChatService(HttpClient http)
    {
        _Http = http;
    }

    public async Task<string> SendMessage (string message)
    {
        var request = new
        {
            model = "deepseek-r1:32b",
            prompt = message,
            stream = false

        };
        var response = await _Http.PostAsJsonAsync("http://localhost:11434/api/generate", request);
        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
        return result?.response ?? "No response from ai .";
    }

    private class  OllamaResponse
    {
        public string response { get; set; } = "";
    }
}