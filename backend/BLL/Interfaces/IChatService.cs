using Common.DTOs.ChatDtos;
using Common.Responses;
using Common.ViewModels.ChatVMs;

namespace BLL.Interfaces;
public interface IChatService
{
    Task<ApiResponse<PagedConversationsVM>> GetConversations(PagedConversationParams param);
    Task<ApiResponse<PagedMessagesVM>> GetMessages(PagedMessageParams param);
}
