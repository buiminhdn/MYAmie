using Common.DTOs.FeedbackDtos;
using Common.Responses;
using Common.ViewModels.FeedbackVMs;
using MYAmie.Common.DTOs.FeedbackDtos;

namespace BLL.Interfaces;
public interface IFeedbackService
{
    Task<ApiResponse<PagedFeedbacksVM>> GetFeedbacks(FilterFeedbackParams param);
    Task<ApiResponse> AddFeedback(AddFeedbackParams param);
    Task<ApiResponse> UpdateFeedback(UpdateFeedbackParams param);
    Task<ApiResponse> DeleteFeedback(DeleteFeedbackParams param);
    Task<ApiResponse> ResponseFeedback(ResponseFeedbackParams param);

}
