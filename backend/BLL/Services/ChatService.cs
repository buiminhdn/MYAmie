using BLL.Interfaces;
using Common.DTOs.ChatDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.ChatVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Utility.Constants;

namespace BLL.Services;
public class ChatService(IUnitOfWork unitOfWork, ILogger<ChatService> logger) : IChatService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ChatService> _logger = logger;

    public async Task<ApiResponse<PagedConversationsVM>> GetConversations(PagedConversationParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId);

            if (account == null)
                return ApiResponse<PagedConversationsVM>.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse<PagedConversationsVM>.Failure(ResponseMessages.AccountNotVerified);

            var conversations = await _unitOfWork.Messages.GetConversations(param.CurrentUserId);

            var pagedConversations = PagedList<ConversationVM>.ToPagedList(
                conversations,
                param.PageNumber,
                10
            );

            var result = new PagedConversationsVM
            {
                Conversations = pagedConversations,
                HasMore = pagedConversations.PaginationData.HasNext,
                PageNumber = param.PageNumber
            };

            return ApiResponse<PagedConversationsVM>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching conversations for user {UserId}", param.CurrentUserId);
            return ApiResponse<PagedConversationsVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<PagedMessagesVM>> GetMessages(PagedMessageParams param)
    {
        try
        {
            var currentUser = await _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId);
            var otherUser = await _unitOfWork.Accounts.GetByIdAsync(param.OtherUserId);

            if (currentUser == null || otherUser == null)
                return ApiResponse<PagedMessagesVM>.Failure(ResponseMessages.AccountNotFound);

            if (!currentUser.IsEmailVerified)
                return ApiResponse<PagedMessagesVM>.Failure(ResponseMessages.AccountNotVerified);

            var messages = await _unitOfWork.Messages.GetMessages(param.CurrentUserId, param.OtherUserId);

            var pagedMessages = PagedList<MessageVM>.ToPagedList(
                messages,
                param.PageNumber,
                20
            );

            pagedMessages.Reverse();

            var result = new PagedMessagesVM
            {
                Messages = pagedMessages,
                HasMore = pagedMessages.PaginationData.HasNext,
                PageNumber = param.PageNumber
            };

            return ApiResponse<PagedMessagesVM>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching messages for users {CurrentUserId} and {OtherUserId}", param.CurrentUserId, param.OtherUserId);
            return ApiResponse<PagedMessagesVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
