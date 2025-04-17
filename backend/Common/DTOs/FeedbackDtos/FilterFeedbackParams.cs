using Common.DTOs.Core;

namespace MYAmie.Common.DTOs.FeedbackDtos;
public class FilterFeedbackParams : PaginationParams
{
    /// <summary>
    /// The id of the target of the feedback (user, service, or place)
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Status of the feedback (RESPONSE, UNRESPONSE)
    /// </summary>
    public bool? IsResponded { get; set; }
    public int? Rate { get; set; }
}


