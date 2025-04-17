using Common.ViewModels.ChatVMs;
using DAL.Repository.Core;
using Models.Messages;

namespace DAL.Repository.IRepository;
public interface IMessageRepo : IBaseRepo<Message>
{
    Task<IEnumerable<ConversationVM>> GetConversations(int userId);
    Task<IEnumerable<MessageVM>> GetMessages(int currentUserId, int otherUserId);
}
