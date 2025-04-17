namespace Common.ViewModels.ChatVMs;
public class PagedMessagesVM
{
    public IEnumerable<MessageVM> Messages { get; set; }
    public bool HasMore { get; set; } = false;
    public int PageNumber { get; set; }
}
