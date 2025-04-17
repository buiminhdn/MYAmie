using AutoMapper;
using Common.ViewModels.ChatVMs;
using DAL.Repository.Core;
using Microsoft.AspNetCore.SignalR;
using Models.Messages;
using System.Security.Claims;
using Utility;
using Utility.Constants;
namespace WebAPI.Hubs;

public class ChatHub(IUnitOfWork unitOfWork, ILogger<ChatHub> logger, IMapper mapper) : Hub
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ChatHub> _logger = logger;
    private readonly IMapper _mapper = mapper;
    public async Task<MessageVM> SendMessage(int receiverId, string messageContent)
    {
        try
        {
            var senderId = GetCurrentUserId();

            if (senderId == 0)
            {
                throw new HubException(ResponseMessages.LoginRequired);
            }

            if (receiverId <= 1 || string.IsNullOrWhiteSpace(messageContent))
            {
                throw new ArgumentException("Thông tin đầu vào không hợp lệ.");
            }

            var isFriend = await _unitOfWork.Friendships.IsFriend(senderId, receiverId);

            if (!isFriend)
            {
                throw new HubException(ResponseMessages.FriendshipNotFound);
            }

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = messageContent,
                CreatedBy = senderId,
                CreatedDate = DateTimeUtils.TimeInEpoch()
            };

            await _unitOfWork.Messages.AddAsync(message);
            await _unitOfWork.SaveAsync();


            var messageVM = _mapper.Map<MessageVM>(message);

            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", messageVM);

            return messageVM;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in {MethodName}. ReceiverId: {ReceiverId}, Content: {Content}",
                nameof(SendMessage), receiverId, messageContent);
            return null;
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }
}
