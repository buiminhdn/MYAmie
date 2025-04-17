using Common.Pagination;

namespace Common.ViewModels.FeedbackVMs;
public class PagedFeedbacksVM
{
    public IEnumerable<FeedbackVM> Feedbacks { get; set; }
    public double AverageRating { get; set; }
    public int TotalFeedback { get; set; }
    public PaginationData Pagination { get; set; }
}
