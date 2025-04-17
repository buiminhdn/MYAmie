namespace Common.ViewModels.FeedbackVMs;
public class FeedbackInfoVM
{
    public IEnumerable<FeedbackVM> Feebacks { get; set; }
    public double AverageRating { get; set; }
    public int TotalFeedback { get; set; }
}
