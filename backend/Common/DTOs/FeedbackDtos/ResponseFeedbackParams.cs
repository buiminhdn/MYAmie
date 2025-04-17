using Common.DTOs.Core;

namespace Common.DTOs.FeedbackDtos;
public class ResponseFeedbackParams : BaseParams
{
    public int Id { get; set; }
    public string Message { get; set; }
}
