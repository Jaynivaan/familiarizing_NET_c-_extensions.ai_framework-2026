using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Phase1_Layer2_BlazorChat.Services;

public class ChatService
{
    //this willlater hold chat logic, ollama calls logic, 
    //database save/load operations logic
    private readonly HttpClient _http;
    private readonly MessageProcessingService _messageProcessing;

    
    public ChatService(HttpClient http, MessageProcessingService messageProcessing)
    {
        _http = http;
        _messageProcessing = messageProcessing;
    }

    public async Task<string> SendMessage (string message)
    {
        var request = new
        {
            model = "deepseek-r1:latest",
            prompt = message,
            stream = false

        };
        HttpResponseMessage response = await _http.PostAsJsonAsync(
            "http://localhost:11434/api/generate",                              //this is the endpoint for ollama api, we will call this endpoint to get response from ollama
            request
            );

        if((int)response.StatusCode <200 || (int)response.StatusCode >=300) //this is another way to check if the response is successful or not, we check if the status code is between 200 and 299, if it is not, then we consider it as a failed response
        {
           
            string rawError = await response.Content.ReadAsStringAsync();         //if the response is not successful, we read the error message from the response content and return it
           
            return $"Ollama call failed: {(int)response.StatusCode} | {rawError}";     //this will help us to debug the issue with ollama call, we can see the status code and the error message from the response content

        }
        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
        return  _messageProcessing.CleanAiResponse(result?.response ?? "No response from ai .");
    }

    private class  OllamaResponse
    {
        public string response { get; set; } = "";
    }
}