namespace Phase1_Layer2_BlazorChat.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string Role { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}