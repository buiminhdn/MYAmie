using Common.DTOs.Core;

namespace Common.DTOs.FeedbackDtos;
public class UpdateFeedbackParams : BaseParams
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; }
}
