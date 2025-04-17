namespace Common.ViewModels.ProfileVMs;
public class UserProfileVM
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
    public string Cover { get; set; }
    public string DateOfBirth { get; set; }
    public ICollection<string> Images { get; set; }
    public ICollection<string> Characteristics { get; set; }

    public int CityId { get; set; }
    public ICollection<int> CategoryIds { get; set; }
}
