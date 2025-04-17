namespace Common.ViewModels.ChatVMs;
public class PagedConversationsVM
{
    public IEnumerable<ConversationVM> Conversations { get; set; }
    public bool HasMore { get; set; } = false;
    public int PageNumber { get; set; }
}
