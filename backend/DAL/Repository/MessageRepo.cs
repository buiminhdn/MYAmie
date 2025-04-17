using Common.ViewModels.ChatVMs;
using DAL.Repository.Core;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Messages;
using Utility;

namespace DAL.Repository;
public class MessageRepo : BaseRepo<Message>, IMessageRepo
{
    public MessageRepo(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ConversationVM>> GetConversations(int userId)
    {
        var messages = await _dbSet
        .AsNoTracking()
        .Include(m => m.Sender)
        .Include(m => m.Receiver)
        .Where(m => m.SenderId == userId || m.ReceiverId == userId)
        .OrderByDescending(m => m.CreatedDate) // <-- Newest messages first
        .ToListAsync();

        return messages
            .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
            .Select(g =>
            {
                var latestMessage = g.First();
                return new ConversationVM
                {
                    Id = g.Key,
                    Name = latestMessage.SenderId == userId
                        ? latestMessage.Receiver.FirstName
                        : latestMessage.Sender.FirstName,
                    Avatar = latestMessage.SenderId == userId
                        ? latestMessage.Receiver.Avatar
                        : latestMessage.Sender.Avatar,
                    Content = latestMessage.Content,
                    SentAt = DateTimeUtils.EpochToTimeString(latestMessage.CreatedDate),
                    //IsRead = latestMessage.Status == MessageStatus.READ,
                    SenderId = latestMessage.SenderId
                };
            });
    }

    public async Task<IEnumerable<MessageVM>> GetMessages(int currentUserId, int otherUserId)
    {
        var messages = await _dbSet
            .AsNoTracking()
            .Where(m =>
                (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
            .OrderByDescending(m => m.CreatedDate)
            .Select(m => new MessageVM
            {
                Id = m.Id,
                Content = m.Content,
                SentAt = DateTimeUtils.EpochToTimeString(m.CreatedDate),  // Format the sent time
                Status = m.Status.ToString(),
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId
            })
            .ToListAsync();

        return messages;
    }
}
