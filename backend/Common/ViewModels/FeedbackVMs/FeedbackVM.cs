namespace Common.ViewModels.FeedbackVMs;
public class FeedbackVM
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public string Name { get; set; }
    public string CreatedDate { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; }
    public string Response { get; set; }
    public int SenderId { get; set; }
    public int OwnerId { get; set; }
}
