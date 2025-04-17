namespace Common.ViewModels.UserVMs;
public class BusinessVM
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string OperatingHours { get; set; }
    public string City { get; set; }
    public string Cover { get; set; }

    // Feedback
    public int AverageRating { get; set; }
    public int TotalFeedback { get; set; }
}
