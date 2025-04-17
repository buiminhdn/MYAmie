namespace Common.ViewModels.ChatVMs;
public class MessageVM
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string SentAt { get; set; }
    public string Status { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
}
