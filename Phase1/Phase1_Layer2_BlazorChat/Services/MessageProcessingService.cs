namespace Phase1_Layer2_BlazorChat.Services;

public class MessageProcessingService
{
    //this will later hold message processing logic, such as formatting the message, extracting relevant information from the message, etc.
    //for now, we will just return the message as it is, but in the future, we can add more logic to process the message before sending it to the chat service
    public string CleanAiResponse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "No response from ai .";
        var cleaned = RemoveThinkBlocks(text);

        return cleaned.Trim();
           
    }

    private string RemoveThinkBlocks(string text)
    {
        var cleaned = text;
       
        while (cleaned.Contains("<think>") && cleaned.Contains("</think>"))
        {
            int startIndex = cleaned.IndexOf ("<think>");
            int endIndex = cleaned.IndexOf("</think>") + "</think>".Length;                         // Find the end index of the closing tag to ensure we remove the entire block
            if (startIndex >=0 && endIndex > startIndex)                                            // Check if the tags are properly nested
            {
                cleaned = cleaned.Remove(startIndex, endIndex - startIndex);
            }
            else
            {
                break;                                                                              // If the tags are not properly nested, exit the loop to avoid infinite loop
            }
        }
        return cleaned;
    }
}