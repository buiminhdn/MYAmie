using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.FeedbackDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.FeedbackVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Feedbacks;
using MYAmie.Common.DTOs.FeedbackDtos;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FeedbackService> logger) : IFeedbackService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<FeedbackService> _logger = logger;

    public async Task<ApiResponse<PagedFeedbacksVM>> GetFeedbacks(FilterFeedbackParams param)
    {
        try
        {
            // Build the filter expression dynamically
            Expression<Func<Feedback, bool>> filter = feedback =>
                feedback.TargetId == param.Id &&
                (!param.Rate.HasValue || param.Rate > 0 && feedback.Rating == param.Rate.Value) &&
                (!param.IsResponded.HasValue ||
                    (param.IsResponded.Value && feedback.Response != null) ||
                    (!param.IsResponded.Value && feedback.Response == null));

            var feedbacks = await _unitOfWork.Feedbacks.GetAllAsync(filter, "Sender");

            var feedbackVMs = _mapper.Map<IEnumerable<FeedbackVM>>(feedbacks);
            var totalFeedback = feedbacks.Count();
            var averageRating = feedbacks.Any()
                ? Math.Round(feedbacks.Average(f => f.Rating), 1)
                : 0;

            var pagedFeedbacks = PagedList<FeedbackVM>.ToPagedList(feedbackVMs, param.PageNumber, param.PageSize);

            var pagedFeedbacksVM = new PagedFeedbacksVM
            {
                TotalFeedback = totalFeedback,
                AverageRating = averageRating,
                Feedbacks = pagedFeedbacks,
                Pagination = pagedFeedbacks.PaginationData
            };

            return ApiResponse<PagedFeedbacksVM>.Success(pagedFeedbacksVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching feedbacks. TargetId: {TargetId}", param.Id);
            return ApiResponse<PagedFeedbacksVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> AddFeedback(AddFeedbackParams param)
    {
        try
        {
            var existingFeedback = await _unitOfWork.Feedbacks.GetAsync(f =>
                f.SenderId == param.CurrentUserId &&
                f.TargetId == param.TargetId &&
                f.TargetType == param.TargetType);

            if (existingFeedback != null)
                return ApiResponse.Failure(ResponseMessages.FeedbackAlreadyExists);

            if (param.TargetType == FeedbackTargetType.PLACE)
            {
                var place = await _unitOfWork.Places.GetByIdAsync(param.TargetId);
                if (place == null)
                    return ApiResponse.Failure(ResponseMessages.PlaceNotFound);
            }
            else
            {
                var account = await _unitOfWork.Accounts.GetByIdAsync(param.TargetId);
                if (account == null)
                    return ApiResponse.Failure(ResponseMessages.AccountNotFound);
            }

            var feedback = new Feedback
            {
                SenderId = param.CurrentUserId,
                TargetId = param.TargetId,
                TargetType = param.TargetType,
                Rating = param.Rating,
                Content = param.Content,
                CreatedBy = param.CurrentUserId,
            };

            await _unitOfWork.Feedbacks.AddAsync(feedback);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.FeedbackAdded)
                : ApiResponse.Failure(ResponseMessages.FeedbackNotAdded);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding feedback. SenderId: {SenderId}, TargetId: {TargetId}", param.CurrentUserId, param.TargetId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> DeleteFeedback(DeleteFeedbackParams param)
    {
        try
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(param.Id);

            if (feedback == null)
                return ApiResponse.Failure(ResponseMessages.FeedbackNotFound);

            if (feedback.SenderId != param.CurrentUserId)
                return ApiResponse.Failure(ResponseMessages.NotAllowed);

            await _unitOfWork.Feedbacks.DeleteAsync(feedback);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.FeedbackDeleted)
                : ApiResponse.Failure(ResponseMessages.FeedbackNotDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting feedback. FeedbackId: {FeedbackId}", param.Id);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> ResponseFeedback(ResponseFeedbackParams param)
    {
        try
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(param.Id);

            if (feedback == null)
                return ApiResponse.Failure(ResponseMessages.FeedbackNotFound);

            if (feedback.TargetId != param.CurrentUserId)
                return ApiResponse.Failure(ResponseMessages.NotAllowed);

            if (feedback.Response != null)
                return ApiResponse.Failure(ResponseMessages.FeedbackResponded);

            feedback.Response = param.Message;
            feedback.UpdatedBy = param.CurrentUserId;
            feedback.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Feedbacks.UpdateAsync(feedback);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.FeedbackResponded)
                : ApiResponse.Failure(ResponseMessages.FeedbackNotResponded);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while responding to feedback. FeedbackId: {FeedbackId}", param.Id);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdateFeedback(UpdateFeedbackParams param)
    {
        try
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(param.Id);

            if (feedback == null)
                return ApiResponse.Failure(ResponseMessages.FeedbackNotFound);

            if (feedback.SenderId != param.CurrentUserId)
                return ApiResponse.Failure(ResponseMessages.NotAllowed);

            if (DateTimeUtils.TimeInEpoch() - feedback.CreatedDate > 15 * 24 * 60 * 60)
                return ApiResponse.Failure(ResponseMessages.FeedbackTimeOut);

            if (feedback.Response != null)
                return ApiResponse.Failure(ResponseMessages.FeedbackResponded);

            feedback.Rating = param.Rating;
            feedback.Content = param.Content;
            feedback.UpdatedBy = param.CurrentUserId;
            feedback.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Feedbacks.UpdateAsync(feedback);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.FeedbackUpdated)
                : ApiResponse.Failure(ResponseMessages.FeedbackNotUpdated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating feedback. FeedbackId: {FeedbackId}", param.Id);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
