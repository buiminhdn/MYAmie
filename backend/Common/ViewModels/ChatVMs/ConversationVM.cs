namespace Common.ViewModels.ChatVMs;
public class ConversationVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Content { get; set; }
    public string SentAt { get; set; }
    public int SenderId { get; set; }
}
