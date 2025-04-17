using Common.DTOs.Core;
using Models.Feedbacks;

namespace Common.DTOs.FeedbackDtos;
public class AddFeedbackParams : BaseParams
{
    public int TargetId { get; set; }
    public FeedbackTargetType TargetType { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; }
}
